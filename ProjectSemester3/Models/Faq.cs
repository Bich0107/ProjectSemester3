using System;
using System.Collections.Generic;

#nullable disable

namespace ProjectSemester3.Models
{
    public partial class Faq
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
    }
}
