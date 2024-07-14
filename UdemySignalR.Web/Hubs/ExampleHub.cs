using Microsoft.AspNetCore.SignalR;

namespace UdemySignalR.Web.Hubs
{
    public class ExampleHub:Hub
    {
        public async Task BroadcastMessageToAllClient(string message) 
            // bu method client tarafından tetiklenecek. index javascprit tarafından method tetiklenince başka bir methodu tetikleyecek.
            //client methodları çağıranlar.
        {
            await Clients.

        }

    }
}
