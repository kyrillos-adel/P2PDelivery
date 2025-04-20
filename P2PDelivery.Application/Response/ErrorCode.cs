namespace P2PDelivery.Application.Response
{
    public enum ErrorCode
    {
        None = 0,
        ServerError=1,
        UnexpectedError=2,
        UnAuthorize=3,

        // Auth
        EmailExist = 101,
        EmailNotExist = 102,
        IncorrectPassword = 103,

        // User
        UserNotExist = 200,
        IdentityError = 201,
        ValidationError = 202,
        Userexist = 212,
        UserNotFound = 203,
        InvalidToken = 204,
        UserAlreadyDeleted = 205,
        Unauthorized = 206,
        UnknownError = 207,
        DeleteFailed = 208,
        UserDeleted = 209,
        LoginFailed = 210,
        InvalidPassword = 211,
        UpdateFailed = 212,
        // DeliveryRequest Errors
        DeliveryRequestNotExist = 300,
        



        // Application Errors


        // Item Errors


        // Payment Errors

    }
}
