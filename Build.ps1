[CmdletBinding()]
param (
    [Parameter(HelpMessage="The action to execute.")]
    [ValidateSet("Build", "Test", "Pack")]
    [string] $Action = "Build",

    [Parameter(HelpMessage="The msbuild configuration to use.")]
    [ValidateSet("Debug", "Release")]
    [string] $Configuration = "Debug",

    [switch] $NoRestore,

    [switch] $Clean
)

function RunCommand {
    param ([string] $CommandExpr)
    Write-Verbose "  $CommandExpr"
    Invoke-Expression $CommandExpr
}

$projectName = "Danom"
$rootDir = $PSScriptRoot
$srcDir = Join-Path -Path $rootDir -ChildPath 'src'
$testDir = Join-Path -Path $rootDir -ChildPath 'test'

switch ($Action) {
    "Test"        { $projectdir = Join-Path -Path $testDir -ChildPath "$projectName.Tests" }
    "Pack"        { $projectDir = Join-Path -Path $srcDir -ChildPath "$projectName" }
    Default       { $projectDir = Join-Path -Path $srcDir -ChildPath "$projectName" }
}

if(!$NoRestore.IsPresent) {
    RunCommand "dotnet restore $projectDir --force --force-evaluate --nologo --verbosity quiet"
}

if ($Clean)
{
    RunCommand "dotnet clean $projectDir -c $Configuration --nologo --verbosity quiet"
}

switch ($Action) {
    "Test"        { RunCommand "dotnet test `"$projectDir`"" }
    "Pack"        { RunCommand "dotnet pack `"$projectDir`" -c $Configuration --include-symbols --include-source" }
    Default       { RunCommand "dotnet build `"$projectDir`" -c $Configuration" }
}
