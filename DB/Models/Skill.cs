using System;
using System.Collections.Generic;

namespace DB.Models
{
    public partial class Skill
    {
        public Skill()
        {
            Consultants = new HashSet<Consultant>();
            Projects = new HashSet<Project>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Consultant> Consultants { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}
