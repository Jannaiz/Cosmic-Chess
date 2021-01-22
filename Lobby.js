
const { v4: uuidv4 } = require('uuid');
const DataHasMap = require("./DataSaveBank.js").DataObj;
var customId = require("custom-id");


var LobbyObj = class Lobby{

  constructor(isPublic, lobbyCode){

    this.isPublic = isPublic;

    this.users = new DataHasMap();


    if(lobbyCode){
        this.lobbyCode = lobbyCode;
    }else{

        this.lobbyCode = customId({});

    }



  }

  isPublic(){
    if(isPublic == 1){
      return true;
    }
    return false;
  }

  getLobbyCode(){
      return  this.lobbyCode;
  }

  addUser(username, user){
    this.users.addData(username, user);

    console.log(username+" is now in lobby "+this.lobbyCode );
  }

  setReady(username, ready){
    this.user.readyState = true;
  }

  checkAllReady(){

    var usersAsArray = this.users.getData();

    var i;
    for (i = 0; i < usersAsArray.lenght; i++){
        if(!usersAsArray[i].readyState){
          return false;
        }
    }

    return  true;


  }

}


module.exports.LobbyObj = LobbyObj;
