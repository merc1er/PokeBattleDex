dotnet build .\BattleDex\BattleDex.csproj -p:Platform=x64
@if %ERRORLEVEL% equ 0 (
    echo.
    echo Executable: %CD%\BattleDex\bin\x64\Debug\net10.0-windows10.0.19041.0\win-x64\BattleDex.exe
)
