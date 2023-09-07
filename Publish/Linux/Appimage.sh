#!/bin/bash


ORIGIN="$(pwd)"
cd "../.."
if [ -d "bin/Release/net6.0/publishLinux" ]
then
    rm -rf "bin/Release/net6.0/publishLinux"
fi
mkdir -p "bin/Release/net6.0/publishLinux"
dotnet publish -r linux-x64 -c Release -p:SelfContained=True -o bin/Release/net6.0/publishLinux
cd "$ORIGIN"

cd "Sources"
unzip -o appimagetool-x86_64.AppImage.zip
cd "$ORIGIN"
chmod +x Sources/appimagetool-x86_64.AppImage

APP_NAME="Permastead"
APP_OUTPUT_PATH="Build"
PUBLISH_OUTPUT_DIRECTORY="../../bin/Release/net6.0/publishLinux/."
APP_TAR_NAME1="Permastead"
APP_TAR_NAME2="linux_AppImage_x86_64"

if [ -d "$APP_OUTPUT_PATH" ]
then
    rm -rf "$APP_OUTPUT_PATH"
fi

mkdir -p "$APP_OUTPUT_PATH/$APP_NAME/usr/bin"
mkdir -p "$APP_OUTPUT_PATH/$APP_NAME/usr/share/metainfo"

cp -a "$PUBLISH_OUTPUT_DIRECTORY" "$APP_OUTPUT_PATH/$APP_NAME/usr/bin/"
cp -a "Sources/AppRun" "$APP_OUTPUT_PATH/$APP_NAME/AppRun"
cp -a "Sources/permastead.desktop" "$APP_OUTPUT_PATH/$APP_NAME/permastead.desktop"
cp -a "Sources/permastead.appdata.xml" "$APP_OUTPUT_PATH/$APP_NAME/usr/share/metainfo/permastead.appdata.xml"
cp -a "Sources/icn.png" "$APP_OUTPUT_PATH/$APP_NAME/icn.png"

chmod 755 "$APP_OUTPUT_PATH/$APP_NAME/AppRun"

#cp "../../NOTICE.txt" "$APP_OUTPUT_PATH/NOTICE.txt"
#cp "../../LICENSE.txt" "$APP_OUTPUT_PATH/LICENSE.txt"

#VERSION=$(cat ../version.txt | sed 's/ *$//g' | sed 's/\r//' | sed ':a;N;$!ba;s/\n//g')
VERSION="0.1"

cd "$APP_OUTPUT_PATH"

ARCH=x86_64 ../Sources/appimagetool-x86_64.AppImage -n "$APP_NAME"

tar -czvf "$APP_TAR_NAME1$VERSION$APP_TAR_NAME2.tar.gz" "Permastead-x86_64.AppImage" 
mv "$APP_TAR_NAME1$VERSION$APP_TAR_NAME2.tar.gz" ../../"$APP_TAR_NAME1$VERSION$APP_TAR_NAME2.tar.gz"
