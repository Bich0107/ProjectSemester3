using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        [Required]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Job { get; set; }
        public string Gender { get; set; }
        [Required]
        [Display(Name = "Identity number")]
        public string IdNum { get; set; }
        [Required]
        public bool Staff { get; set; }
        [Required]
        public int PositionId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public bool Locked { get; set; }
        public bool IsAuthentication { get; set; }
        public int WrongPassword { get; set; }

        public virtual Role Position { get; set; }
        public virtual ICollection<BankAccount> BankAccounts { get; set; }
        public virtual ICollection<Problem> Problems { get; set; }
    }
}
