using System;
using System.Collections.Generic;

#nullable disable

namespace ProjectSemester3.Models
{
    public partial class Transaction
    {
        public int Id { get; set; }
        public Guid BankAccountIdFrom { get; set; }
        public Guid BankAccountIdTo { get; set; }
        public DateTime Time { get; set; }
        public decimal Amount { get; set; }
        public string Content { get; set; }
        public int ResultId { get; set; }
        public decimal BalanceFrom { get; set; }
        public decimal BalanceTo { get; set; }

        public virtual BankAccount BankAccountIdFromNavigation { get; set; }
        public virtual BankAccount BankAccountIdToNavigation { get; set; }
        public virtual Result Result { get; set; }
    }
}
