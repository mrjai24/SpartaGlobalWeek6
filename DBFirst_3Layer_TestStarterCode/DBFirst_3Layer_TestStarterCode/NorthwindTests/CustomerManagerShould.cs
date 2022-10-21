using Microsoft.EntityFrameworkCore;
using Moq;
using NorthwindBusiness;
using NorthwindData;
using NorthwindData.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace NorthwindTests;

public class CustomerManagerShould
{
    private CustomerManager _sut;

    [Ignore("Should fail")]
    [Test]
    public void BeAbleToConstructCustomerManager()
    {
        _sut = new CustomerManager(null);
        Assert.That(_sut, Is.InstanceOf<CustomerManager>());
    }

    [Test]
    public void BeAbleToConstruct_UsingMoq()
    {
        var mockObject = new Mock<ICustomerService>();
        _sut = new CustomerManager(mockObject.Object);
        Assert.That(_sut, Is.InstanceOf<CustomerManager>());
    }

    //Stub
    [Category("Happy Path")]
    [Test]
    public void ReturnTrue_WhenUpdateIsCalled_WithValidId()
    {
        //Arrange
        var mockObject = new Mock<ICustomerService>();
        var originalCustomer = new Customer
        {
            CustomerId = "MANDA"
        };
        mockObject.Setup(cs => cs.GetCustomerById("MANDA")).Returns(originalCustomer);

        _sut = new CustomerManager(mockObject.Object);
        //Act
        var result = _sut.Update("MANDA", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
        Assert.That(result);
    }

    [Category("Happy Path")]
    [Test]
    public void UpdateSelectedCustomer_WhenUpdateIsCalled_WithValidId()
    {
        //Arrange
        var mockObject = new Mock<ICustomerService>();
        var originalCustomer = new Customer
        {
            CustomerId = "MANDA",
            ContactName = "Nish Mandal",
            CompanyName = "Sparta Global",
            City = "Birmnigham"
        };
        mockObject.Setup(cs => cs.GetCustomerById("MANDA")).Returns(originalCustomer);

        _sut = new CustomerManager(mockObject.Object);
        //Act
        var result = _sut.Update("MANDA", "Nish Mandal", "UK", "London", null);
        Assert.That(_sut.SelectedCustomer.ContactName, Is.EqualTo("Nish Mandal"));
        Assert.That(_sut.SelectedCustomer.Country, Is.EqualTo("UK"));
        Assert.That(_sut.SelectedCustomer.City, Is.EqualTo("London"));
    }

    [Category("Sad Path")]
    [Test]
    public void ReturnsFalse_WhenUpdateIsCalled_WithInvalidId()
    {
        //Arrange
        var mockObject = new Mock<ICustomerService>();
        mockObject.Setup(cs => cs.GetCustomerById(It.IsAny<string>())).Returns((Customer)null);
        _sut = new CustomerManager(mockObject.Object);
        //Act
        var result = _sut.Update(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
        //Assert
        Assert.That(result, Is.False);
    }

    [Category("Sad Path")]
    [Test]
    public void NotChangeTheSelectedCustomer_WhenUpdateIsCalled_WithValidId()
    {
        var mockObject = new Mock<ICustomerService>();
        mockObject.Setup(cs => cs.GetCustomerById("MANDA")).Returns((Customer)null);
        var originalCustomer = new Customer
        {
            CustomerId = "MANDA",
            ContactName = "Nish Mandal",
            CompanyName = "Sparta Global",
            City = "Birmingham"
        };
        _sut = new CustomerManager(mockObject.Object);
        _sut.SetSelectedCustomer(originalCustomer);

        var result = _sut.Update("MANDA", "Nish Mandal", "UK", "London", null);
        Assert.That(_sut.SelectedCustomer.ContactName, Is.EqualTo("Nish Mandal"));
        Assert.That(_sut.SelectedCustomer.Country, Is.EqualTo(null));
        Assert.That(_sut.SelectedCustomer.City, Is.EqualTo("Birmingham"));
    }

    [Category("Happy Path")]
    [Test]
    public void Delete_ReturnsTrue_WhenItIsCalled()
    {
        // Arrange
        var mockObject = new Mock<ICustomerService>();
        Customer Nish = new Customer
        {
            CustomerId = "Manda"
        };
        mockObject.Setup(mo => mo.GetCustomerById("Manda")).Returns(Nish);
        _sut = new CustomerManager(mockObject.Object);
        var result = _sut.Delete("Manda");
        Assert.That(result, Is.True);
    }

    [Category("Happy Path")]
    [Test]
    public void ACustomerIsDeletedWhen_Delete_IsCalledWithAValidId()
    {
        // Arrange
        var mockObject = new Mock<ICustomerService>();
        Customer Nish = new Customer
        {
            CustomerId = "Manda",
            ContactName = "Nish Mandal",
            CompanyName = "Sparta Global",
            City = "Birmingham"
        };
        mockObject.Setup(mo => mo.GetCustomerById("Manda")).Returns(Nish);
        _sut = new CustomerManager(mockObject.Object);
        var result = _sut.Delete("Manda");
        Assert.That(result, Is.True);
        var userExists = _sut.RetrieveAll();
        Assert.That(userExists, Is.Null);
    }

    [Category("Sad Path")]
    [Test]
    public void DoesNotDeleteSelectedCustomer_WehnDeleteIsCalled_WithValidId()
    {
        var mockObject = new Mock<ICustomerService>();
        mockObject.Setup(cs => cs.GetCustomerById(It.IsAny<string>())).Returns((Customer)null);
        _sut = new CustomerManager(mockObject.Object);
        var reault = _sut.Delete(It.IsAny<string>());
        Assert.That(reault, Is.False);
    }

    [Test]
    public void ReturnTrue_WhenDeleteIsCalledWithValidId()
    {
        // Arrange
        var mockCustomerService = new Mock<ICustomerService>();
        var customer = new Customer()
        {
            CustomerId = "ROCK",
        };
        mockCustomerService.Setup(cs => cs.GetCustomerById("ROCK")).Returns(customer);
        _sut = new CustomerManager(mockCustomerService.Object);
        // Act
        var result = _sut.Delete("ROCK");

        // Assert
        Assert.That(result);
    }

    [Test]
    public void SetSelectedCustomerToNull_WhenDeleteIsCalledWithValidId()
    {
        // Arrange
        var mockCustomerService = new Mock<ICustomerService>();
        var customer = new Customer()
        {
            CustomerId = "ROCK",
        };
        _sut.SelectedCustomer = customer;
        mockCustomerService.Setup(cs => cs.GetCustomerById("ROCK")).Returns(customer);
        _sut = new CustomerManager(mockCustomerService.Object);
        // Act
        var result = _sut.Delete("ROCK");

        // Assert
        Assert.That(_sut.SelectedCustomer, Is.Null);
    }

    [Test]
    public void ReturnFalse_WhenDeleteIsCalled_WithInvalidId()
    {
        // Arrange
        var mockCustomerService = new Mock<ICustomerService>();

        mockCustomerService.Setup(cs => cs.GetCustomerById("ROCK")).Returns((Customer)null);
        _sut = new CustomerManager(mockCustomerService.Object);
        // Act
        var result = _sut.Delete("ROCK");

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void NotChangeTheSelectedCustomer_WhenDeleteIsCalled_WithInvalidId()
    {
        // Arrange
        var mockCustomerService = new Mock<ICustomerService>();

        mockCustomerService.Setup(cs => cs.GetCustomerById("ROCK")).Returns((Customer)null);

        var originalCustomer = new Customer()
        {
            CustomerId = "ROCK",
            ContactName = "Rocky Raccoon",
            CompanyName = "Zoo UK",
            City = "Telford"

        };

        _sut = new CustomerManager(mockCustomerService.Object);
        _sut.SelectedCustomer = originalCustomer;
        // Act
        _sut.Delete("ROCK");

        // Assert that SelectedCustomer is unchanged
        Assert.That(_sut.SelectedCustomer.ContactName, Is.EqualTo("Rocky Raccoon"));
        Assert.That(_sut.SelectedCustomer.CompanyName, Is.EqualTo("Zoo UK"));
        Assert.That(_sut.SelectedCustomer.Country, Is.EqualTo(null));
        Assert.That(_sut.SelectedCustomer.City, Is.EqualTo("Telford"));
    }

    //Using Moq to throw exceptions
    [Category("Sad Path - Update")]
    [Test]
    public void ReternFalse_WhenUpdateIsCalled_AndDatabaseThrowsException()
    {
        //Arrange
        var mockCustomerService = new Mock<ICustomerService>();
        mockCustomerService.Setup(cs => cs.GetCustomerById(It.IsAny<string>())).Returns(new Customer());
        mockCustomerService.Setup(cs => cs.SaveCustomerChanges()).Throws<DbUpdateConcurrencyException>();

        _sut = new CustomerManager(mockCustomerService.Object);
        var result = _sut.Update(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
        
        Assert.That(result, Is.False);
    }

    [Category("Behaviour Based Testing")]
    [Test]
    public void CallSaveCustomerChanges_WhenUpdateIsCalled_WithValidId()
    {
        var mockCustomerService = new Mock<ICustomerService>();
        mockCustomerService.Setup(cs => cs.GetCustomerById(It.IsAny<string>())).Returns(new Customer());

        _sut = new CustomerManager(mockCustomerService.Object);
        var result = _sut.Update(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

        mockCustomerService.Verify(cs => cs.SaveCustomerChanges(), Times.Once);
        mockCustomerService.Verify(cs => cs.SaveCustomerChanges(), Times.Exactly(1));
        mockCustomerService.Verify(cs => cs.SaveCustomerChanges(), Times.AtMostOnce);
        mockCustomerService.Verify(cs => cs.GetCustomerList(), Times.Never);
    }

    [Test]
    public void LetsSeeWhatHappens_WhenUpdateIsCalled_IfAllInvocations_AreNotSetUp()
    {
        var mockCustomerService = new Mock<ICustomerService>();
        mockCustomerService.Setup(cs => cs.GetCustomerById(It.IsAny<string>())).Returns(new Customer());

        _sut = new CustomerManager(mockCustomerService.Object);
        var result = _sut.Update(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

        Assert.That(result);
    }

    [Test]
    public void Create_CallsCreateCustomerMethod()
    {
        var mockCustomerService = new Mock<ICustomerService>();
        _sut = new CustomerManager(mockCustomerService.Object);
        _sut.Create(It.IsAny<String>(), It.IsAny<string>(), It.IsAny<string>());
        mockCustomerService.Verify(cs => cs.CreateCustomer(It.IsAny<Customer>()), Times.Once);
    }

    [Test]
    public void Delete_CallsGetCustomerAndRemoveCustomer()
    {
        var mockCustomerService = new Mock<ICustomerService>();
        var originalCustomer = new Customer { CustomerId = "MANDA" };
        mockCustomerService.Setup(c=>c.GetCustomerById("MANDA")).Returns(originalCustomer);
        _sut = new CustomerManager(mockCustomerService.Object);
        _sut.Delete(originalCustomer.CustomerId);
        mockCustomerService.Verify(cs => cs.GetCustomerById(originalCustomer.CustomerId), Times.Once);
        mockCustomerService.Verify(cs => cs.RemoveCustomer(originalCustomer), Times.Once);
    }

    [Test]
    public void RetrieveAll_ReturnsCustomerInTheDatabase()
    {
        var mockCustomerService = new Mock<ICustomerService>();
        var newCustomer = new Customer
        {
            CustomerId = "MANDO",
            ContactName = "Nish Mandal",
            CompanyName = "Sparta Global",
            City = "Birmingham"
        };
        mockCustomerService.Setup(cs => cs.GetCustomerList()).Returns(new List<Customer> { newCustomer });
        _sut = new CustomerManager(mockCustomerService.Object);
        var result = _sut.RetrieveAll();
        Assert.That(result, Is.TypeOf<Customer>());
        Assert.That(result.Contains(newCustomer));
    }
}