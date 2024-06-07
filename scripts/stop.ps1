$processNames = "Valuator", "nginx", "Valuator.RankCalculator", "Valuator.EventsLogger"

foreach ( $processName in $processNames ) {
    $process = Get-Process -Name $processName -ErrorAction SilentlyContinue

    if ( $process ) {
        Stop-Process -Name $processName -Force
        Write-Host "Stopped process: $processName"
        continue
    } 

    Write-Host "Process $processName not found"
}