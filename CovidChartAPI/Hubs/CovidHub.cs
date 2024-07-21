using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace CovidChartAPI.Hubs
{
    public class CovidHub : Hub
    {
        public async Task GetCovidList()
        {
            await Clients.All.SendAsync("ReceiveCovidList", "serviceden covid19 verilerini al");
        }
    }
}

