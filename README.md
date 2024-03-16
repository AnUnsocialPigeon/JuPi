# JuPi
*Author: An Unsocial Pigeon*

## Installation Guide
Run the following shell script in your terminal (this assumes the existence of .NET 8.0 or later.):
```sh
#!/bin/bash

TEMP_DIR=$(mktemp -d)
OUTPUT_DIR="JuPi"
EXECUTABLE_NAME="JuPi.exe"

echo "Cloning repository..."
git clone https://github.com/AnUnsocialPigeon/JuPi.git $TEMP_DIR

echo "Building JuPi..."
dotnet publish $TEMP_DIR -c Release -o $OUTPUT_DIR

echo "Adding JuPi to PATH..."
sudo ln -sf $OUTPUT_DIR/$EXECUTABLE_NAME /usr/local/bin/JuPi

echo "Installation complete. You can now run the JuPi using 'JuPi' command."
```

I hope this works!

## Usage Guide
Run the following in the terminal once installed:
```sh
JuPi {optional: overridden config.txt path}
```
Configurable options include:
- `-h` or `--help` for help
- {More to be added soon!}

### How to play
- To play the game, start writing PI! 
 - Green digits = good, red digits = bad.
- To finish, press `Enter`.
- For a hint, press `h`

The first run, the program will require access to the internet in order to download PI. 
*(After the first run; no more internet required for download)*

## Files
This project will load a config.txt file. This is where all of the main options about the program are stored.
Maybe in future there will be some more args, like username overriding, but, not yet... That's on the TODO.

leaderboard.csv is a csv of all previous attempts.

pi.txt is the "answer". Preferably don't touch this... 

## Final notes:
This program has not been tested on Linux yet. As it is a .NET Application, there may be unforseen bugs. Please report any :)