param (
	[Parameter(Mandatory)] [string] $baseDir,
	[Parameter(Mandatory)] [string] $pluginDir
)

$baseDir = [System.IO.Path]::GetFullPath($baseDir)
$pluginDir = [System.IO.Path]::GetFullPath($pluginDir)

if (Test-Path -Path $pluginDir -PathType Container) {
	$files = Get-ChildItem $pluginDir -Filter *.dll
	foreach ($file in $files) {
		$baseFilePath = [System.IO.Path]::Join($baseDir, $file.Name)
		if (Test-Path -Path $baseFilePath -PathType Leaf) {
			Write-Host "Removing $($file.FullName) ..."
			Remove-Item -Path $file.FullName
		}
	}
} else {
	Write-Host "Plugin directory not found: $pluginDir"
}