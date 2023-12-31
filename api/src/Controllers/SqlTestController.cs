using Microsoft.AspNetCore.Mvc;
using SistemaVendasApi.Data;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SistemaVendasApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SqlTestController : ControllerBase
{
    private readonly ILogger<SqlTestController> _logger;
    private readonly SVContext _context;

    public SqlTestController(ILogger<SqlTestController> logger, SVContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogTrace("Get");
        try
        {
            _logger.LogTrace("SqlQuery");
            var dbReturn = _context.Database.SqlQuery<string>($"SELECT @@VERSION AS sql_version").ToList();
            var sqlVersion = dbReturn[0].ToString();
            
            _logger.LogDebug("sqlVersion",[sqlVersion]);
            _logger.LogTrace("Return OK");
            return Ok(sqlVersion);
        }
        catch(Exception ex)
        {
            _logger.LogDebug("Exception",[ex]);
            _logger.LogTrace("Return BadRequest");
            return BadRequest(ex);
        }
    }
}