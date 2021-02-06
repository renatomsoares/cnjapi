namespace Application.Filters.Notifications
{
    public class ErrorDescription : Description
    {
        public ErrorDescription(string message,  params string[] args)
            : base(message, args)
        {
        }
    }
}
