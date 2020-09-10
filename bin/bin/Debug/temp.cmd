@echo off
 SetLocal EnableExtensions EnableDelayedExpansion
 copy "C:\Program Files\Crypto Pro\CSP\csptest.exe" >nul
 chcp 1251
 set path="C:\Program Files\Crypto Pro\CSP"
if exist %computername%.txt del /f /q %computername%.txt
 if exist temp.txt del /f /q temp.txt
 set NameK="" 
 for /f "usebackq tokens=3,4* delims=\" %%a in (`csptest -keyset -enum_cont -fqcn -verifycontext` ) do (
 set NameK=%%a
;csptest -passwd -showsaved -container "!NameK!" >> temp.txt
 )
del /f /q csptest.exe
 set/a $ai=-1
 set/a $bi=2
 for /f "usebackq delims=" %%a in ("temp.txt") do @(set "$a=%%a"
 if "!$a:~,14!"=="AcquireContext" echo:!$a! >> %computername%.txt
if "!$a:~,8!"=="An error" echo:Увы, ключевой носитель отсутствует или пароль не был сохранен. >> %computername%.txt & echo: >> %computername%.txt
 if "!$a:~,5!"=="Saved" set/a $ai=1
if !$ai! geq 0 set/a $ai-=1 & set/a $bi-=1 & echo:!$a! >> %computername%.txt
 if !$bi!==0 echo: >> %computername%.txt & set/a $bi=2
)
 del /f /q temp.txt
 EndLocal
 echo on