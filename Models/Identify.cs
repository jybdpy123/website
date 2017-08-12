using System;

namespace Models
{
    public class Identify
    {
        public int? No { get; set; }
        public DateTime Date { get; set; }
        public string IdentityCode { get; set; }
    }

    public class IdentityQuestion
    {
        public string No { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }

    public class IdentityLog
    {
        public string No { get; set; }
        public string Qq { get; set; }
        public DateTime Date { get; set; }
        public bool? Result { get; set; }
    }
}
