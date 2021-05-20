using System;
using System.Collections.Generic;

#nullable disable

namespace ProjectSemester3.Models
{
    public partial class Setting
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int DefaultNumOfShowedTransaction { get; set; }
        public int DefaultCurrencyId { get; set; }
        public string WarningMsg1 { get; set; }
        public string WarningMsg2 { get; set; }
        public string WarningMsg3 { get; set; }
        public string SuccessMsg { get; set; }
    }
}
