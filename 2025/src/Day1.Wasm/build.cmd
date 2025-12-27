@echo off
setlocal enabledelayedexpansion

echo ===================================
echo Building Day1 WASM Plugin
echo ===================================

pushd "%~dp0"

echo.
echo Step 1: Restoring NuGet packages...
dotnet restore
if errorlevel 1 goto :error

echo.
echo Step 2: Building and publishing the WASM component...
dotnet publish -c Release
if errorlevel 1 goto :error

echo.
echo Step 3: Setting up demo application...
cd demo-app

if not exist "node_modules" (
    echo Installing npm dependencies...
    npm install
    if errorlevel 1 goto :error
)

echo.
echo Step 4: Transpiling WASM component to JavaScript...
call npm run transpile
if errorlevel 1 goto :error

echo.
echo ===================================
echo Build Complete!
echo ===================================
echo.
echo The WASM file is located at:
echo   bin\Release\net9.0\wasi-wasm\publish\Day1.Wasm.wasm
echo.
echo To run the demo application:
echo   cd demo-app
echo   npm run dev
echo.

popd
goto :eof

:error
echo.
echo Build failed with error code %errorlevel%
popd
exit /b %errorlevel%
