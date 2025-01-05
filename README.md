# Prototype Payment

This project implements REST and gRPC interfaces.

## Utilities

### Clean all bin and obj directories

Run the following command in PowerShell to remove all bin and obj folders, especially useful when recompiling SDK projects to recreate the NuGet packages.

* Open PowerShell in the folder where you saved the script.

* To allow the execution of local scripts, you may need to adjust the execution policy. For example:

´´´PowerShell
Set-ExecutionPolicy RemoteSigned -Scope CurrentUser
´´´

* Finally, run the script:

´´´PowerShell
.\Remove-BinObj.ps1 -RootDirectory "C:\Caminho\Do\Seu\Projeto"
´´´