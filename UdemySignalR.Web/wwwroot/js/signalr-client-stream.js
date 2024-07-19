$(document).ready(function () {

    const connection = new signalR.HubConnectionBuilder().withUrl("/exampleTypeSafeHub").configureLogging(signalR.LogLevel.Information).build();

    async function start() {

        try {
            await connection.start().then(() => {
                console.log("Hub ile baglanti kuruldu.");
                $("#connectionId").html(`Connection Id : ${connection.connectionId}`);
            });
        }
        catch (err) {
            console.error("hub ile bağlantı kurulamadı", err);
            setTimeout(() => start(), 5000)
        }

    }

    connection.onclose(async () => {
        await start();
    })

    const broadcastStreamDataToAllClient = "BroadcastStreamDataToAllClient";
    const receiveMessageAsStreamForAllClient = "ReceiveMessageAsStreamForAllClient";

    connection.on("broadcastStreamDataToAllClient", (name) => {
        $("#streamBox").append(`<p>${name}</p>`)

    })

    $("#btn_fromClient_ToHub").click(function () {

        const names= $("#txt_stream").val();

        const nameAsChunk = names.split(";");

        const subject = new signalR.Subject();

        connection.send(broadcastStreamDataToAllClient, subject).catch(err => console.error(err))

        nameAsChunk.forEach(name => {
            subject.next(name)
        });

        subject.completed();





    })










    start();



});