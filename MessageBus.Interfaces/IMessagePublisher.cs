﻿namespace MessageBus.Interfaces;

public interface IMessagePublisher
{
    void Publish( MessageId messageId, string content );
    
    void Publish<T>( MessageId messageId, T content );
}