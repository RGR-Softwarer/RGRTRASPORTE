using System.Net;

namespace Infra.CrossCutting.Handlers.Notifications
{
    public class Notification
    {
        public string Message { get; set; }
        public HttpStatusCode? StatusCode { get; set; }

        public Notification(string message)
        {
            Message = message;
        }

        public Notification(string message, HttpStatusCode statusCode)
        {
            Message = message;
            StatusCode = statusCode;
        }
    }
}
