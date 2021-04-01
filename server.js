const http = require('http');
const url = require('url');
const WebSocket = require('ws');
const {
  v4: uuidv4
} = require('uuid');
const Buffer = require('buffer').Buffer;

// Personal
const DataHasMap = require('./DataSaveBank.js').DataObj;
const makeUser = require('./User.js').makeUser;
const Lobby = require('./Lobby.js').LobbyObj;

// Server setup
const server = http.createServer();
// Ask port (from heroku) to run on
const port = process.env.PORT || 3000;

// Web socket server setup
const wss = new WebSocket.Server({
  noServer: true,
  clientTracking: true
});

// Temp data saving
let users = new DataHasMap();
let lobbies = new DataHasMap();
let food = new DataHasMap();

food.addData("apple", 1);
food.addData("apple2", 2);
food.addData("peer", 37);
food.addData("tomaat", 22);
food.addData("tomaate", 1);
food.addData("tomaate", 1);
food.addData("tomaatee", 1);
food.addData("tomaate", 1);
food.addData("tomaate", 1);

console.log(food.getAllData());
console.log(food.getJSON());
console.log(food.getAllValuesAsList());




// Setup the ws server
wss.on('connection', function connection(ws, request) {
  console.log(new Date() + ' | A new client connected.');


  // When data is beining asked or given



  ws.on('message', function(dataString) {
    try {


      // Read the data
      try {
        var data = JSON.parse(dataString);
      } catch (e) {
        // If not a json

        console.log(errorMessage(makeError(400, 701, "Failed to parse data.", false)));
        console.log("Data was not in a JSON format, failed to parse.");
        // Stop the message function
        return;
      }

      // The the code of the packet to know the request



      // Procces the header

      //TEM not fully implemented object

      try {

        // Get the nessesary input
        try {
          var header = data.header;
        } catch (e) {
          throw makeError(400, 703, "", true);
        }

        try {
          // Get paketType from header
          var packetType = Number(header.packetType);
        } catch (e) {
          try {
            // Loof if the type was misplaced
            var packetType = Number(data.packetType);
          } catch (e) {
            // Pakect Type not found, throw an error
            throw makeError(400, 702, "", true);
          }
        }

        // If this isn't the Handshake; indentify the user
        try {

          var username = header.username;
          if (!username) {
            throw "";
          }
        } catch (e) {
          throw makeError(400, 704, "", true);
        }
        if (packetType != 1) {

          try {
            var sessionId = header.sessionId;
            if (!sessionId) {
              throw "";
            }
          } catch (e) {
            throw makeError(400, 705, "", true);
          }


          try {
            if (!(users.getData(username).sessionId == sessionId)) {
              throw makeError(400, 705, "", true);
            }
          } catch (e) {
            throw makeError(400, 705, "", true);
          }

          // Check the session Id, so we know for sure


        }
        console.log("header loaded succesfully");
      } catch (e) {
        //if (e == "no header")
        console.log("header not loaded succesfully");
        // If there is a problem with the username throw an error
        if (e.errorCode == 705) {
          throw e;
        }
        throw e;
        //If fully implemented add return
        //return
      }




      switch (packetType) {

        // Handshake
        case 1:
          // receive: Handshake	1:	username
          // send: 1 sessionId("id"), welcomeMessage
          process.stdout.write("Received: 1 @");

          // Check the input for the username

          process.stdout.write(username);



          var packet = makePacket(1);
          var uuidPlayer = uuidv4();
          users.addData(username, makeUser(username, null, null, uuidPlayer, ws));

          packet.sessionId = uuidPlayer;
          packet.welcomeMessage = "Welcome " + username + ", your accont has been reristerd to play a game";
          process.stdout.write(" is welcome.\n");

          break;
        case 2:
          // receive: GetPublicLobbies	2:	username
          // send: 2 sessionId("id"), welcomeMessage
          process.stdout.write("@" + username + " #2");




          var packet = makePacket(2);

          //Get all the lobbies as objects
          var existingLobbies = lobbies.getAllValuesAsList();
          var publicLobbyCodes = [];

          for (var i = 0; i < existingLobbies.length; i++) {

            // If it's a public lobbby add it
            if (existingLobbies[i].isPublic == 1) {

              publicLobbyCodes.push(existingLobbies[i].getLobbyCode());
            }
          }
          packet.lobbyCodes = publicLobbyCodes;
          break;

        case 3:
          // receive: joinRequest	3:	username, lobbyCode
          // send: 3 succes (0 of 1)
          process.stdout.write("@" + username + " #3");

          var packet = makePacket(3);

          // Try reading the lobby input
          try {
            var lobbyCode = data.lobbyCode;
            if(lobbyCode == null ||lobbyCode == undefined){
              throw makeError("400", "700", "invalid Lobby code", true);
            }
          } catch (e) {
            throw makeError("400", "700", "invalid Lobby code", true);
          }

          try {

            // Add the usere to the lobby
            lobbies.getData(lobbyCode).addUser(username, users.getData(username));
            users.getData(username).lobbyCode = lobbyCode;

            packet.succes = 1;

            // Message othere players that player joined
            sendMessageExceptOne(lobbyCode, username + " joined the game!", ws);
          } catch (e) {

            packet.succes = 0;
          }

          break;
        case 4:
          // receive: ReadyUp	4:	lobbyCode,username
          // send: 1 color
          process.stdout.write("@" + username + " #4");


          // Set useres as ready
          users.getData(username).readyState = true;
          lobbies.getData(users.getData(username).lobbyCode).setReady(username, true);

          // Message all the others that he joined
          sendMessageExceptOne(users.getData(username).lobbyCode, username + " is now ready!", ws);

          // Check id the game can start
          if (lobbies.getData(users.getData(username).lobbyCode).checkAllReady()) {

            // Message that all are ready, and game can start
            console.log("All users are ready for the game of " + users.getData(username).lobbyCode);

            // Make packet
            var packet = makePacket(4);
            var lobbyUsers = lobbies.getData(users.getData(username).lobbyCode).getAllUsers();

            console.log(lobbyUsers);

            for (var i = 0; i < lobbyUsers.length; i++) {
              packet.color = i;
              console.log(lobbyUsers[i]);
              lobbyUsers[i].ws.send(JSON.stringify(packet));

            }

            sendMessage(users.getData(username).lobbyCode, "The game has begun!");
          }
          // The packed is already been send, no need to send a new
          var packet = null;


          break;
        case 5:
          // receive: Movment	5:	startPos, endPos
          // send: 5 startPos, endPos
          process.stdout.write("@" + username + " #5 ");

          // No data needs to be send back
          var packet = null;

          //console.log("user:" + username + "object: " + users.getData(username));

          //console.log("lobbyCode: " + users.getData(username).lobbyCode);
          var currentLobby = lobbies.getData(users.getData(username).lobbyCode);

          // Is the game active
          if (!currentLobby.isActive) {
            throw makeError("400", "700", " Lobby is not active", true);
          }

          //console.log("lobby " + currentLobby);
          var clients = currentLobby.getAllUsers();

          // Send to new move to all other players
          for (var i = 0; i < clients.length; i++) {
            var clientWs = clients[i].ws;
            if (clientWs !== ws && clientWs.readyState === WebSocket.OPEN) {
              clientWs.send(dataString);
            }
          }

          /*clients.forEach(function each(client) {
            if (client.ws !== ws && client.ws.readyState === WebSocket.OPEN) {
              client.ws.send(data);
            }
          });*/
          // Save them to log
          var pos1 = "(" + data.startPos.x + "," + data.startPos.y + ")";
          var pos2 = "(" + data.endPos.x + "," + data.endPos.y + ")";

          // Send this as a message
          sendMessage(users.getData(username).lobbyCode, username + " moved from " + pos1 + " to " + pos2 + "!");

          break;

        case 6:
          // receive: createLobby	6:	username, isPublic
          // send: 6 lobbyCode
          process.stdout.write("@" + username + " #6");


          process.stdout.write(username);

          // Check all the data to make a lobby
          try {
            var isPublic = Number(data.isPublic);
            var map = data.map;
            var playerAmount = Number(data.playerAmount);
            var dimensionAmount = Number(data.dimensionAmount);

            if (isPublic == null || isPublic == undefined || map == null || map == undefined ||
              playerAmount == null || playerAmount == undefined || dimensionAmount == null || dimensionAmount == undefined) {
              throw makeError(400, 700, "", true);
            }

          } catch (e) {
            throw makeError(400, 700, "", true);
          }

          var packet = makePacket(6);

          // Make the lobby
          var newLobby = new Lobby(isPublic, map, playerAmount, dimensionAmount);
          newLobby.addUser(username, users.getData(username));

          // get the code of it
          var code = newLobby.getLobbyCode();
          lobbies.addData(code, newLobby);

          packet.lobbyCode = code;
          users.getData(username).lobbyCode = code;
          break;

        case 7:
          process.stdout.write("@" + username + " #7");
          var packet = makePacket(7);


          var message = data.message;

          var forwardMessage = username + ": " + message;
          var clients = lobbies.getData(users.getData(username).lobbyCode).getAllUsers();

          packet.message = forwardMessage;
          for (var i = 0; i < clients.length; i++) {
            var clientWs = clients[i].ws;
            if (clientWs !== ws && clientWs.readyState === WebSocket.OPEN) {
              clientWs.send(JSON.stringify(packet));
            }
          }

          packet = null;
          break;

        case 8:
          process.stdout.write("@" + username + " #8");
          var packet = makePacket(8);

          break;

        default:

          var packet = makePacket("-1");

      }
      if (packet) {
        ws.send(JSON.stringify(packet));
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

    } catch (e) {
      if (e.errorCode) {
        console.error(e.message);
      } else {
        console.error(e);
        throw e;
      }

    }
  });



  ws.on('close', function(connection) {
    users.getAllValuesAsList().forEach(function each(user) {

      if (userObject.ws === ws) {
        sendMessageExceptOne(user.lobbyCode, user.username + " has left the game!", user.ws);
      }
    });
    console.log(new Date() + ' | Colosing connection for a client.');
  });




});


server.on('upgrade', function upgrade(request, socket, head) {
  console.log(new Date() + ' | Upgrading http connection to wss: url = ' + request.url);
  // Parsing url from the request.
  var parsedUrl = url.parse(request.url, true, true);
  const pathname = parsedUrl.pathname
  console.log(new Date() + ' | Pathname = ' + pathname);
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



function makePacket(packetType) {
  return packet = {
    packetType: packetType
  };
}

function sendMessage(lobbyCode, message) {
  var packet = makePacket(7);

  var clients = lobbies.getData(lobbyCode).getAllUsers();

  packet.message = message;
  for (var i = 0; i < clients.length; i++) {
    var clientWs = clients[i].ws;
    if (clientWs.readyState === WebSocket.OPEN) {
      clientWs.send(JSON.stringify(packet));
    }
  }
}

function sendMessageExceptOne(lobbyCode, message, except) {
  var packet = makePacket(7);

  var clients = lobbies.getData(lobbyCode).getAllUsers();

  packet.message = message;
  for (var i = 0; i < clients.length; i++) {
    var clientWs = clients[i].ws;
    if (clientWs !== except && clientWs.readyState === WebSocket.OPEN) {
      clientWs.send(JSON.stringify(packet));
    }
  }
}



/*
statusCode (standard http):
400 Bad Request
401 Unauthorized
405 Method Not Allowed
410 Gone
423 Locked

errorCode:
6xx: while connecting
7xx: while connected

701: Data of packet was not as a JSON
702: packetType not supported or given
703: header wasn't given.
704: user wasn't found.
705: sessionId wasn't found or machted.


71x: problem with packet 1:  Handshake
710: Not a speciefd problem
711: No user name given
712: User name already taken


72x: problem with packet 2:  GetPublicLobbies
73x: problem with packet 3:  JoinRequest
74x: problem with packet 4:  ReadyUp
75x: problem with packet 5:  Movement
76x: problem with packet 6:
77x: problem with packet 7:
78x: problem with packet 8:



*/


function makeError(statusCode, errorCode, customMessage, makeMessage) {
  var e = {
    statusCode: statusCode,
    errorCode: errorCode,
    customMessage: customMessage,
    message: ""
  };
  if (makeMessage) {
    e.message = errorMessage(e);
  }
  return e;
}



function errorMessage(e) {

  message = "Status ";
  switch (e.statusCode) {
    case 400:
      message += "400 Bad Request.";
      break;
    case 401:
      message += "401 Unauthorized.";
      break;

    case 405:
      message += "405 Method Not Allowed.";
      break;

    case 410:
      message += "410 Gone.";
      break;
    case 423:
      message += "423 Locked.";
      break;
    default:
      message += statusCode || "none";


  }
  message += " Error Code "
  switch (e.errorCode) {
    case 701:
      message += "701 packet data was not in a JSON format.";
      break;
    case 702:
      message += "702 packet type not supported or given.";
      break;
    case 703:
      message += "703 header was not given.";
      break;
    case 704:
      message += "704 user was not found.";
      break;
    case 705:
      message += "705 sessionId was not found or machted.";
      break;


    case 710:
      message += "710 problem with the Handshake.";
      break;
    case 711:
      message += "711 no user name given.";
      break;
    case 712:
      message += "711 no user name taken.";
      break;

    default:

  }

  // If the error is not fully implemented on the server / client side
  switch (e.errorCode) {
    case 703:
    case 704:
    case 705:
      message += "Not fully implemented, possible future problem.";
    default:

  }


  return "\n" + message + "\n" + e.customMessage + "\n";
}
