namespace Bookify.Domain;

public record PricingDetails(
    Money PriceForPeriod,
    Money CleaningFee,
    Money AmenitiesUpCharge,
    Money TotalPrice
);