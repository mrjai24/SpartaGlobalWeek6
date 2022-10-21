using Microsoft.EntityFrameworkCore;
using NorthwindData;
using NorthwindData.Services;
using NUnit.Framework;
using System.Linq;

namespace NorthwindTests;

public class CustomerServiceTests
{
    private CustomerService _sut;
    private NorthwindContext _context;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var options = new DbContextOptionsBuilder<NorthwindContext>()
            .UseInMemoryDatabase(databaseName: "Example_DB")
            .Options;
        _context = new NorthwindContext(options);
        _sut = new CustomerService(_context);

        //Seed the database
        _sut.CreateCustomer(new Customer { CustomerId = "PHILL", ContactName = "Philip Windridge", CompanyName = "Sparta Global", City = "Birmingham" });
        _sut.CreateCustomer(new Customer { CustomerId = "MANDA", ContactName = "Nish Mandal", CompanyName = "Sparta Global", City = "Birmingham" });
    }

    [Test]
    public void GivenAValidId_CorrectCustomerIsReturned()
    {
        var result = _sut.GetCustomerById("PHILL");
        Assert.That(result, Is.TypeOf<Customer>());
        Assert.That(result.ContactName, Is.EqualTo("Philip Windridge"));
        Assert.That(result.CompanyName, Is.EqualTo("Sparta Global"));
        Assert.That(result.City, Is.EqualTo("Birmingham"));
    }

    [Test]
    public void GivenANewCustomer_CreateCustomerAddsItToTheDatabase()
    {
        var numberOfCustomersBefore = _context.Customers.Count();
        var newCustomer = new Customer
        {
            CustomerId = "ODELL",
            ContactName = "Max Odell",
            CompanyName = "Sparta Global",
            City = "Surrey"
        };
        _sut.CreateCustomer(newCustomer);
        var numberOfCustomersAfter = _context.Customers.Count();
        var result = _sut.GetCustomerById("ODELL");

        Assert.That(numberOfCustomersBefore + 1, Is.EqualTo(numberOfCustomersAfter));
        Assert.That(result, Is.TypeOf<Customer>());
        Assert.That(result.ContactName, Is.EqualTo("Max Odell"));
        Assert.That(result.CompanyName, Is.EqualTo("Sparta Global"));
        Assert.That(result.City, Is.EqualTo("Surrey"));

        Assert.That(newCustomer, Is.EqualTo(result));

        //clean up
        _context.Customers.Remove(newCustomer);
        _context.SaveChanges();
    }

    [Test]
    public void GetCustomerList_Returns_Expected()
    {
        var result = _sut.GetCustomerList();

        Assert.That(result.Count, Is.EqualTo(2));

        Assert.That(result[0].CustomerId, Is.EqualTo("PHILL"));
        Assert.That(result[0].ContactName, Is.EqualTo("Philip Windridge"));
        Assert.That(result[0].CompanyName, Is.EqualTo("Sparta Global"));
        Assert.That(result[0].City, Is.EqualTo("Birmingham"));

        Assert.That(result[1].CustomerId, Is.EqualTo("MANDA"));
        Assert.That(result[1].ContactName, Is.EqualTo("Nish Mandal"));
        Assert.That(result[1].CompanyName, Is.EqualTo("Sparta Global"));
        Assert.That(result[1].City, Is.EqualTo("Birmingham"));
    }

    [Test]
    public void GivenAValidId_RemoveCustomer_RemovesCorrectCustomer()
    {
        var newCustomer = new Customer
        {
            CustomerId = "KOTHA",
            ContactName = "Jai Kothari",
            CompanyName = "Sparta Global",
            City = "Birmingham"
        };
        _sut.CreateCustomer(newCustomer);
        _sut.RemoveCustomer(newCustomer);
        _sut.SaveCustomerChanges();
        var doesExist = _sut.GetCustomerById("KOTHA");
        Assert.That(doesExist, Is.EqualTo(null));
    }
}