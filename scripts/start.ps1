$appPath = "..\Valuator\"
$nginxPath = "..\nginx"

$urls = "http://localhost:5001", "http://localhost:5002"

# Run Application for each specified url
foreach ( $url in $urls ) {
    Start-Process `
        -FilePath "dotnet" `
        -ArgumentList "run --no-build --urls `"$url`"" `
        -WorkingDirectory $appPath 
    Write-Host "App started. Url: $url"
}

# Run nginx
Start-Process `
    -FilePath "nginx.exe" `
    -WorkingDirectory $nginxPath
Write-Host "Nginx started"