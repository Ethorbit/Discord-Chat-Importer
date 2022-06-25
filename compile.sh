#!/bin/bash
for i in $(cat "compile-list.txt"); do
     dotnet publish -c Release -o "./bin/Release/$i" --self-contained false -r $i
done

for i in $(find "./bin/Release" -mindepth 1 -maxdepth 1 -type d); do 
     rm "$i.zip"
     7z a -tzip -mx9 -sdel "$i.zip" "$i/*"
     rm -d "$i"
done 
