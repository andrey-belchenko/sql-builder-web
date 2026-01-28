# Script to regenerate ANTLR parser files
# Usage: .\regenerate_antlr.ps1 [path-to-antlr-jar]

param(
    [string]$AntlrJar = ""
)

$ErrorActionPreference = "Stop"

# Try to find ANTLR jar in common locations
if ([string]::IsNullOrEmpty($AntlrJar)) {
    $possiblePaths = @(
        "$PSScriptRoot\antlr-4.13.1-complete.jar",
        "$env:USERPROFILE\.antlr\antlr-4.13.1-complete.jar",
        "$env:LOCALAPPDATA\antlr\antlr-4.13.1-complete.jar",
        "C:\tools\antlr\antlr-4.13.1-complete.jar"
    )
    
    foreach ($path in $possiblePaths) {
        if (Test-Path $path) {
            $AntlrJar = $path
            Write-Host "Found ANTLR jar at: $AntlrJar"
            break
        }
    }
}

if ([string]::IsNullOrEmpty($AntlrJar) -or -not (Test-Path $AntlrJar)) {
    Write-Host "ANTLR jar not found. Please download from: https://www.antlr.org/download/antlr-4.13.1-complete.jar"
    Write-Host "Or specify path: .\regenerate_antlr.ps1 -AntlrJar 'C:\path\to\antlr-4.13.1-complete.jar'"
    exit 1
}

$GrammarDir = "$PSScriptRoot\SqlBuilderLib\Grammars"
$OutputDir = "$PSScriptRoot\SqlBuilderLib.Generated"

Write-Host "Regenerating ANTLR parser files..."
Write-Host "Grammar directory: $GrammarDir"
Write-Host "Output directory: $OutputDir"
Write-Host ""

# Generate parser files
java -cp "`"$AntlrJar`"" org.antlr.v4.Tool `
    -Dlanguage=CSharp `
    -o "$OutputDir" `
    "$GrammarDir\PlSqlLexer.g4" `
    "$GrammarDir\PlSqlParser.g4"

if ($LASTEXITCODE -eq 0) {
    Write-Host "`nParser files regenerated successfully!"
    Write-Host "Now build the project: dotnet build SqlBuilderLib.Generated\SqlBuilderLib.Generated.csproj"
} else {
    Write-Host "`nError regenerating parser files. Exit code: $LASTEXITCODE"
    exit $LASTEXITCODE
}
