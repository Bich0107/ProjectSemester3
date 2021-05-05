using System;
using System.Collections.Generic;

#nullable disable

namespace ProjectSemester3.Models
{
    public partial class Result
    {
        public Result()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public bool Success { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
