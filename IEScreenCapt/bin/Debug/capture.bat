@echo off
set url=%1
set o="tmp/ie-tester"
set ref="tmp/ref.png"

if not exist %ref% IEScreenCapt.exe -u=%url% -o=%ref% -ie="edge" -df="no"

rem IEScreenCapt.exe -u=%url% -o="%o%-quirks.png" -ie="quirks" -df="yes"
IEScreenCapt.exe -u=%url% -o="%o%-ie7.png" -ie="7" -df="yes" 
rem IEScreenCapt.exe -u=%url% -o="%o%-ie8.png" -ie="8" -df="yes"
rem IEScreenCapt.exe -u=%url% -o="%o%-ie8+.png" -ie="8+" -df="yes"
rem IEScreenCapt.exe -u=%url% -o="%o%-ie9-.png" -ie="9" -df="yes"
