

exports.makeUser = function makeUser(username, color, lobbyCode, sessionId, ws){

  return user={
      username:username,
      color:color,
      lobbyCode:lobbyCode,
      sessionId:sessionId,
      readyState:false,
      ws:ws

  };



};
