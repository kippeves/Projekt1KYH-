using System;
using System.Collections.Generic;

namespace DB.Models
{
    public partial class Consultant
    {
        public Consultant()
        {
            Projects = new HashSet<Project>();
            Skills = new HashSet<Skill>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string FullName => FirstName + " " + LastName;
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Skill> Skills { get; set; }
    }
}
