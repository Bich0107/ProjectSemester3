using System;
using System.Collections.Generic;

#nullable disable

namespace ProjectSemester3.Models
{
    public partial class Currency
    {
        public Currency()
        {
            BankAccounts = new HashSet<BankAccount>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public double ExchangeRate { get; set; }
        public bool Default { get; set; }

        public virtual ICollection<BankAccount> BankAccounts { get; set; }
    }
}
