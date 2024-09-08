using Bookify.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure;


internal sealed class BookingConfiguration: IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("bookings");
        builder.HasKey(booking => booking.Id);

        builder.OwnsOne(booking => booking.PriceForPeriod, pricebuilder =>{
            pricebuilder.Property(money=> money.currency)
                .HasConversion(currency => currency.Code, code => Currency.FromCode(code));
        });

        builder.OwnsOne(booking => booking.CleaningFee, pricebuilder=>{
            pricebuilder.Property(money => money.currency)
                .HasConversion(currency => currency.Code, code => Currency.FromCode(code));
        });

        builder.OwnsOne(booking => booking.AmenitiesUpCharge, priceBuilder=>{
            priceBuilder.Property(money => money.currency)
                .HasConversion(currency => currency.Code, code => Currency.FromCode(code));
        });

        builder.OwnsOne(booking => booking.TotalPrice, priceBuilder=>{
            priceBuilder.Property(money => money.currency)
                .HasConversion(currency => currency.Code, code => Currency.FromCode(code));
        });

        builder.OwnsOne(booking=> booking.Duration);

        builder.HasOne<Apartment>()
            .WithMany()
            .HasForeignKey(booking=> booking.ApartmentId);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(booking=> booking.UserId);
    }
}