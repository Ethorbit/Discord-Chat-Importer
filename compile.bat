for /F "tokens=*" %%A in  ( compile-list.txt ) do  (
   dotnet publish -c Release -o "./bin/Release/%%A" --self-contained false -r %%A
)

for /D %%d in ( ./bin/Release/* ) do (
	cd "./bin/Release"
	del "./%%d.zip"
	7z.exe a -tzip -mx9 -sdel "%%d.zip" "./%%d/*" 
	rmdir "./%%d"
)
