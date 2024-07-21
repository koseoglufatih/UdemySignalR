using CovidChartAPI.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;

namespace CovidChartAPI.Models
{
    public class CovidService
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<CovidHub> _hubContext;

        public CovidService(AppDbContext context, IHubContext<CovidHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public IQueryable<Covid> GetList()
        {
            return _context.Covids.AsQueryable();            //asqueryable içerisinde ekstra sorgulamalar yapıp en son çağırıldığında veritabanına gider aradaki sorgulamalar veritabanına gönderilmez.
              //Ienumarable sorgulama yapıldığında gider veritabanındaki tabloları getirip sorgulama yapar queryable where //////sorgulamasıyla beraber getirir. yani performans farkı var

        }

        public async Task SaveCovid(Covid covid)
        {
            await _context.Covids.AddAsync(covid);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("ReceiveCovidList", "data");
        }
    }
}
