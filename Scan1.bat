@echo off
cd C:\Program Files (x86)\ClamWin\bin
DEL /Q C:\Users\Szymon\Desktop\Scan\Scan.txt
clamscan -r C:/Users/Szymon/Desktop/Upload -l C:/Users/Szymon/Desktop/Scan/Scan.txt
DEL /Q C:\Users\Szymon\Desktop\Upload\