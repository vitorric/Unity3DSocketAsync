const net = require('net'),
    server = net.createServer();

server.listen(3009, function() {
    console.log("Server Running!");
});

//new User
server.on('connection', function(socket){
    
    console.log("New Connection");

    let totalLength = 0;

    //receiving data
    socket.on('data', chunk => {
       totalLength += chunk.length;

       //callback to Unity3D
       socket.write(`${totalLength}|`);  
    });

    // Evento disconnect ocorre quando sai um usuário.
    socket.on('disconnect', function() {
        console.log('disconnect');
    });

    //Receveid All data
    socket.on('end', function() {
        console.log('done - Total Bytes: ' + totalLength);
    });
});