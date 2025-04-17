namespace P2PDelivery.Application.Response
{
    public record RequestResponse<T>(T Data, bool IsSuccess, string Message, ErrorCode ErrorCode)
    {
        public static RequestResponse<T> Success(T data, string message = "")
        {
            return new RequestResponse<T>(data, true, message, ErrorCode.None);
        }
        public static RequestResponse<T> Failure(ErrorCode errorCode, string message)
        {
            return new RequestResponse<T>(default, false, message, errorCode);
        }
    }
}
