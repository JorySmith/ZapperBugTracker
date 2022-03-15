using System.ComponentModel;

namespace ZapperBugTracker.Models
{
    public class ProjectPriority
    {
        // Id primary key
        public int Id { get; set; }

        [DisplayName("Priority Name")]
        public string Name { get; set; }
    }
}
