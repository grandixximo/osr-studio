$params = @{
    'PackageName' = 'OSR Studio Classic';
    'Url' = "https://github.com/grandixximo/osr-studio/releases/download/$tag/OSR-Studio-Classic-Portable.zip";
    'UnzipLocation' = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)";
    'Checksum' = $checksum;
    'ChecksumType' = 'sha256';
};

Install-ChocolateyZipPackage @params