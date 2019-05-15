#!/bin/sh

# Colors
GREEN='\033[1;32m'
NO_COLOR='\033[0m'

# Add hooksPah to git config
git config core.hooksPath ./hooks

# Print git config for debugging purposes
git config --get-regexp ^core

printf "\n"
printf $GREEN
printf "config.hooksPath added to local git config, \npress a key to continue..."
printf $NO_COLOR
printf "\n"

read

exit 0