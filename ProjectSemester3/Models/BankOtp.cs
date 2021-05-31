using System;
using System.Collections.Generic;

#nullable disable

namespace ProjectSemester3.Models
{
    public partial class BankOtp
    {
        public int Id { get; set; }
        public string Otp { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }
        public Guid AccountObjectId { get; set; }

        public virtual AccountObject AccountObject { get; set; }
    }
}
