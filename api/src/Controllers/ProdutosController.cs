using Microsoft.AspNetCore.Mvc;
using SistemaVendasApi.Data;
using SistemaVendasApi.Services;

namespace SistemaVendasApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly ILogger<ProdutosController> _logger;
    private readonly Produtos _core;

    public ProdutosController(ILogger<ProdutosController> logger, SVContext context)
    {
        _logger = logger;
        _core = new Produtos(context);
    }    

    [HttpGet()]
    public IActionResult Get()
    {
        _logger.LogTrace("Get");
        var response = new Models.Response();
        try
        {
            var lst = new List<Models.ProdutoResponse>();
            _logger.LogTrace("GetProdutos");
            var lstCore = _core.GetProdutos();
            _logger.LogDebug("_core.GetProdutos", lstCore.ToArray());
            foreach(SistemaVendasApi.Models.Produtos p in lstCore)
            {
                var m = Models.Produto.ConvertResponse(p);                
                _logger.LogDebug("ConvertResponse", [m]);
                lst.Add(m);
            }
            Models.ExceptionResponse.Ok(ref response, lst, _logger);            
            return Ok(response);
        }
        catch(Exception ex)
        {
            Models.ExceptionResponse.Error(ref response,ex,_logger);
            return BadRequest(response);
        }
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        _logger.LogTrace("Get ID");
        var response = new Models.Response();
        try
        {
            _logger.LogTrace("GetProduto ID");
            var produto = _core.GetProduto(id);
            if (produto == null) return NotFound();
            else
            {
                var m = new Models.ProdutoResponse();
                m = Models.Produto.ConvertResponse(produto);
                Models.ExceptionResponse.Ok(ref response, m, _logger);
                return Ok(response);
            }
        }
        catch(Exception ex)
        {
            Models.ExceptionResponse.Error(ref response,ex,_logger);
            return BadRequest(response);
        }
    }
   
    [HttpPost()]
    public IActionResult Post(Models.ProdutoRequest request)
    {
        _logger.LogTrace("Post");
        var response = new Models.Response();

        try
        {
            _logger.LogTrace("ConvertModel");
            var produtoCore = Models.Produto.ConvertModel(request);
            _logger.LogDebug("produtoCore",[produtoCore]);
            _logger.LogTrace("_core.Add");
            produtoCore = _core.Add(produtoCore);
            _logger.LogDebug("produtoCore",[produtoCore]);
            if (produtoCore.ID.Equals(0))
            {
                throw new Exception("Não foi possível inserir o produto.");
            }
            else
            {
                Models.ExceptionResponse.Ok(ref response, Models.Produto.ConvertResponse(produtoCore), _logger);
                return Ok(response);
            }
        }
        catch(Exception ex)
        {
            Models.ExceptionResponse.Error(ref response,ex,_logger);
            return BadRequest(response);
        }
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, Models.ProdutoRequest request)
    {
        _logger.LogTrace("Put");
        var response = new Models.Response();

        try
        {
            var produtoCore = _core.GetProduto(id);
            if (produtoCore != null)
            {
                _logger.LogTrace("ConvertModel");
                var produtoAlt = Models.Produto.ConvertModel(request);
                produtoAlt.ID = produtoCore.ID;
                _logger.LogDebug("produtoAlt",[produtoAlt]);
                _logger.LogTrace("_core.Add");
                _core.Update(produtoAlt);
                Models.ExceptionResponse.Ok(ref response, "", _logger);
                return Ok(response);
            }
            else
            {
                response.Validation = new Validation.ValidateProcess();
                response.Validation.Add(new Validation.ModelValid()
                {
                    Type = Validation.ValidType.Error,
                    Message = "Produto não encontrado"
                });
                _logger.LogDebug("response",[response]);
                return NotFound(response);
            }
            
        }
        catch(Exception ex)
        {
            Models.ExceptionResponse.Error(ref response,ex,_logger);
            return BadRequest(response);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _logger.LogTrace("Delete");
        var response = new Models.Response();

        try
        {
            _logger.LogTrace("_core.GetProduto");
            var produtoCore = _core.GetProduto(id);
            _logger.LogDebug("produtoCore",[produtoCore]);
            if (produtoCore == null)
            {
                Models.ExceptionResponse.Warning(ref response, "Produto não encontrado", _logger);
                return NotFound(response);
            }
            else
            {
                _logger.LogTrace("_core.Delete");
                if (_core.Delete(produtoCore))
                {
                    Models.ExceptionResponse.Ok(ref response, "Registro apagado.", _logger);
                    return Ok(response);
                }
                else
                {
                    throw new Exception("Não foi possível atualizar o produto.");
                }
            }
        }
        catch(Exception ex)
        {
            Models.ExceptionResponse.Error(ref response,ex,_logger);
            return BadRequest(response);
        }
    }
}