@echo off
DEL /Q C:\Users\Szymon\Desktop\Scan\Scan.txt
cd C:\Program Files\AVAST Software\Avast
ashCmd C:\Users\Szymon\Desktop\Upload --report=Scan --console
cd C:\ProgramData\AVAST Software\Avast\report
move Scan.txt C:\Users\Szymon\Desktop\Scan\Scan.txt"
DEL /Q C:\Users\Szymon\Desktop\Upload\