const http = require('http');
const url = require('url');
const WebSocket = require('ws');
const { v4: uuidv4 } = require('uuid');
const Buffer = require('buffer').Buffer;

const DataHasMap = require('./DataSaveBank.js').DataObj;
const meakUser = require('./User.js').meakUser;
const Lobby = require('./Lobby.js').LobbyObj;


const server = http.createServer();
const port = process.env.PORT || 3000;

const wss = new WebSocket.Server({ noServer: true, clientTracking: true});


let users = new DataHasMap();
let lobbys = new DataHasMap();


wss.on('connection', function connection(ws, request) {
  console.log(new Date() + ' | A new client connected.');

  //var uuidPlayer = uuidv4();

  /*let sessionMsg = {};
  sessionMsg.type = "register";
  sessionMsg.method = "register";

  //let playerKey = request.header['sec-websocket-key'];
  //sessionMsg.sessionId = playerKey;
  sessionMsg.uuidPlayer = uuidPlayer;

  ws.send(JSON.stringify(sessionMsg));*/

  //users.addData()


  ws.on('message', function(dataString) {






    //console.log('Message: '+data);


    //var buf = new Uint8Array(data).buffer;
//var dv = new DataView(buf);
    var data = JSON.parse(dataString);
    var code = Number(data.packetType);


    switch (code) {
      case 1:
      // recive: Handshake	1:	username
      // send: 1 sessionId("id"), welcomeMessage

        var pakket = makePacket(1);
        var username = data.username;
        var uuidPlayer = uuidv4();
        users.addData(username, meakUser(username,null,null,uuidPlayer,ws));

        pakket.sessionId = uuidPlayer;
        pakket.welcomeMessage = "Welcome "+username+", your accont has been reristerd to play a game";


        break;
      case 2:
      // recive: GetPublicLobbys	2:	username
      // send: 2 sessionId("id"), welcomeMessage

        var pakket = makePacket(2);


        var existingLobbys = lobbys.getAllData();
        var publicLobbyCodes = [];
        console.log(existingLobbys);
        for (var i = 0; i < existingLobbys.length; i++) {
          console.log(existingLobbys[i][0][1].isPublic);
          if(existingLobbys[i][0][1].isPublic == 1){
            console.log(existingLobbys[i][0][1].getLobbyCode());
            publicLobbyCodes.push(existingLobbys[i][0][1].getLobbyCode());
          }
        }
        pakket.lobbyCodes = publicLobbyCodes;


        break;
      case 3:
      // recive: joinRequest	3:	username, lobbyCode
      // send: 3 succes (0 of 1)
        var pakket = makePacket(3);

        var username = data.username;
        var lobbyCode = data.lobbyCode;

        lobbys.getData(lobbyCode).addUser(username,users.getData(username));

        pakket.succes = 1;

        //console.log(users.getData(username));
        //users.getData(username).ws.send("hello");




        break;
      case 4:
      // recive: ReadyUp	4:	lobbyCode,username
      // send: 1 color



        var username = data.username;
        users.getData(username).readyState = true;
        lobbys.getData(users.getData(username).lobbyCode).setReady(username,true);

        if(lobbys.getData(users.getData(username).lobbyCode).checkAllReady){

          console.log("All users are ready for the game of "+users.getData(username).lobbyCode);

          var pakket = makePacket(4);
          var lobbyUsers = lobbys.getData(users.getData(username).lobbyCode).getAllUsers();

          console.log(lobbyUsers);

          for (var i = 0; i < lobbyUsers.length; i++) {
            pakket.color = i;
            console.log(lobbyUsers[i]);
            lobbyUsers[i][0][1].ws.send(JSON.stringify(pakket));

          }
        }
        var pakket = null;


        break;
      case 5:
      // recive: Movment	5:	startPos, endPos
      // send: 5 startPos, endPos

        var pakket = null;
        var username = data.username;
        console.log(users.getData(username).lobbyCode);
        var clients = lobbys.getData(users.getData(username).lobbyCode).getAllUsers();

        clients.forEach(function each(client) {
          if (client.ws !== ws && client.ws.readyState === WebSocket.OPEN) {
            client.ws.send(data);
          }
        });



        break;

      case 6:
        // recive: createLobby	6:	username, isPublic
        // send: 6 lobbyCode

          var pakket = makePacket(6);

          var username = data.username;
          var isPublic = Number(data.isPublic);
          var newLobby = new Lobby(isPublic);
          newLobby.addUser(username, users.getData(username));

          var code = newLobby.getLobbyCode();
          lobbys.addData(code, newLobby);

          pakket.lobbyCode = code;
          users.getData(username).lobbyCode = code;
          break;

      default:

        var pakket = makePacket("-1");

    }
    if(pakket){
      ws.send(JSON.stringify(pakket));
    }



    /*if( Number(code) == 0){
      var startPos = data.startPos;
      var endPos = data.endPos;
      console.log("start pos:"+startPos.x+" "+startPos.y);
      console.log("end pos:"+endPos.x+" "+endPos.y);
    }*/




    //ws.send(msgString);
    //const buf = Buffer.from(data.data);
    //console.log(buf.readInt32BE(0));
    //console.log(buf.readInt32BE(4));
    /*

    wss.clients.forEach(function each(client) {
      if (client !== ws && client.readyState === WebSocket.OPEN) {
        client.send(dataString);
      }
    });

    */


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



function makePacket(pakketType) {
  return pakket = {
    packetType:pakketType
  };
}
