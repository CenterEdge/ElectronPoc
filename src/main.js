const { app, BrowserWindow } = require('electron');
const { createServer } = require('net');

let server;

function createWindow (body) {
    // Create the browser window.
    let win = new BrowserWindow({ 
        width: 800, 
        height: 600, 
        skipTaskbar: true, 
        minimizable: false, 
        maximizable: false
    });
    win.setMenu(null);

    // and load the index.html of the app.
    win.loadFile('index.html');

    if (body && body.text) {
        win.webContents.on('did-finish-load', () => {
            win.webContents.send('setText', body);
        });
    }

    return {
        hwnd: [...win.getNativeWindowHandle()]
    }
}

const commands = {
    "close": () => app.quit(),
    "openWindow": (body) => createWindow(body)
}

function processCommand(command, body) {
    if (commands[command]) {
        return commands[command](body);
    }
}

function startServer() {
    server = createServer((stream) => {
        /** @type Buffer */
        let buffer;

        stream.on('data', function(c) {
            if (!buffer) {
                buffer = c;
            } else {
                buffer = Buffer.concat([buffer, c]);
            }
            
            let index = buffer.findIndex((i) => i === 0);

            if (index == 0) {
                // noop
                buffer = null;
            } else if (index >= 0) {
                try {
                    let message = JSON.parse(buffer.toString('utf8', 0, index - 1));

                    if (message.command) {
                        var response = processCommand(message.command, message.body);

                        if (response) {
                            stream.write(JSON.stringify(response) + '\0', 'utf8');
                        } else {
                            stream.write('\0', 'utf8');
                        }
                    }
                } catch (e) {
                    console.log(e);
                }

                if (index >= buffer.byteLength-1) {
                    buffer = null;
                } else {
                    buffer = buffer.slice(index + 1);
                }
            }
        });
    });

    server.listen("\\\\.\\pipe\\advantage_pipe");
    console.log("Listening...");
}

app.on('ready', () => {
    startServer();
});

app.on('window-all-closed', e => e.preventDefault() );

app.on('quit', function(){
    if (server) {
        server.close();
    }
});