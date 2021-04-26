
const { v4: uuidv4 } = require('uuid');
const DataHasMap = require("./DataSaveBank.js").DataObj;
var customId = require("custom-id");


var LobbyObj = class Lobby{

  constructor(isPublic,map,playerAmount,dimensionAmount, lobbyCode){


    this.isPublic = isPublic;
    this.map = map;
    this.playerAmount = playerAmount;
    this.dimensionAmount = dimensionAmount;


    this.activeGame = false;

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

  isActive(){
    return  this.activeGame;
  }
  getdimensionAmount(){
    return  this.dimensionAmount;
  }

  getAllUsers(){
    return this.users.getAllValuesAsList();
  }

  addUser(username, user){

    //if(usersAsArray.length > this.playerAmount) return;
    try {
      if(!this.users.addData(username, user, true)){
        throw "";
      }
    } catch (e) {

    }


    console.log(username+" is now in lobby "+this.lobbyCode );
  }

  setReady(username, ready){
    //this.user.readyState = true;
    this.users.getData(username).readyState = true;
  }

  checkAllReady(){

    var usersAsArray = this.users.getAllData();

    var i;
    if(usersAsArray.length == 0 || usersAsArray.length == 1){
      return false;
    }
    for (i = 0; i < usersAsArray.length; i++){
        if(!usersAsArray[i][0][1].readyState){
          return false;
        }
    }
    this.activeGame = true;
    return  true;


  }

}


module.exports.LobbyObj = LobbyObj;
