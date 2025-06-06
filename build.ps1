param (
    $PreRelease
)

function Clear-Folder {
    param (
        [string] $Path
    )

    if (Test-Path -Path $Path) {
        Remove-Item $Path -Recurse -Force | Out-Null
    }
    New-Item $Path -ItemType Directory | Out-Null
}

function Get-ALCompiler {
    param (
        [string] $Type,
        [string] $Nav,
        [string] $CU,
        [string] $CompilerCachePath = "C:\ALCompiler",
        $PreRelease
    )

    if ($Type -eq "Marketplace") {
        return Get-MarketplaceALCompiler -CompilerCachePath $CompilerCachePath -PreRelease $PreRelease
    }

    if ($Type -eq "Nav") {
        return Get-NavALCompiler -Nav $Nav -CU $CU -CompilerCachePath $CompilerCachePath
    }

    return $null
}

function Get-NavALCompiler {
    param (
        [string] $Nav,
        [string] $CU,
        [string] $CompilerCachePath = "C:\ALCompiler"
    )   

    $navFolder = "Nav" + $Nav
    $destPath = Join-Path -Path $CompilerCachePath -ChildPath $navFolder
    $extensionFolder = Join-Path -Path $destPath -ChildPath "ext"   
    $compilerPath = Join-Path -Path $extensionFolder -ChildPath "extension\bin"
    $compilerFilePath = Join-Path -Path $compilerPath -ChildPath "alc.exe"

    if (!(Test-Path -Path $compilerFilePath)) {
        $vsixPath = Get-NavCompilerVSIX -Nav $Nav -CU $CU
        if ($null -eq $vsixPath) {
            return $null
        }
        Expand-ALCompilerVSIX -Path $vsixPath -Destination $extensionFolder
    }

    $data = @{
        Path = $compilerPath
        Version = [System.Version]::new(0, 0, 0)
    }

    return $data
}

function Get-NavCompilerVSIX {
    param (
        [string] $Nav,
        [string] $CU
    )

    $url = Get-NavArtifactUrl -nav $Nav -country 'w1' -cu $CU
    $artifactPaths = Download-Artifacts -artifactUrl $url -includePlatform
    
    foreach ($path in $artifactPaths) {
        $vsixPath = Join-Path -Path $path -ChildPath "ModernDev\program files\Microsoft Dynamics NAV\110\Modern Development Environment\ALLanguage.vsix"
        if (Test-Path -Path $vsixPath) {
            return $vsixPath
        }
    }
    return $null
}

function Get-MarketplaceALCompilerVersion {
    param (
        [string] $Url
    )

    $urlParts = $Url.Split("/")
    $version = [System.Version]::new($urlParts.Get(6))

    return $version
}

function Get-MarketplaceALCompiler {
    param (
        [string] $CompilerCachePath,
        $PreRelease
    )

    $destPath = Join-Path -Path $CompilerCachePath -ChildPath "Marketplace"
    $extensionFolder = Join-Path -Path $destPath -ChildPath "ext"
    
    try {
        if ($PreRelease) {
            $url = Get-LatestAlLanguageExtensionUrl -allowPrerelease
        } else {
            $url = Get-LatestAlLanguageExtensionUrl
        }
        
        $lastUrl = Get-LastCompilerUrl -Path $destPath
        if ($url -ne $lastUrl) {
        
            if (!(Test-Path -Path $destPath)) {
                New-Item $destPath -ItemType Directory | Out-Null
            }

            $vsixPath = Join-Path -Path $destPath -ChildPath "extension.zip"
            if (Test-Path -Path $vsixPath) {
                Remove-Item $vsixPath -Force | Out-Null
            }

            Invoke-WebRequest -Uri $url -OutFile $vsixPath
            Expand-ALCompilerVSIX -Path $vsixPath -Destination $extensionFolder

            Set-LastCompilerUrl -Path $destPath -Url $url
        }
    }
    catch {        
    }

    $data = @{
        Path = Join-Path -Path $extensionFolder -ChildPath "extension\bin\win32"
        Version = Get-MarketplaceALCompilerVersion -Url $url
    }

    return $data
}

function Get-LastCompilerUrl {
    param (
        [string] $Path
    )
    $filePath = Join-Path -Path $Path -ChildPath "lasturl.txt"
    if (Test-Path -Path $filePath) {
        return Get-Content -Path $filePath
    }
    return ""
}

function Set-LastCompilerUrl {
    param (
        [string] $Path,
        [string] $Url
    )
    $filePath = Join-Path -Path $Path -ChildPath "lasturl.txt"
    Set-Content -Path $filePath -Value $Url -Force
}

function Expand-ALCompilerVSIX {
    param (
        [string] $Path,
        [string] $Destination
    )
    Add-Type -AssemblyName System.IO.Compression.FileSystem
    [System.IO.Compression.ZipFile]::ExtractToDirectory($Path, $Destination)
}

function Set-ExtensionPackageVersion  {
    param (
        [System.Version] $ALCompilerVersion
    )

    $packageManifest = Get-Content -Raw "package.json" -Encoding UTF8 | ConvertFrom-Json
    $packageVersion = [System.Version]::new($packageManifest.version)

    $newPackageVersion = [System.Version]::new($ALCompilerVersion.Major, $packageVersion.Minor, $ALCompilerVersion.Build)
    $newVersionText = $newPackageVersion.ToString()

    npm version $newVersionText --alow-same-version --no-git-tag-version
}

# Setup BC Container Helper
$module = Get-Module -ListAvailable -Name BCContainerHelper
if ($null -eq $module) {
    Write-Host "Installing BC Container Helper"
    Install-Module BCContainerhelper -Force
} else {
    Write-Host "Updating BC Container Helper"
    Update-Module BCContainerhelper -Force
}

Import-Module BCContainerhelper

# Download compiler libraries and update dll references
Write-Host "Downloading Latest AL Compiler from VS Code Marketplace"
$marketplaceALCompiler = Get-ALCompiler -Type "Marketplace" -PreRelease $PreRelease
$marketplaceALCompilerPath = $marketplaceALCompiler.Path

Write-Host "Downloading Nav 2018 AL Compiler from Nav Artifacts"
$nav2018ALCompiler = Get-ALCompiler -Type "Nav" -Nav "2018" -CU "cu14"
$nav2018ALCompilerPath = $nav2018ALCompiler.Path

Write-Host "Updating libraries references"
$oldLibrariesPath = "C:\Projects\MicrosoftALVersions\LatestBC\bin\win32"
$newLibrariesPath = $marketplaceALCompilerPath
$oldNav2018LibrariesPath = "C:\Projects\MicrosoftALVersions\Nav2018\microsoft.al-0.13.82793\bin"
$newNav2018LibrariesPath = $nav2018ALCompilerPath

$projectFilesList = Get-ChildItem -Path "language-server" -Filter "*.csproj" -Recurse
foreach ($projectFile in $projectFilesList) {
    $projectContent = Get-Content -Path $projectFile.FullName
    $projectContent = $projectContent.Replace($oldLibrariesPath, $newLibrariesPath)
    $projectContent = $projectContent.Replace($oldNav2018LibrariesPath, $newNav2018LibrariesPath)
    Set-Content -Path $projectFile.FullName -Value $projectContent -Force
}

# Prepare target language server folders in the vscode extension bin folder
$darwinBinPath = ".\vscode-extension\bin\netcore\darwin"
$winBinPath = ".\vscode-extension\bin\netcore\win32"
$linuxBinPath = ".\vscode-extension\bin\netcore\linux"
$netframeworkBinPath = ".\vscode-extension\bin\netframeworknav2018"

Clear-Folder -Path $darwinBinPath
Clear-Folder -Path $winBinPath
Clear-Folder -Path $linuxBinPath
Clear-Folder -Path $netframeworkBinPath

# Build language server
cd ".\language-server\"

# Windows - .net core
Write-Host "Building Windows .net core language server"
dotnet publish ".\AZALDevToolsServer.NetCore\AZALDevToolsServer.NetCore.csproj" -r win-x64 -f net8.0 -o "..\vscode-extension\bin\netcore\win32" --self-contained true --configuration Release

# MacOS - .net core
Write-Host "Building MacOS .net core language server"
dotnet publish ".\AZALDevToolsServer.NetCore\AZALDevToolsServer.NetCore.csproj" -r osx-x64 -f net8.0 -o "..\vscode-extension\bin\netcore\darwin" --self-contained true --configuration Release

# Linux - .net core
Write-Host "Building Linux .net core language server"
dotnet publish ".\AZALDevToolsServer.NetCore\AZALDevToolsServer.NetCore.csproj" -r linux-x64 -f net8.0 -o "..\vscode-extension\bin\netcore\linux" --self-contained true --configuration Release

# Windows - .net framework for Nav2018 extension development
Write-Host "Building Windows .net framework language server for Nav 2018"
$msBuildPath = &"${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -latest -prerelease -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe
& $msBuildPath ".\AZALDevToolsServer.NetFrameworkNav2018\AZALDevToolsServer.NetFrameworkNav2018.csproj" -p:Configuration=Release -p:OutputPath="..\..\vscode-extension\bin\netframeworknav2018"

# Update readme and changelog
Write-Host "Copying readme and changelog to vscode extension folder"
cd ".."
Copy-Item -Path ".\CHANGELOG.md" -Destination ".\vscode-extension\CHANGELOG.md" -Force
Copy-Item -Path ".\README.md" -Destination ".\vscode-extension\README.md" -Force
Copy-Item -Path ".\LICENSE" -Destination ".\vscode-extension\LICENSE" -Force

# Build vscode extension
cd "vscode-extension"
if ($PreRelease) {
    Write-Host "Setting extension version to pre-release"
    Set-ExtensionPackageVersion -ALCompilerVersion $marketplaceALCompiler.Version
}
write-Host "Packaging extension..."
vsce package
cd ".."
