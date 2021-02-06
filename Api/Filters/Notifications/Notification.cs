using System.Collections.Generic;

namespace Application.Filters.Notifications
{
    public class Notification
    {
        public IList<string> Erros { get; } = new List<string>();
        public void Add(string description)
        {
            Erros.Add(description);
        }
    }
}
