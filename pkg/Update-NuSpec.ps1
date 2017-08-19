param(
    [Parameter(mandatory=$true)][string] $Nuspec,
    [Parameter(mandatory=$true)][string] $TemplateNupkg,
    [Parameter(mandatory=$true)][string] $TemplateNuspec
)

$Nuspec = Resolve-Path $Nuspec
$TemplateNupkg = Resolve-Path $TemplateNupkg

Add-Type -Assembly System.IO.Compression.FileSystem
$ZipArchive = [System.IO.Compression.ZipFile]::OpenRead($TemplateNupkg)
$ZipArchiveEntry = $ZipArchive.GetEntry($TemplateNuspec)
$EntryStream = $ZipArchiveEntry.Open()
$TemplateXml = New-Object System.Xml.XmlDocument
$TemplateXml.Load($EntryStream)

[xml] $TargetXml = Get-Content $Nuspec
$TargetXml.package.metadata.id = $TemplateXml.package.metadata.id
$TargetXml.package.metadata.version = $TemplateXml.package.metadata.version
$TargetXml.package.metadata.title = $TemplateXml.package.metadata.title
$TargetXml.package.metadata.description = $TemplateXml.package.metadata.description
$TargetXml.package.metadata.authors = $TemplateXml.package.metadata.authors
$TargetXml.package.metadata.owners = $TemplateXml.package.metadata.owners
$TargetXml.package.metadata.copyright = $TemplateXml.package.metadata.copyright

$newDependencies = $TargetXml.ImportNode($TemplateXml.package.metadata.dependencies, $true)
$TargetXml.package.metadata.ReplaceChild($newDependencies, $TargetXml.package.metadata.dependencies)

$TargetXml.Save($Nuspec)

$EntryStream.Dispose()
$ZipArchive.Dispose()