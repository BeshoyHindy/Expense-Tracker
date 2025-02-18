using ExpenseTracker.Domain.Entities;
using FluentAssertions;

namespace ExpenseTracker.Domain.Tests;

public class ExpenseTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateExpense()
    {
        // Arrange
        var amount = 100.00m;
        var category = "Food";
        var description = "Lunch";
        var date = DateTime.UtcNow;
        var userId = Guid.NewGuid();

        // Act
        var expense = Expense.Create(amount, category, description, date, userId);

        // Assert
        expense.Should().NotBeNull();
        expense.Amount.Should().Be(amount);
        expense.Category.Should().Be(category);
        expense.Description.Should().Be(description);
        expense.Date.Should().Be(date);
        expense.UserId.Should().Be(userId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_WithInvalidAmount_ShouldThrowDomainException(decimal amount)
    {
        // Arrange
        var category = "Food";
        var description = "Lunch";
        var date = DateTime.UtcNow;
        var userId = Guid.NewGuid();

        // Act & Assert
        var action = () => Expense.Create(amount, category, description, date, userId);
        action.Should().Throw<DomainException>()
            .WithMessage("Amount must be greater than zero");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_WithInvalidCategory_ShouldThrowDomainException(string category)
    {
        // Arrange
        var amount = 100.00m;
        var description = "Lunch";
        var date = DateTime.UtcNow;
        var userId = Guid.NewGuid();

        // Act & Assert
        var action = () => Expense.Create(amount, category, description, date, userId);
        action.Should().Throw<DomainException>()
            .WithMessage("Category is required");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_WithInvalidDescription_ShouldThrowDomainException(string description)
    {
        // Arrange
        var amount = 100.00m;
        var category = "Food";
        var date = DateTime.UtcNow;
        var userId = Guid.NewGuid();

        // Act & Assert
        var action = () => Expense.Create(amount, category, description, date, userId);
        action.Should().Throw<DomainException>()
            .WithMessage("Description is required");
    }

    [Fact]
    public void Update_WithValidData_ShouldUpdateExpense()
    {
        // Arrange
        var expense = Expense.Create(
            100.00m,
            "Food",
            "Initial description",
            DateTime.UtcNow,
            Guid.NewGuid());

        var newAmount = 150.00m;
        var newCategory = "Groceries";

        // Act
        expense.Update(newAmount, newCategory);

        // Assert
        expense.Amount.Should().Be(newAmount);
        expense.Category.Should().Be(newCategory);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Update_WithInvalidAmount_ShouldThrowDomainException(decimal amount)
    {
        // Arrange
        var expense = Expense.Create(
            100.00m,
            "Food",
            "Initial description",
            DateTime.UtcNow,
            Guid.NewGuid());

        // Act & Assert
        var action = () => expense.Update(amount, "New Category");
        action.Should().Throw<DomainException>()
            .WithMessage("Amount must be greater than zero");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Update_WithInvalidCategory_ShouldThrowDomainException(string category)
    {
        // Arrange
        var expense = Expense.Create(
            100.00m,
            "Food",
            "Initial description",
            DateTime.UtcNow,
            Guid.NewGuid());

        // Act & Assert
        var action = () => expense.Update(150.00m, category);
        action.Should().Throw<DomainException>()
            .WithMessage("Category is required");
    }
}