

exports.meakUser = function maekUser(username, color, lobbyCode, sessionId, ws){

  return user={
      username:username,
      color:color,
      lobbyCode:lobbyCode,
      sessionId:sessionId,
      readyState:false,
      ws:ws

  };



};
