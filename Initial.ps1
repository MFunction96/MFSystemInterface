$parent = Split-Path -Parent $MyInvocation.MyCommand.Definition
$path = $parent + "\MFSystemInterface\ref\MFStandardUtil"
git clone -q --branch=master https://github.com/MFunction96/MFStandardUtil.git $path
nuget restore
dotnet restore
