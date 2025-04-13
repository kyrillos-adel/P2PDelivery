using Azure.Core;
using P2PDelivery.Application.Response;

namespace P2PDelivery.API.Middelwares
{
    public class GlobalErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlerMiddleware> _logger;

        public GlobalErrorHandlerMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                var requestId = Guid.NewGuid();
                _logger.LogError(ex.Message, $"RequestId: {requestId}", $"error message:{ex.Message}", $"stack trace:{ex.StackTrace}" );


                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                var message = ex.Message is null ? "An unexpected error occurred." : ex.Message;

                var responseMsg = $"{requestId}: {message}";
                var errorResponse = RequestResponse<string>.Failure(ErrorCode.UnexpectedError, responseMsg);
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    
    }
}
