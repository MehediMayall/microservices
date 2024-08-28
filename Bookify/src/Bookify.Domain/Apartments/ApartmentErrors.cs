namespace Bookify.Domain;

public static class ApartmentErrors
{
    public static Error NotFound()=>
        new Error("ApartmentErrors.NotFound","Apartment not found");
}