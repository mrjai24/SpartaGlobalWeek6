using System.Collections.Generic;
using System.Linq;

namespace NorthwindData.Services;

public class CustomerService : ICustomerService
{
    private readonly NorthwindContext _context;
    public CustomerService()
    {
        _context = new NorthwindContext();
    }
    public CustomerService(NorthwindContext context)
    {
        _context = context;
    }
    public void CreateCustomer(Customer customer)
    {
        _context.Customers.Add(customer);
        _context.SaveChanges();
    }
    public Customer GetCustomerById(string customerID) => _context.Customers.Find(customerID);
    public List<Customer> GetCustomerList() => _context.Customers.ToList();
    public void RemoveCustomer(Customer customer)
    {
        _context.Customers.Remove(customer);
        _context.SaveChanges();
    }
    public void SaveCustomerChanges() => _context.SaveChanges();
}