namespace Wox.Plugin.StackOverlow.Infrascructure.Model
{
    public class ErrorResponse : Response
    {
        public ResponseErrorType ErrotType { get; }

        public string Message { get; }

        public ErrorResponse(ResponseErrorType errotType, string message = null)
        {
            ErrotType = errotType;
            Message = message;
        }
    }
}