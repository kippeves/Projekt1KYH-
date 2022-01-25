using System;
using System.Collections.Generic;

namespace DB.Models
{
    public partial class Status
    {
        public Status()
        {
            Projects = new HashSet<Project>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Project> Projects { get; set; }
    }
}
