using System;
using System.Collections.Generic;

#nullable disable

namespace ProjectSemester3.Models
{
    public partial class Problem
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime? AnswerDate { get; set; }
        public Guid? AnswererId { get; set; }

        public virtual AccountObject Answerer { get; set; }
    }
}
