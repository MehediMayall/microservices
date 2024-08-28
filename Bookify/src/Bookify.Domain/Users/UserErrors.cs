namespace Bookify.Domain;


public static class UserErrors
{
    public static Error NotFound() =>
        new Error("UserErrors.NotFound","User not found");
}