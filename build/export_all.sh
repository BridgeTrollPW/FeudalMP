#!/bin/bash
Godot_v3.2.3-stable_mono_x11.64 --path ../ --export "Windows Desktop" ./../export/win64/release/FeudalMP
Godot_v3.2.3-stable_mono_x11.64 --path ../ --export-debug "Windows Desktop" ./../export/win64/debug/FeudalMP
Godot_v3.2.3-stable_mono_x11.64 --path ../ --export "Linux/X11" ./../export/x11/release/FeudalMP
Godot_v3.2.3-stable_mono_x11.64 --path ../ --export-debug "Linux/X11" ./../export/x11/debug/FeudalMP