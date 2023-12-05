$hostURL = "https://salearningjoestockapi.azurewebsites.net"
$testStatus = "PASS"

try {
    # Create Test profile
    $body = @{
        Email = "testaccount@msn.com"
        Name = "testaccount"
        Description = "test account"
        AccountType = "CUSTOM"
    } | ConvertTo-Json

    Invoke-RestMethod -Uri $hostURL/api/profile -Method POST -Body $body

    # Get Test profile
    Invoke-RestMethod -Uri $hostURL/api/profile/testaccount@msn.com

    # Delete Test Profile
    Invoke-RestMethod -Uri $hostURL/api/profile/testaccount@msn.com -Method DELETE
}
catch {
    $testStatus = "FAIL"
}
finally {
    $Results = Get-Content -Raw -Path ".\TestData.json" | ConvertFrom-Json -NoEnumerate
    $Results += @{
        Date = (Get-Date)
        Status = $testStatus
        Description = "Nightly Test"
        Environment = "QA"
    }
    $tempResults = ConvertTo-Json $Results
    $tempResults | Set-Content -Path ".\TestData.json"
    "data = " + $tempResults | Set-Content -Path ".\TestData.js"
}