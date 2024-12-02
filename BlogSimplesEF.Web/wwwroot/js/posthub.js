const connection = new signalR.HubConnectionBuilder()
    .withUrl("/PostsHub")
    .configureLogging(signalR.LogLevel.Information)
    //.withAutomaticReconnect([0, 2000, 10000, 30000]) //Reconectar automaticamente
    //.withAutomaticReconnect([0, 0, 10000])
    .withAutomaticReconnect({
        nextRetryDelayInMilliseconds: retryContext => {
            if (retryContext.elapsedMilliseconds < 60000)
                return Math.random() * 10000;
            else
                return null;
        }
    })
    .build();

async function start()
{

    try {
        await connection.start();
        console.assert(connection.state === signalR.HubConnectionState.Connected);
        console.log("SignalR Connected.");
    } catch (err) {
        console.assert(connection.state === signalR.HubConnectionState.Disconnected);
        console.log(err);
        setTimeout(() => start(), 5000);
    }

};

connection.onclose(async () =>
{
    console.assert(connection.state === signalR.HubConnectionState.Disconnected);

    var msgCLient = `Connection closed due to error "${error}". Try refreshing this page to restart the connection.`;
    console.warn(msgCLient);
});

connection.on("ReceiveMessage", (user, message) =>
{
    var msgFromServer = `WebSocket[Novo post]:${user}: ${message}`;
    console.log(msgFromServer);
    $.notify(msgFromServer);
});

connection.onreconnecting(error =>
{
    console.assert(connection.state === signalR.HubConnectionState.Reconnecting);
    //Aqui se usar elemento de interface podemos trabalhar em cima disso

    var msgCLient = `Connection lost due to error "${error}". Reconnecting.`;
    console.warn(msgCLient);
    //$.notify(msgCLient);

});

connection.onreconnected(connectionId => {
    console.assert(connection.state === signalR.HubConnectionState.Connected);

    //Aqui se usar elemento de interface podemos trabalhar em cima disso

    var msgCLient = `Connection reconnected with id ${connectionId}`;
    Consol.warn(msgCLient);
    //$.notify(msgCLient);
});


// Start the connection.
start();

