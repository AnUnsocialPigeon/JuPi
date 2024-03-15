# JuPi
*Author: An Unsocial Pigeon*

## Installation Guide
Run the following shell script in your terminal:
```sh
#!/bin/bash

TEMP_DIR=$(mktemp -d)
OUTPUT_DIR="JuPi"
EXECUTABLE_NAME="JuPi.exe"

echo "Cloning repository..."
git clone https://github.com/AnUnsocialPigeon/JuPi.git $TEMP_DIR

echo "Building application..."
dotnet publish $TEMP_DIR -c Release -o $OUTPUT_DIR

echo "Adding application to PATH..."
sudo ln -sf $OUTPUT_DIR/$EXECUTABLE_NAME /usr/local/bin/JuPi

echo "Installation complete. You can now run the application using 'JuPi' command."
```

I hope this works!