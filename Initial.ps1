$parent = Split-Path -Parent $MyInvocation.MyCommand.Definition
$path = $parent + "\ref\MFStandardUtil"
git clone -q --branch=master https://github.com/MFunction96/MFStandardUtil.git $path