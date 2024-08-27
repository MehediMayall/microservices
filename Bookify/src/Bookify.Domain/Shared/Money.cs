namespace Bookify.Domain;

public record Money(decimal Amount, Currency currency)
{
    public static Money operator +(Money first, Money second)
    {
        if (first.currency != second.currency)
            throw new InvalidOperationException("Currencies have to be same");

        return new Money(first.Amount + second.Amount, first.currency);
    }


    public static Money Zero() => new Money(0, Currency.None);
    public static Money Zero(Currency currency) => new Money(0, currency);

    public bool IsZero(Currency currency) => this == Zero(currency);

}