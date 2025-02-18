using ExpenseTracker.Application.Expenses.Commands;
using ExpenseTracker.Application.Expenses.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExpensesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ExpensesController> _logger;
    
    public ExpensesController(IMediator mediator, ILogger<ExpensesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(AddExpenseCommand command)
    {
        _logger.LogInformation("Creating new expense for user {UserId}", command.UserId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExpense(Guid id, UpdateExpenseCommand command)
    {
        if (id != command.Id)
            return BadRequest("The ID in the URL must match the ID in the request body");

        _logger.LogInformation("Updating expense {ExpenseId}", id);
        await _mediator.Send(command);
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpense(Guid id)
    {
        _logger.LogInformation("Deleting expense {ExpenseId}", id);
        await _mediator.Send(new DeleteExpenseCommand(id));
        return NoContent();
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ExpenseDto>> GetExpenseById(Guid id)
    {
        var expense = await _mediator.Send(new GetExpenseByIdQuery(id));
        return Ok(expense);
    }
    
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<List<ExpenseDto>>> GetUserExpenses(
        Guid userId,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var expenses = await _mediator.Send(new GetUserExpensesQuery(userId, startDate, endDate));
        return Ok(expenses);
    }
    
    [HttpGet("user/{userId}/summary")]
    public async Task<ActionResult<ExpenseSummaryDto>> GetUserExpenseSummary(
        Guid userId,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var summary = await _mediator.Send(new GetUserExpenseSummaryQuery(userId, startDate, endDate));
        return Ok(summary);
    }
    
    [HttpGet("user/{userId}/categories")]
    public async Task<ActionResult<List<CategoryExpenseDto>>> GetExpensesByCategory(
        Guid userId,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var categoryExpenses = await _mediator.Send(
            new GetExpensesByCategoryQuery(userId, startDate, endDate));
        return Ok(categoryExpenses);
    }
}