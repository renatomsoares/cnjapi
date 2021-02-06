using System;

namespace Domain.Views
{
    public class LawsuitView
    {
        public int IdLawsuit { get; set; }
        public string CaseNumber { get; set; }
        public string CourtName { get; set; }
        public string ResponsibleName { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
