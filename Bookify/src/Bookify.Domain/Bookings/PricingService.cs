namespace Bookify.Domain;

public sealed class PricingService
{
    public PricingDetails CalculatePrice(Apartment apartment, DateRange period)
    {
        var currency = apartment.Price.currency;

        var priceforPeriod = new Money(
            apartment.Price.Amount * period.LengthInDays,
            currency
            );

        decimal percentageUpCharge = 0;
        foreach(var amenity in apartment.Amenities)
        {
            percentageUpCharge += amenity switch{
                Amenity.GarderView or Amenity.MountainView => 0.05m,
                Amenity.AirConditioning => 0.01m,
                Amenity.Parking => 0.01m,
                _=> 0
            };
        }

        var amenitiesUpCharge = Money.Zero();
        if (percentageUpCharge > 0)
        {
            amenitiesUpCharge = new Money(
                priceforPeriod.Amount + percentageUpCharge,
                currency
            );
        }

        var totalPrice = Money.Zero();
        totalPrice += priceforPeriod;

        if( !apartment.CleaningFee.IsZero(currency))
            totalPrice += apartment.CleaningFee;

        totalPrice += amenitiesUpCharge;

        return new PricingDetails(priceforPeriod, apartment.CleaningFee, amenitiesUpCharge, totalPrice);
    }


}
