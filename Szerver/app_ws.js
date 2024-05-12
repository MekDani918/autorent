const ws = require("ws");

const wsServer = new ws.Server({ noServer: true });
wsServer.on('connection', (socket)=>{
    console.log('New connection!');
    socket.send('Hi!');
    wsEcho(socket);
});

function wsEcho(socket){
    socket.on('message', (message)=>{
        console.log('ws: %s', message);
        socket.send('' + message);
    });
}

module.exports = wsServer;