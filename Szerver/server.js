const http = require('http');
const { app, wsServer } = require('./app');

const port = process.env.PORT || 3000;

const server = http.createServer(app);

server.listen(port);
console.log(`Listening on ${process.env.PORT || 3000}`);

server.on('upgrade', (request, socket, head) => {
    wsServer.handleUpgrade(request, socket, head, socket => {
        wsServer.emit('connection', socket, request);
    });
});
