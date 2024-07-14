$(document).ready(function()
{

    const broadcastMessageToAllClientHubMethodCall = "BroadcastMessageToAllClient";
    const ReceiveMessageForAllClientMethodCall = "ReceiveMessageForAllClient";
    const receiveConnectedClientCountAllClient = "ReceiveConnectedClientCountAllClient";


    ; const connection = new signalR.HubConnectionBuilder().withUrl("/exampleTypeSafeHub").configureLogging(signalR.LogLevel.Information).build();

    function start() {
        connection.start().then(() => console.log("Hub ile baglanti kuruldu."));
    }

    try {
        start();
    }
    catch
    {
        setTimeout(()=> start(),5000)
    }


    //subscribe
    connection.on(ReceiveMessageForAllClientMethodCall, (message) => {

        console.log("Gelen Mesaj", message);
    })

    var span_client_count = $("#span - connected - client - count");

    connection.on(receiveConnectedClientCountAllClient, (count) => {
        span_client_count.text(count);
        console.log("connected client count", count);
    })


    $("#btn-send-message-all-client").click(function () {

        const message = "hello world";
        connection.invoke(broadcastMessageToAllClientHubMethodCall,message).catch(err=> console.error("hata",err))

    })



})