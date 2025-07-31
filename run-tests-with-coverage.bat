@echo off
echo 🧪 Executando testes com cobertura de código...
echo ===============================================

echo ⚡ Limpando builds anteriores...
dotnet clean > nul 2>&1

echo 🔨 Compilando projeto...
dotnet build --verbosity minimal
if %ERRORLEVEL% neq 0 (
    echo ❌ Erro na compilação!
    pause
    exit /b 1
)

echo 🚀 Executando testes com cobertura...
dotnet test --collect:"XPlat Code Coverage" --settings:runsettings.xml --verbosity minimal --results-directory:./TestResults

echo 📊 Resultados salvos em: TestResults/
echo ✅ Cobertura gerada!

echo.
echo 💡 Para visualizar relatório detalhado:
echo    reportgenerator -reports:"TestResults/**/coverage.cobertura.xml" -targetdir:"CoverageReport" -reporttypes:"HtmlInline_AzurePipelines"

echo.
echo 📋 Arquivos de cobertura:
dir TestResults\*\*.xml /s /b 2>nul

echo.
pause