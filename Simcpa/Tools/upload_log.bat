@echo off
set ftpfilename=autoftp.cfg
echo open 182.92.76.190 >"%ftpfilename%"
echo andrew>>"%ftpfilename%"
echo pengdong>>"%ftpfilename%"
echo bin>>"%ftpfilename%"
echo prompt>>"%ftpfilename%"
echo mput "C:\Program Files\Ȥ�Է�\log\*">>"%ftpfilename%"
echo bye>>"%ftpfilename%"
ftp -s:"%ftpfilename%"
del "%ftpfilename%" 