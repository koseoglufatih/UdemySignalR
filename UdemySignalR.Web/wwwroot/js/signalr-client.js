$(document).ready(function () {


    const connection = new signalR.HubConnectionBuilder().withUrl("/exampleTypeSafeHub").configureLogging(signalR.LogLevel.Information).build();

    const broadcastMessageToAllClientHubMethodCall = "BroadcastMessageToAllClient";
    const receiveMessageForAllClientMethodCall = "ReceiveMessageForAllClient";

    const broadcastMessageToCallerClient = "BroadcastMessageToCallerClient"
    const receiveMessageForCallerClient = "ReceiveMessageForCallerClient";

    const broadcastMessageToOtherClient = "BroadcastMessageToOtherClient";
    const receiveMessageForOthersClient = "ReceiveMessageForOthersClient"

    const broadcastMessageToIndividualClient = "BroadcastMessageToIndividualClient";
    const receiveMessageForIndividualClient = "ReceiveMessageForIndividualClient";


    const receiveConnectedClientCountAllClient = "ReceiveConnectedClientCountAllClient";

    const groupA = "GroupA";
    const groupB = "GroupB";
    let currentGroupList = [];                          //rastgele basarsa bu grouba dahil olacak

    function refreshGroupList() {
        $("#groupList").empty();
        currentGroupList.forEach(x => {
            $("#groupList").append(`<p>${x}</p>`)
        })
    }

    $("#btn-groupA-add").click(function () {

        if (currentGroupList.includes(groupA)) return;

        connection.invoke("AddGroup", groupA).then(() => {

            currentGroupList.push(groupA);
            refreshGroupList();
        })
    })

    $("#btn-groupB-add").click(function () {

        if (currentGroupList.includes(groupB)) return;

        connection.invoke("AddGroup", groupB).then(() => {


            currentGroupList.push(groupB);
            refreshGroupList();
        })
    })

    $("#btn-groupA-remove").click(function () {


        $("#btn-groupA-remove").click(function () {
            if (!currentGroupList.includes(groupA)) return;

            connection.invoke("RemoveGroup", groupA).then(() => {
                currentGroupList = currentGroupList.filter(x => x !== groupA); // currentGroupList'i güncelle
                refreshGroupList();
            })
        })

        $("#btn-groupB-remove").click(function () {
            if (!currentGroupList.includes(groupB)) return;

            connection.invoke("RemoveGroup", groupB).then(() => {
                currentGroupList = currentGroupList.filter(x => x !== groupB); // currentGroupList'i güncelle
                refreshGroupList();
            })
        })

        $("#btn-groupA-send-message").click(function () {
            const message = "Group A Mesaj";
            connection.invoke("BroadcastMessageToGroupClients", groupA, message).catch(err => console.error("hata", err))
            console.log("Mesaj gönderildi");

        })

        $("#btn-groupB-send-message").click(function () {
            const message = "Group B Mesaj";
            connection.invoke("BroadcastMessageToGroupClients", groupB, message).catch(err => console.error("hata", err))
            console.log("Mesaj gönderildi");

        })

        connection.on("ReceiveMessageForGroupClients", (message) => {

            console.log("Gelen Mesaj", message);
        })











        function start() {
            connection.start().then(() => {
                console.log("Hub ile baglanti kuruldu.");
                $("#connectionId").html(`Connection Id : ${connection.connectionId}`);
            });
        }

        try {
            start();
        }
        catch
        {
            setTimeout(() => start(), 5000)
        }


        //subscribe
        connection.on(receiveMessageForAllClientMethodCall, (message) => {

            console.log("Gelen Mesaj", message);
        })

        connection.on(receiveMessageForCallerClient, (message) => {

            console.log("(Caller) Gelen Mesaj", message);
        })

        connection.on(receiveMessageForOthersClient, (message) => {

            console.log("(Others) Gelen Mesaj", message);
        })

        connection.on(receiveMessageForIndividualClient, (message) => {

            console.log("(Individual) Gelen Mesaj", message);
        })


        const span_client_count = $("#span - connected - client - count");
        connection.on(receiveConnectedClientCountAllClient, (count) => {
            span_client_count.text(count);
            console.log("connected client count", count);
        })

















        $("#btn-send-message-all-client").click(function () {

            const message = "hello world";
            connection.invoke(broadcastMessageToAllClientHubMethodCall, message).catch(err => console.error("hata", err))
            console.log("Mesaj gönderildi");

        })

        $("#btn-send-message-caller-client").click(function () {

            const message = "hello world";
            connection.invoke(broadcastMessageToCallerClient, message).catch(err => console.error("hata", err))
            console.log("Mesaj gönderildi");

        })

        $("#btn-send-message-others-client").click(function () {

            const message = "hello world";
            connection.invoke(broadcastMessageToOtherClient, message).catch(err => console.error("hata", err))
            console.log("Mesaj gönderildi");

        })

        $("#btn-send-message-individual-client").click(function () {

            const message = "hello world";
            const connectionId = $("#text-connectionId").val();
            connection.invoke(broadcastMessageToIndividualClient, connectionId, message).catch(err => console.error("hata", err))
            console.log("Mesaj gönderildi");



        })



    })

})

