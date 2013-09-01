@echo off
set url=%1
set ref="%tmp%/IECapt/%url%-edge.png"

if not exist %ref% IEScreenCapt.exe -u=%url% -ie="edge" -df="no" -dg="yes" -s="yes"

IEScreenCapt.exe 	-u=%url% 	-ie="quirks" 	-df="yes" -rf=%ref% -dg="yes" -s="yes"
IEScreenCapt.exe 	-u=%url% 	-ie="7" 		-df="yes" -rf=%ref% -dg="yes" -s="yes"
IEScreenCapt.exe 	-u=%url% 	-ie="8" 		-df="yes" -rf=%ref% -dg="yes" -s="yes"
IEScreenCapt.exe 	-u=%url% 	-ie="8+" 		-df="yes" -rf=%ref% -dg="yes" -s="yes"
IEScreenCapt.exe 	-u=%url% 	-ie="9" 		-df="yes" -rf=%ref% -dg="yes" -s="yes"
IEScreenCapt.exe 	-u=%url% 	-ie="10" 		-df="yes" -rf=%ref% -dg="yes" -s="yes"
