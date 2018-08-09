﻿#  
# Module manifest for module '%MODULE-NAME%'  
#  
# Generated by: Microsoft Corporation  
#  
# Generated on: %DATE%
#  

$PSDefaultParameterValues.Clear()
Set-StrictMode -Version Latest

%IMPORTED-DEPENDENCIES%

if (Test-Path -Path "$PSScriptRoot\StartupScripts")
{
    Get-ChildItem "$PSScriptRoot\StartupScripts" | ForEach-Object {
        . $_.FullName
    }
}

$FilteredCommands = %DEFAULTRGCOMMANDS%

if ($Env:ACC_CLOUD -eq $null)
{
    $FilteredCommands | ForEach-Object {
        if (!$global:PSDefaultParameterValues.Contains($_))
        {
            $global:PSDefaultParameterValues.Add($_,
                {
                    $context = Get-AzureRmContext
                    if (($context -ne $null) -and $context.ExtendedProperties.ContainsKey("Default Resource Group")) {
                        $context.ExtendedProperties["Default Resource Group"]
                    } 
                })
        }
    }
}