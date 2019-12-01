# Electron POC

Demonstrates the use of an Electron process to show UI on demand via JSON messages sent via named pipes from .NET.

## Prepping

Clone the repo and run `npm install` from the `electron-poc` directory.

## Launching The Backing Service

To support rapid development and support Hot Module Reloading, we want to run the Electron application
using an HTTP server. This would be the case for production.

Run `npm run serve` from the `electron-poc` directory to start the server.

It will also watch and recompile the Electron main process files. However, relaunching Electron is required
for this to take effect.

## Seeing It In Action

Run the WinFormsTest project in Visual Studio to see it in action. It will launch Electron in the background
and communicate with it. Closing the app will shutdown the Electron process automatically (it's linked at the
OS level as a child process using jobs).
