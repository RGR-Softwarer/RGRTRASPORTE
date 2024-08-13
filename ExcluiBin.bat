@echo off
setlocal

REM Muda para o diretório onde o script está localizado
cd /d %~dp0

REM Encontrar e excluir todas as pastas bin
for /d /r %%G in (bin) do (
    if exist "%%G" (
        echo Deletando pasta: %%G
        rd /s /q "%%G"
    )
)

REM Encontrar e excluir todas as pastas obj
for /d /r %%G in (obj) do (
    if exist "%%G" (
        echo Deletando pasta: %%G
        rd /s /q "%%G"
    )
)

echo Todas as pastas 'bin' e 'obj' foram excluídas.
endlocal
pause
