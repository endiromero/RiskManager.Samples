param($name)

$VerbosePreference = $true

Write-Verbose "Starting script"
Write-Progress -Activity "Script 1" -CurrentOperation "Loading XML" -Status "processing" -PercentComplete 0
Start-Sleep -Seconds 2
Write-Progress -Activity "Script 1" -CurrentOperation "Processing XML" -Status "processing" -PercentComplete 50
Start-Sleep -Seconds 2
Write-Progress -Activity "Script 1" -CurrentOperation "Saving XML" -Status "processing" -PercentComplete 100
Start-Sleep -Seconds 2

Get-Process -Name $name