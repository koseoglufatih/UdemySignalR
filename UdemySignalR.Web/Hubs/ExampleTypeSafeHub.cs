using Microsoft.AspNetCore.SignalR;
using UdemySignalR.Web.Models;

namespace UdemySignalR.Web.Hubs
{
    public class ExampleTypeSafeHub:Hub<IExampleTypeSafeHub>
    {

        private static int ConnectedClientCount = 0;

        public async Task BroadcastMessageToAllClient(string message)      
        {
            await Clients.All.ReceiveMessageForAllClient(message);
        }   //hepsine yayın yapıyor

        public async Task BroadcastTypedMessageToAllClient(Product product)
        {
            await Clients.All.ReceiveTypedMessageForAllClient(product);
        }


        public async Task BroadcastMessageToCallerClient(string message)
        {
            await Clients.Caller.ReceiveMessageForCallerClient(message);
        }        //kendisi dahil herkese yayın yapıyor
        public async Task BroadcastMessageToOtherClient(string message)
        {
            await Clients.Others.ReceiveMessageForOthersClient(message);
        }          //kendisi hariç herkese yayın yapıyor
        public async Task BroadcastMessageToIndividualClient(string connectionId,string message)
        {
            await Clients.Client(connectionId).ReceiveMessageForIndividualClient(message);
        } // id si belirlenen client e yayın yapacak spesifik

        public async Task BroadcastStreamDataToAllClient(IAsyncEnumerable<string> nameAsChunks)
        {
           
            await foreach (var names in nameAsChunks)
            {
                await Task.Delay(1000);
                await Clients.All.ReceiveMessageAsStreamForAllClient(names);
            }
                

        }







        public async Task BroadcastMessageToGroupClients(string groupName, string message)
        {
            await Clients.Group(groupName).ReceiveMessageForGroupClients(message);
        }

        public async Task AddGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId,groupName);        
            await Clients.Caller.ReceiveMessageForCallerClient($"{groupName}grubuna dahil oldunuz.");         
            await Clients.Group(groupName).ReceiveMessageForGroupClients($"Kullanıcı({Context.ConnectionId}) {groupName} dahil oldu."); // sadece bu gruptakilere message düşmesi için yazdık.

            //await Clients.Others.ReceiveMessageForOthersClient($"Kullanıcı({Context.ConnectionId}) {groupName} dahil oldu.");   //diğerlerine bilgi geçmek için yazılabilir.
        }

        public async Task RemoveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId,groupName);
            await Clients.Caller.ReceiveMessageForCallerClient($"{groupName}grubundan çıktınız.");           
            await Clients.Group(groupName).ReceiveMessageForGroupClients($"Kullanıcı({Context.ConnectionId}) {groupName} gruğtan çıktı");

            //await Clients.Others.ReceiveMessageForOthersClient($"Kullanıcı({Context.ConnectionId}) {groupName} gruğtan çıktı");
        }



        public override async Task OnConnectedAsync()
        {
            ConnectedClientCount++;

            await Clients.All.ReceiveConnectedClientCountAllClient(ConnectedClientCount);
            await base.OnConnectedAsync();
           
        }
        public override async  Task OnDisconnectedAsync(Exception? exception)
        {
            ConnectedClientCount--;

            await Clients.All.ReceiveConnectedClientCountAllClient(ConnectedClientCount);
            await base.OnDisconnectedAsync(exception);
        }

        
    }
}
