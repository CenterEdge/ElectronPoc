import { app, ipcMain, BrowserWindow } from 'electron';
import { createServer, Server } from 'net';
import { EventEmitter } from 'events';

let server: Server;
const Int32Size = 4;


interface Message {
  name: string;
  body?: any;
}

type MessageHandler = (message: string, body: any) => void;

interface MessageHandlerSet {
  [command: string]: MessageHandler;
}

export const startServer = (win: BrowserWindow, pipeName: string) => {
  const messageSource = new EventEmitter();

  const messageNames: MessageHandlerSet = {
    show: () => {
      win.show();
      win.focus();

      sendMessage({
        name: 'window-shown',
        body: {
          hwnd: [...win.getNativeWindowHandle()]
        }
      });
    },
    hide: () => win.hide(),
    close: () => app.quit(),
    actions: (_, body) => {
      console.log(body.actions);
      win.webContents.send('actions', body.actions);
    }
  };

  function processMessage(name: string, body: any) {
    if (messageNames[name]) {
      messageNames[name](name, body);
    }
  }

  function sendMessage(message: Message) {
    const responseBuffer = Buffer.from(JSON.stringify(message), 'utf8');
    const responseWithSizeBuffer = Buffer.alloc(responseBuffer.length + Int32Size);
    responseWithSizeBuffer.writeInt32LE(responseBuffer.length, 0);
    responseBuffer.copy(responseWithSizeBuffer, Int32Size);

    messageSource.emit('message', responseWithSizeBuffer);
  }

  ipcMain.on('actions', (_, actions) => {
    sendMessage({
      name: 'actions',
      body: {
        actions
      }
    });
  });

  server = createServer((stream) => {
    let buffer: Buffer;
    let packetLength: number;

    stream.on('data', c => {
      if (!buffer) {
        buffer = c;
      } else {
        buffer = Buffer.concat([buffer, c], buffer.length + c.length);
      }

      if (!packetLength && buffer.length >= Int32Size) {
        packetLength = buffer.readInt32LE(0);
      }

      if (packetLength && buffer.length >= packetLength + Int32Size) {
        try {
          const message: Message = JSON.parse(buffer.toString('utf8', Int32Size, packetLength + Int32Size));
          console.log(message);

          if (message.name) {
            processMessage(message.name, message.body);
          }
        } catch (e) {
          console.log(e);
        }

        if (packetLength + Int32Size >= buffer.length) {
          buffer = null;
        } else {
          buffer = buffer.slice(packetLength + Int32Size);
        }

        packetLength = undefined;
      }
    });

    const messageHandler = (message: Buffer) => {
      stream.write(message);
    };
    messageSource.addListener('message', messageHandler);

    stream.on('close', () => {
      messageSource.removeListener('message', messageHandler);
    });
  });

  server.listen(`\\\\.\\pipe\\${pipeName}`);
  console.log('Listening...');
};

export const stopServer = () => {
  if (server) {
    server.close();
  }
};
