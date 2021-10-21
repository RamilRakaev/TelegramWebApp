using GoogleCalendarBusiness;
using GoogleCalendarService;
using Domain.Model;
using Xunit;
using System.Threading.Tasks;

namespace UniteTests
{
    public class GoogleCalendarTest
    {
        private static readonly IGoogleCalendar google;

        static GoogleCalendarTest()
        {
            var options = new Option[] {
                new Option("ApiKey", "AIzaSyDdHs_CxKsuLN_hVpXKGM6Ly20iKuBD_go"),
                new Option("CalendarId", "fdhqb13f02sdoq6pp2o2fu39pc@group.calendar.google.com")
            };
            google = new GoogleCalendar(options);
        }

        [Fact]
        public async Task Test1()
        {
            var events = await google.ShowFreeDays(1);
        }
    }
}
