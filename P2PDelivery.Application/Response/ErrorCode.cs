namespace P2PDelivery.Application.Response
{
    public enum ErrorCode
    {
        None = 0,
        ServerError=1,
        UnexpectedError=2,

        // Auth
        EmailExist = 101,
        EmailNotExist = 102,
        IncorrectPassword = 103,

        // User
        UserNotExist = 200,
        IdentityError = 201,
        ValidationError = 202,
        Userexist = 203,


        // DeliveryRequest Errors


        // Application Errors


        // Item Errors


        // Payment Errors

    }
}
