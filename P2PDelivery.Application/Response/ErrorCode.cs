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
        CanNotRecover = 213,
        Userexist = 214,
        InvalidRefreshToken = 215,
        // DeliveryRequest Errors
        DeliveryRequestNotExist = 300,
        DeliveryRequestAlreadyExist = 301,
        DeliveryRequestCreated = 302,
        DeliveryRequestFaliedDeleted = 303,
        DeliveryRequestDeleted = 304,
        DeliveryRequestUpdatedSuccessfully = 305,
        DeliveryRequestUpdateFailed = 306,

        
        // Chat Errors
        ChatNotFound = 350,



        // Application Errors
        ApplicationNotExist = 400,
       


        // Item Errors


        // Payment Errors
        
        
        
        // Notifications Errors
        NotificationNotFound = 500,
        NotificationCreationError = 501,
        matchnotexist = 502
    }
}
