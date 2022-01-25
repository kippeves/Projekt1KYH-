using System;
using System.Collections.Generic;

namespace DB.Models
{
    public partial class Project
    {
        public Project()
        {
            Consultants = new HashSet<Consultant>();
            Skills = new HashSet<Skill>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int StatusId { get; set; }
        public int CompanyId { get; set; }

        public virtual Company? Company { get; set; }
        public virtual Status? Status { get; set; }

        public virtual ICollection<Consultant> Consultants { get; set; }
        public virtual ICollection<Skill> Skills { get; set; }
    }
}
