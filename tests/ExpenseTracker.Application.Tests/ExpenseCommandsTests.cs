using ExpenseTracker.Application.Common;
using ExpenseTracker.Application.Common.Interfaces;
using ExpenseTracker.Application.Expenses.Commands;
using ExpenseTracker.Domain.Entities;
using FluentAssertions;
using Moq;

namespace ExpenseTracker.Application.Tests;

public class ExpenseCommandsTests
{
    private readonly Mock<IExpenseRepository> _mockExpenseRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    public ExpenseCommandsTests()
    {
        _mockExpenseRepository = new Mock<IExpenseRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
    }

    [Fact]
    public async Task AddExpense_WithValidData_ShouldCreateExpense()
    {
        // Arrange
        var command = new AddExpenseCommand(
            Amount: 100.00m,
            Category: "Food",
            Description: "Lunch",
            Date: DateTime.UtcNow,
            UserId: Guid.NewGuid());

        var handler = new AddExpenseCommandHandler(
            _mockExpenseRepository.Object,
            _mockUnitOfWork.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        _mockExpenseRepository.Verify(x => x.Add(It.IsAny<Expense>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateExpense_WithValidData_ShouldUpdateExpense()
    {
        // Arrange
        var existingExpense = Expense.Create(
            amount: 50.00m,
            category: "Food",
            description: "Old description",
            date: DateTime.UtcNow,
            userId: Guid.NewGuid());

        var command = new UpdateExpenseCommand(
            Id: existingExpense.Id,
            Amount: 75.00m,
            Category: "Groceries");

        _mockExpenseRepository
            .Setup(x => x.GetByIdAsync(existingExpense.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingExpense);

        var handler = new UpdateExpenseCommandHandler(
            _mockExpenseRepository.Object,
            _mockUnitOfWork.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _mockExpenseRepository.Verify(x => x.Update(It.IsAny<Expense>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        existingExpense.Amount.Should().Be(75.00m);
        existingExpense.Category.Should().Be("Groceries");
    }

    [Fact]
    public async Task UpdateExpense_WithNonExistentId_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new UpdateExpenseCommand(
            Id: Guid.NewGuid(),
            Amount: 75.00m,
            Category: "Groceries");

        _mockExpenseRepository
            .Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Expense)null);

        var handler = new UpdateExpenseCommandHandler(
            _mockExpenseRepository.Object,
            _mockUnitOfWork.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => 
            handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task DeleteExpense_WithValidId_ShouldDeleteExpense()
    {
        // Arrange
        var existingExpense = Expense.Create(
            amount: 50.00m,
            category: "Food",
            description: "Test expense",
            date: DateTime.UtcNow,
            userId: Guid.NewGuid());

        var command = new DeleteExpenseCommand(existingExpense.Id);

        _mockExpenseRepository
            .Setup(x => x.GetByIdAsync(existingExpense.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingExpense);

        var handler = new DeleteExpenseCommandHandler(
            _mockExpenseRepository.Object,
            _mockUnitOfWork.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _mockExpenseRepository.Verify(x => x.Remove(existingExpense), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteExpense_WithNonExistentId_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new DeleteExpenseCommand(Guid.NewGuid());

        _mockExpenseRepository
            .Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Expense)null);

        var handler = new DeleteExpenseCommandHandler(
            _mockExpenseRepository.Object,
            _mockUnitOfWork.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => 
            handler.Handle(command, CancellationToken.None));
    }
}