namespace TaskManagement.Application.Common.Exceptions
{
    public class DuplicateTaskException:Exception
    {
        public DuplicateTaskException(string message):base(message)
        {
        }
    }
}
