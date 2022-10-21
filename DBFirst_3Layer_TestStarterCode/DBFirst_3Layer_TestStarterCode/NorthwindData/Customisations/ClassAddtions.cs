namespace NorthwindData;

public partial class Customer
{
    public override string ToString() => $"{CustomerId} - {ContactName} - {City} - {Country}";
}