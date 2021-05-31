using System;
using System.Collections.Generic;

#nullable disable

namespace ProjectSemester3.Models
{
    public partial class AccountObject
    {
        public AccountObject()
        {
            BankAccounts = new HashSet<BankAccount>();
            Problems = new HashSet<Problem>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Job { get; set; }
        public string Gender { get; set; }
        public string IdNum { get; set; }
        public bool Staff { get; set; }
        public int PositionId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Locked { get; set; }
        public bool IsAuthentication { get; set; }
        public int WrongPassword { get; set; }

        public virtual Role Position { get; set; }
        public virtual ICollection<BankAccount> BankAccounts { get; set; }
        public virtual ICollection<Problem> Problems { get; set; }
    }
}
