using System.ComponentModel;

namespace ZapperBugTracker.Models
{
    public class TicketType
    {
        // Primary key Id (DisplayName() is for the View)
        public int Id { get; set; }

        [DisplayName("Type Name")]
        public string Name { get; set; }
    }
}
