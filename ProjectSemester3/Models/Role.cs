using System;
using System.Collections.Generic;

#nullable disable

namespace ProjectSemester3.Models
{
    public partial class Role
    {
        public Role()
        {
            AccountObjects = new HashSet<AccountObject>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Grade { get; set; }

        public virtual ICollection<AccountObject> AccountObjects { get; set; }
    }
}
