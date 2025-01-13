<#
.SYNOPSIS
    Remove todas as pastas "bin" e "obj" de um diretório e seus subdiretórios.

.DESCRIPTION
    Este script, quando executado, percorre recursivamente o diretório especificado,
    encontrando todas as pastas cujo nome seja "bin" ou "obj". Em seguida, remove 
    essas pastas e todo seu conteúdo.

.PARAMETER RootDirectory
    Caminho do diretório raiz (pasta principal) onde a limpeza será realizada.

.EXAMPLE
    PS> .\Remove-BinObj.ps1 -RootDirectory "C:\Projetos\MeuProjeto"

.NOTES
    Autor: [Seu Nome]
    Data: 05/01/2025
#>

param(
    [Parameter(Mandatory = $true)]
    [string]$RootDirectory
)

Write-Host "Iniciando limpeza em:" $RootDirectory

try {
    # 1. Procura pastas 'bin'
    $binDirs = Get-ChildItem -Path $RootDirectory -Directory -Filter "bin" -Recurse -ErrorAction SilentlyContinue
    # 2. Procura pastas 'obj'
    $objDirs = Get-ChildItem -Path $RootDirectory -Directory -Filter "obj" -Recurse -ErrorAction SilentlyContinue

    # Combina as duas listas de diretórios
    $dirsToRemove = $binDirs + $objDirs

    # Remove duplicatas caso alguma pasta tenha sido listada mais de uma vez
    $dirsToRemove = $dirsToRemove | Select-Object -Unique

    Write-Host ("Foram encontradas {0} pastas (bin/obj) para remover." -f $dirsToRemove.Count)

    foreach ($dir in $dirsToRemove) {
        try {
            Remove-Item -Path $dir.FullName -Recurse -Force -ErrorAction Stop
            Write-Host "Removido:" $dir.FullName
        }
        catch {
            Write-Warning ("Falha ao remover pasta: {0}. Erro: {1}" -f $dir.FullName, $_.Exception.Message)
        }
    }

    Write-Host "Limpeza concluída com sucesso."
}
catch {
    Write-Error ("Ocorreu um erro inesperado: {0}" -f $_.Exception.Message)
}