
# change working directory to be the directory this script lives in
$scriptPath = $MyInvocation.MyCommand.Path
$localDir = Split-Path $scriptPath
Push-Location $localDir
[Environment]::CurrentDirectory = $PWD

# moves files from TStile's \Scripts\Stile into the local \work\content
function fillContent($filePattern)
{
    $src = "..\..\..\source\TStile\Scripts\Stile"
    $dest = ".\work\content\Scripts\Stile"
    robocopy $src $dest $filePattern "/S" "/XO" "/R:2" "/W:1"
}

# move .js and .ts
fillContent "*.js"
fillContent "*.ts"

# pull version info from exe
$version = dir "..\..\..\source\TStile.NuGet.Dev\bin\Debug\TStile.NuGet.Dev.exe" | Select-Object -ExpandProperty VersionInfo
Write-Host $version

# write version to .nuspec
$nuspecPath = ".\work\TStile.nuspec"
[xml]$nuspecXML = Get-Content $nuspecPath
$nuspecXML.package.metadata.version = $version.ProductVersion.ToString()
$nuspecXML.Save($nuspecPath)

# restore working directory
Pop-Location
[Environment]::CurrentDirectory = $PWD


# change working directory to /work, a NuGet convention-based directory
$nugetWork = $localDir + "/work"
Push-Location $nugetWork
[Environment]::CurrentDirectory = $PWD

nuget "pack" "TStile.nuspec" "-OutputDirectory" "..\..\..\localNuGet"

# restore working directory
Pop-Location
[Environment]::CurrentDirectory = $PWD