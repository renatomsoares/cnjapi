using System;

namespace Domain.Entities
{
    public class Lawsuit
    {
        public int IdLawsuit { get; set; }
        public string CaseNumber { get; set; }
        public string CourtName { get; set; }
        public string ResponsibleName { get; set; }
        public DateTime RegistrationDate { get; set; }

        public Lawsuit()
        {
            RegistrationDate = DateTime.Now;
        }
    }
}
