@echo off
pushd %~dp0
setlocal
call "%SYNERGYDE64%dbl\dblvars64.bat"
echo Generating DBL Wrappers for C# code...
gennet40 -output NETSRC:interop.dbl EXE:CSharpInterop.dll
endlocal
popd