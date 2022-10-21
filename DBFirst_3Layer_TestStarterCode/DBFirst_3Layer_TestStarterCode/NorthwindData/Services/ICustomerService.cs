using System.Collections.Generic;

namespace NorthwindData.Services;

public interface ICustomerService
{
    List<Customer> GetCustomerList();
    Customer GetCustomerById(string customerID);
    void CreateCustomer(Customer customer);
    void SaveCustomerChanges();
    void RemoveCustomer(Customer customer);
}