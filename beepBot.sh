# File: beepBot.sh
# Project: beepBot-fluxer
# Created Date: 2026-04-09 22:52:41
# Author: 3urobeat
#
# Last Modified: 2026-04-09 22:52:41
# Modified By: 3urobeat
#
# Copyright (c) 2026 3urobeat <https://github.com/3urobeat>
#
# This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
# This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
# You should have received a copy of the GNU General Public License along with this program. If not, see <https://www.gnu.org/licenses/>.

#!/bin/bash


log() {
    echo "[beepBot] $1"
}

build() {
    log "Building..."
    dotnet build
}

clean() {
    log "Cleaning..."
    rm -rf ./bin ./obj
}

run() {
    log "Starting beepBot..."
    dotnet run
}

dev() {
    log "Starting beepBot in dev mode..."
    source ../env_tokens.sh
    run
}

help() {
  cat <<EOF
beepBot.sh

Usage: $0 [command]

Commands:
  build        Run build
  clean        Run clean
  run, start   Run bot
  dev          Run on dev machine
  help, -h, --help, -?   Show this help

You may pass multiple commands, e.g. $0 clean build run
EOF
}

# If no args provided, show help
if [ $# -eq 0 ]; then
    help
    exit 0
fi

# Iterate through all provided arguments and dispatch
for arg in "$@"; do
    case "$arg" in
        build)     build ;;
        clean)     clean ;;
        run|start) run ;;
        dev)       dev ;;
        help|-h|--help|'-?') help ;;
        *)
            log "Unknown command: $arg" >&2
            help
            exit 1
        ;;
    esac
done
