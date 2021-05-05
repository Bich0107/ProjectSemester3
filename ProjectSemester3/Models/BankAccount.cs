using System;
using System.Collections.Generic;

#nullable disable

namespace ProjectSemester3.Models
{
    public partial class BankAccount
    {
        public BankAccount()
        {
            Checks = new HashSet<Check>();
            TransactionBankAccountIdFromNavigations = new HashSet<Transaction>();
            TransactionBankAccountIdToNavigations = new HashSet<Transaction>();
        }

        public Guid Id { get; set; }
        public Guid UserAccountId { get; set; }
        public decimal Balance { get; set; }
        public string BankCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public int BankId { get; set; }
        public bool Locked { get; set; }
        public int CurrencyId { get; set; }

        public virtual Bank Bank { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual AccountObject UserAccount { get; set; }
        public virtual ICollection<Check> Checks { get; set; }
        public virtual ICollection<Transaction> TransactionBankAccountIdFromNavigations { get; set; }
        public virtual ICollection<Transaction> TransactionBankAccountIdToNavigations { get; set; }
    }
}
