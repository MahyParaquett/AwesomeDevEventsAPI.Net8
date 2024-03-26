using AwesomeDevEventsAPI.Etities;

namespace AwesomeDevEventsAPI.Persistence
{
    public class DevEventsDbContext
    {
        public List<DevEvent> DevEvent {  get; set; }
        public DevEventsDbContext()
        {
            DevEvent = new List<DevEvent>();
        }
    }
}
