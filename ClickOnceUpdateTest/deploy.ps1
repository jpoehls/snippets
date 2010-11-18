Clear-Host

function Get-ScriptDirectory {
	$Invocation = (Get-Variable MyInvocation -Scope 1).Value
	Split-Path $Invocation.MyCommand.Path
}

function Generate-Assembly-Info {
	param(
		[string]$Title,
		[string]$Description,
		[string]$Company,
		[string]$Product,
		[string]$Copyright,
		[string]$Version,
		[string]$InfoVersion,
		[string]$File = $(throw "File is a required parameter.")
	)

	if ($InfoVersion -eq $null) {
		$InfoVersion = $Version
	}
 
	$AsmInfo = "using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: ComVisibleAttribute(false)]
[assembly: AssemblyTitleAttribute(""$title"")]
[assembly: AssemblyDescriptionAttribute(""$description"")]
[assembly: AssemblyCompanyAttribute(""$company"")]
[assembly: AssemblyProductAttribute(""$product"")]
[assembly: AssemblyCopyrightAttribute(""$copyright"")]
[assembly: AssemblyVersionAttribute(""$version"")]
[assembly: AssemblyInformationalVersionAttribute(""$infoVersion"")]
[assembly: AssemblyFileVersionAttribute(""$version"")]
[assembly: AssemblyDelaySignAttribute(false)]"
 
	$Dir = [System.IO.Path]::GetDirectoryName($File)
	if ([System.IO.Directory]::Exists($Dir) -eq $false) {
		Write-Host "Creating directory $Dir"
		[System.IO.Directory]::CreateDirectory($Dir)
	}
	(Get-Item $File).Attributes = 'Normal'
	Write-Output $AsmInfo > $file
}

# set the version we are publishing
$Version = Read-Host "Enter the build number you want (1.0.0.X)"
$Version = "1.0.0.$Version"

# setup the directory paths we will need
$ScriptDir = Get-ScriptDirectory
$PublishRoot = "\\usws501\web\mims\site\clickonce"
$ProviderUrl = "http://usws501/clickonce"
$Publish = Join-Path $PublishRoot $Version

# generate a new AssemblyInfo.cs file
Generate-Assembly-Info `
    -File "$ScriptDir\ClickOnceUpdateTest\Properties\AssemblyInfo.cs" `
    -Title "ClickOnceUpdateTest $Version" `
    -Description "" `
    -Company "ACME" `
    -Product "ClickOnceUpdateTest $Version" `
    -Version $Version `
	-InfoVersion $Version `
    -Copyright "Copyright (c) ACME" `

# create a publish folder for this version of the app
if (Test-Path $Publish) {
	Remove-Item $Publish -Recurse -Force
}

# rebuild the app
$env:path = "$env:windir\Microsoft.NET\Framework\v4.0.30319\;$env:path"
msbuild ""$ScriptDir\ClickOnceUpdateTest.sln"" /nologo `
											   /t:Rebuild `
											   /p:Configuration=Debug `
											   /verbosity:quiet `
											   /p:OutDir=""$Publish\\""
											   
# change to the directory that contains mage.exe
Set-Location $ScriptDir
	
.\mage.exe -New Application `
			-Processor msil `
			-ToFile ""$Publish\ClickOnceUpdateTest.exe.manifest"" `
			-Name ""ClickOnceUpdateTest"" `
			-TrustLevel FullTrust `
			-Version $Version `
			-FromDirectory ""$Publish""
			
# remove any existing *.application file
if (Test-Path "$PublishRoot\ClickOnceUpdateTest.application") {
	Remove-Item "$PublishRoot\ClickOnceUpdateTest.application"
}
	
.\mage.exe -New Deployment `
			-Processor msil `
			-Install true `
			-Publisher "SGS, Inc." `
			-ProviderUrl ""$ProviderUrl/ClickOnceUpdateTest.application"" `
			-Name ""ClickOnceUpdateTest"" `
			-Version $Version `
			-AppManifest ""$Publish\ClickOnceUpdateTest.exe.manifest"" `
			-ToFile ""$PublishRoot\ClickOnceUpdateTest.application""
			
# add a .deploy extension to all files except *.application and *.manifest
foreach ($x in (dir $Publish -Exclude *.manifest -Recurse)) {
	Move-Item $x "$x.deploy"
}

# update the *.application file and set the flag (mapFileExtensions="true")
$xml = [xml](Get-Content "$PublishRoot\ClickOnceUpdateTest.application")
$xml.assembly.deployment.SetAttribute("mapFileExtensions", "true")
$xml.save("$PublishRoot\ClickOnceUpdateTest.application")
	
# copy a backup of this version's *.application file into its version folder
if (Test-Path "$PublishRoot\ClickOnceUpdateTest.application") {
	Copy-Item "$PublishRoot\ClickOnceUpdateTest.application" `
			  "$Publish\ClickOnceUpdateTest.application"
}