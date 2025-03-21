using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository _repo;
    private readonly ILogger<EmployeesController> _logger;

    public EmployeesController(ILogger<EmployeesController> logger, IEmployeeRepository repo)
    {
        _logger = logger;
        _repo = repo;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var employees = _repo.GetEmployees();

        if (employees == null)
        {
            return NotFound();
        }

        return Ok(employees);
    }
}
