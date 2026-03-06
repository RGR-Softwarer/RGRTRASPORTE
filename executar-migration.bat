@echo off
echo Executando migration AddIdaVoltaSupport...
echo.

cd /d "%~dp0"
dotnet ef database update --context TransportadorContext --project Infra.Data\Infra.Data.csproj --startup-project RGRTRASPORTE\RGRTRASPORTE.csproj

if %ERRORLEVEL% EQU 0 (
    echo.
    echo Migration executada com sucesso!
    echo.
) else (
    echo.
    echo Erro ao executar migration. Verifique a connection string e o banco de dados.
    echo.
)

pause


