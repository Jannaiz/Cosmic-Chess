const http = require('http');
const url = require('url');
const WebSocket = require('ws');
const { v4: uuidv4 } = require('uuid');
const Buffer = require('buffer').Buffer;

const server = http.createServer();
const port = 3000;

const wss = new WebSocket.Server({ noServer: true, clientTracking: true});





wss.on('connection', function connection(ws, request) {
  console.log(new Date() + ' | A new client connected.');

  var uuidPlayer = uuidv4();

  let sessionMsg = {};
  sessionMsg.type = "register";
  sessionMsg.method = "register";

  //let playerKey = request.header['sec-websocket-key'];
  //sessionMsg.sessionId = playerKey;
  sessionMsg.uuidPlayer = uuidPlayer;

  ws.send(JSON.stringify(sessionMsg));

[]
  ws.on('message', function(data) {
    console.log('Message: '+data);

    const buf = Buffer.from(data);
    console.log(data.readInt32BE(0));
    console.log(data.readInt32BE(4));
    //ws.send(msgString);

    wss.clients.forEach(function each(client) {
      if (client !== ws && client.readyState === WebSocket.OPEN) {

        client.send(data);
      }
    });


  });

  ws.on('close', function(connection){
      console.log(new Date()+ ' | Colosing connection for a client.');
  });




});


server.on('upgrade', function upgrade(request, socket, head) {
  console.log(new Date() + ' | Upgrading http connection to wss: url = '+request.url);
  // Parsing url from the request.
  var parsedUrl = url.parse(request.url, true, true);
  const pathname = parsedUrl.pathname
  console.log(new Date() + ' | Pathname = '+pathname);
  // If path is valid connect to the websocket.
  if (pathname === '/') {
    wss.handleUpgrade(request, socket, head, function done(ws) {
      wss.emit('connection', ws, request);
    });
  } else {
    socket.destroy();
  }
});

// On establishing port listener.
server.listen(port, function() {
  console.log(new Date() + ' | Server is listening on port ' + port);
  // Server is running.
});
