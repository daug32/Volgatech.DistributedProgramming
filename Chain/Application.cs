using System.Text.Json;
using Chain.Commands;
using Sockets.Listeners;
using Sockets.Models;

namespace Chain;

public class Application
{
    private readonly Listener _listener;
    private readonly CustomNetClient _customNetClient;

    private readonly bool _isInitiator;
    private int? _currentValue = null;

    public Application(
        string currentHost,
        int currentPort,
        string nextHost,
        int nextPort,
        bool isInitiator )
    {
        _listener = new Listener( currentHost, currentPort );
        _customNetClient = new CustomNetClient( nextHost, nextPort );

        _isInitiator = isInitiator;
    }

    public void Run()
    {
        _currentValue = UserIo.AskUserForValue();

        if ( _isInitiator )
        {
            RunAsInitiator();
        }
        else
        {
            RunAsProcess();
        }
    }

    private void RunAsInitiator()
    {
        _customNetClient.SafelySend(
            nameof( SendValueCommand ),
            new SendValueCommand()
            {
                Value = _currentValue!.Value
            } );

        var alreadyReceivedValue = false;

        _listener.Listen( request =>
        {
            if ( request is null || request.RequestName != nameof( SendValueCommand ) )
            {
                return Response.Failed();
            }

            var command = JsonSerializer.Deserialize<SendValueCommand>( request.Data )!;
            _currentValue = command.Value;

            if ( !alreadyReceivedValue )
            {
                alreadyReceivedValue = true;

                Task.Run( () => _customNetClient.SafelySend(
                    nameof( SendValueCommand ),
                    new SendValueCommand
                    {
                        Value = _currentValue!.Value
                    } ) );

                return Response.Ok();
            }

            Console.WriteLine( $"Initiator. Max value: {_currentValue}" );

            return Response.Ok();
        } );
    }

    private void RunAsProcess()
    {
        _listener.Listen( request =>
        {
            if ( request is null || request.RequestName != nameof( SendValueCommand ) )
            {
                return Response.Failed();
            }

            var command = JsonSerializer.Deserialize<SendValueCommand>( request.Data )!;
            _currentValue = Math.Max( command.Value, _currentValue!.Value );

            Task.Run( () => _customNetClient.SafelySend(
                nameof( SendValueCommand ),
                new SendValueCommand
                {
                    Value = _currentValue.Value
                } ) );

            Console.WriteLine( $"Process. Received: {command.Value}. Max value: {_currentValue}" );

            return Response.Ok();
        } );
    }
}