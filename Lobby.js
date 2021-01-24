
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

  getAllUsers(){
    return this.users.getAllData();
  }

  addUser(username, user){
    this.users.addData(username, user);

    console.log(username+" is now in lobby "+this.lobbyCode );
  }

  setReady(username, ready){
    //this.user.readyState = true;
    this.users.getData(username).readyState = true;
  }

  checkAllReady(){

    var usersAsArray = this.users.getAllData();

    var i;
    for (i = 0; i < usersAsArray.length; i++){
        if(!usersAsArray[i][0][1].readyState){
          return false;
        }
    }

    return  true;


  }

}


module.exports.LobbyObj = LobbyObj;
