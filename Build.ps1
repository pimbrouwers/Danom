[CmdletBinding()]
param (
  [Parameter(HelpMessage = "The action to execute.")]
  [ValidateSet("Build", "Test", "Pack")]
  [string] $Action = "Build",

  [Parameter(HelpMessage = "The msbuild configuration to use.")]
  [ValidateSet("Debug", "Release")]
  [string] $Configuration = "Debug",

  [Parameter(HelpMessage = "The project to reference.")]
  [string] $Project,

  [switch] $NoRestore,

  [switch] $Clean
)

function RunCommand {
  param ([string] $CommandExpr)
  Write-Verbose "  $CommandExpr"
  Invoke-Expression $CommandExpr
}

$rootDir = $PSScriptRoot
$actionDir = $rootDir

switch ($Action) {
  { "Pack" -eq $_ } {
    if (!$Project) {
      Write-Error "The project parameter is required when packing."
      exit 1
    }
    $actionDir = Join-Path -Path $rootDir -ChildPath $Project
  }
  Default {
    if ($Project) {
      $actionDir = Join-Path -Path $rootDir -ChildPath $Project
    }
  }
}

if (!$NoRestore.IsPresent) {
  RunCommand "dotnet restore $actionDir --force --force-evaluate --no-cache --nologo --verbosity quiet"
}

if ($Clean) {
  RunCommand "dotnet clean $actionDir -c $Configuration --nologo --verbosity quiet"
}

switch ($Action) {
  "Test" { RunCommand "dotnet test `"$actionDir`"" }
  "Pack" { RunCommand "dotnet pack `"$actionDir`" -c $Configuration --include-symbols --include-source" }
  Default { RunCommand "dotnet build `"$actionDir`" -c $Configuration" }
}
