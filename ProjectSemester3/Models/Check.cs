using System;
using System.Collections.Generic;

#nullable disable

namespace ProjectSemester3.Models
{
    public partial class Check
    {
        public int Id { get; set; }
        public Guid BankAccountId { get; set; }
        public string Bearer { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Address { get; set; }
        public bool Valid { get; set; }

        public virtual BankAccount BankAccount { get; set; }
    }
}
