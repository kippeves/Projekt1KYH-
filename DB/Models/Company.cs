using System;
using System.Collections.Generic;

namespace DB.Models
{
    public partial class Company
    {
        public Company()
        {
            Projects = new HashSet<Project>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Project> Projects { get; set; }
    }
}
