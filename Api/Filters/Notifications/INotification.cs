using System.Collections.Generic;

namespace Application.Filters.Notifications
{
    public interface INotification
    {
        IList<string> Erros { get; }
        void Add(string error);
    }
}
