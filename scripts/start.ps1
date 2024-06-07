$webAppPath = "..\Valuator\"
$urls = "http://localhost:5001", "http://localhost:5002"

$nginxPath = "..\nginx"

$rankCalculatorAppPath = "..\Valuator.RankCalculator\"
$eventsLoggerAppPath = "..\Valuator.EventsLogger\"

# Run Application for each specified url
foreach ( $url in $urls ) {
    Start-Process `
        -FilePath "dotnet" `
        -ArgumentList "run --no-build --urls `"$url`"" `
        -WorkingDirectory $webAppPath 
    Write-Host "App started. Url: $url"
}

# Run nginx
Start-Process `
    -FilePath "nginx.exe" `
    -WorkingDirectory $nginxPath
Write-Host "Nginx started"

# Run rank calculator
Start-Process `
    -FilePath "dotnet" `
    -ArgumentList "run --no-build" `
    -WorkingDirectory $rankCalculatorAppPath 
Write-Host "Rank calculator app started"

# Run events logger
Start-Process `
    -FilePath "dotnet" `
    -ArgumentList "run --no-build" `
    -WorkingDirectory $eventsLoggerAppPath
Write-Host "Events logger app started"