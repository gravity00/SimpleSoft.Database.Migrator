$ErrorActionPreference = "Stop"

$assemblyVersion = "1.0.0"
$assemblyFileVersion = "1.0.0.17251"
$assemblyInformationalVersion = "1.0.0-dev001"
$nugetsDestinationPath = "..\nuget-builds\$($assemblyInformationalVersion)"

Write-Host "Making a major cleanup..."

if(Test-Path $nugetsDestinationPath){
    Remove-Item $nugetsDestinationPath -Recurse
}
New-Item $nugetsDestinationPath -ItemType Directory

Get-ChildItem -Path ".." -Recurse | 
Where-Object {$_.Name -eq "bin" -or $_.Name -eq "obj" -or $_.Name -eq "project.lock.json"} |
ForEach-Object{
    Write-Host "Deleting $($_.FullName)..."
    Remove-Item $_.FullName -Recurse
}

$xprojFiles = Get-ChildItem -Path ".." -Recurse | Where-Object {$_.Name -like "*.xproj"}

Write-Host "Generating AssemblyInfoVersions files..."
$assemblyInfoVersionsContentText = @"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a powershell script.
//
//     Used to define the assembly version information.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("SimpleSoft")]
[assembly: AssemblyProduct("SimpleSoft.Database")]
[assembly: AssemblyCopyright("Copyright © 2017 João Simões")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(false)]


[assembly: AssemblyVersion("$($assemblyVersion)")]
[assembly: AssemblyFileVersion("$($assemblyFileVersion)")]
[assembly: AssemblyInformationalVersion("$($assemblyInformationalVersion)")]
"@
$xprojFiles | Where-Object {$_.FullName -like "*src*"} | ForEach-Object {
    $assemblyInfoVersionsFilePath = "$($_.Directory.FullName)/Properties/AssemblyInfoBase.cs"
    Write-Host "Creating file $($assemblyInfoVersionsFilePath) with assembly file version..."
    $assemblyInfoVersionsContentText | Set-Content $assemblyInfoVersionsFilePath

    $projectJsonFilePath = "$($_.Directory.FullName)/project.json"
    Write-Host "Changing version in file $($projectJsonFilePath)..."
    $projectJsonContent = Get-Content $projectJsonFilePath -Raw -Encoding UTF8
    ($projectJsonContent -replace '"version": "[0-9a-zA-Z.-]+",', """version"": ""$($assemblyInformationalVersion)"",").Trim() | Set-Content $projectJsonFilePath -Encoding UTF8
}

Write-Host "Restoring all packages..."
$xprojFiles | ForEach-Object  {
    Write-Host "Restoring packages for $($_.FullName)..."
    dotnet.exe restore $_.DirectoryName --no-cache
}

Write-Host "Building all projects..."
$xprojFiles | ForEach-Object  {
    Write-Host "Building project $($_.FullName)..."
    dotnet.exe build $_.DirectoryName -c Release
}

Write-Host "Running all tests..."
$xprojFiles | Where-Object {$_.FullName -like "*test*"} | ForEach-Object  {
    Write-Host "Tunning tests from $($_.FullName)..."
    dotnet.exe test $_.DirectoryName
}

Write-Host "Packing all NuGets..."
$xprojFiles | Where-Object {$_.FullName -like "*src*"} | ForEach-Object  {
    Write-Host "Packing NuGet of $($_.FullName)..."
    dotnet.exe pack $_.DirectoryName -c Release -o $($nugetsDestinationPath)
}
