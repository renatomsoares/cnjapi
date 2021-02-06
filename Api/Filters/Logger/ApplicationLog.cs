using System;

namespace Application.Filters.Logger
{
    public class ApplicationLog
    {
        public int Id { get; set; }
        public string Application { get; set; }
        public string LogLevel { get; set; }
        public string Action { get; set; }
        public string Message { get; set; }
        public DateTime? EventDate { get; set; }
    }
}
