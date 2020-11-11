#!/bin/bash

PATH=$PATH:'C:\Program Files\7-Zip'

mkdir big-zip
mkdir small-zips
mkdir -p small-zips-unpacked/{wpl,opr,num,vbo,pnd}

curl http://geodata.nationaalgeoregister.nl/inspireadressen/extract/inspireadressen.zip >big-zip/inspireadressen.zip

7z x -osmall-zips big-zip/inspireadressen.zip

7z x -osmall-zips-unpacked/wpl small-zips/9999WPL*
7z x -osmall-zips-unpacked/opr small-zips/9999OPR*
7z x -osmall-zips-unpacked/num small-zips/9999NUM*
7z x -osmall-zips-unpacked/vbo small-zips/9999VBO*
7z x -osmall-zips-unpacked/pnd small-zips/9999PND*