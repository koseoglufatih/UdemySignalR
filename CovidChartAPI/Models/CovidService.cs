using CovidChartAPI.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public List<CovidChart> GetCovidChartList()
        {
            List<CovidChart> covidCharts = new List<CovidChart>();
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "select tarih,[1],[2],[3],[4],[5] FROM (select[City],[Count],Cast([CovidDate] as date) as tarih FROM Covids) as covidT PIVOT (SUM(Count) For City IN ([1],[2],[3],[4],[5])) as ptable ORDER BY tarih asc";

                command.CommandType = System.Data.CommandType.Text;

                _context.Database.OpenConnection();

                using (var reader=command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CovidChart cc = new CovidChart();

                        cc.CovidDate = reader.GetDateTime(0).ToShortDateString();
                        Enumerable.Range(1, 5).ToList().ForEach(x =>
                        {
                            if (System.DBNull.Value.Equals(reader[x]))
                                {
                                cc.Counts.Add(0);
                            }
                            else {
                                cc.Counts.Add(reader.GetInt32(x));
                            }

                        });


                        covidCharts.Add(cc);

                    }

                }


                _context.Database.CloseConnection();

                return covidCharts;



            }

        }

    }
}
