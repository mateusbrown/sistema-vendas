using Microsoft.AspNetCore.Mvc;
using SistemaVendasApi.Data;
using SistemaVendasApi.Core;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


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
        var response = new Models.ProdutoResponse();
        try
        {
            var lst = new List<Models.Produto>();
            _logger.LogTrace("GetProdutos");
            var lstCore = _core.GetProdutos();
            _logger.LogDebug("_core.GetProdutos", lstCore.ToArray());
            foreach(SistemaVendasApi.Models.Produtos p in lstCore)
            {
                var m = new Models.Produto();
                m.Fill(p);
                lst.Add(m);
                _logger.LogDebug("m.Fill", [m]);
            }
            _logger.LogDebug("response", [response]);
            _logger.LogTrace("Return OK");
            response.Data = lst;
            return Ok(lst);
        }
        catch(Exception ex)
        {
            response.Validation = new Core.Validate.Validation();
            response.Validation.Add(new Core.Validate.ModelValid()
            {
                Type = Core.Validate.ValidType.Error,
                Message = ex.Message
            });
            _logger.LogDebug("response",[response]);
            _logger.LogDebug("ex", [ex]);
            _logger.LogError("Exception",[ex]);
            return BadRequest(response);
        }
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        _logger.LogTrace("Get ID");
        var response = new Models.ProdutoResponse();
        try
        {
            _logger.LogTrace("GetProduto ID");
            var produto = _core.GetProduto(id);
            if (produto == null) return NotFound();
            else
            {
                var m = new Models.Produto();
                m.Fill(produto);
                response.Data = m;
                _logger.LogDebug("m.Fill", [m]);
                _logger.LogTrace("Return OK");
                return Ok(response);
            }
        }
        catch(Exception ex)
        {
            response.Validation = new Core.Validate.Validation();
            response.Validation.Add(new Core.Validate.ModelValid()
            {
                Type = Core.Validate.ValidType.Error,
                Message = ex.Message
            });
            _logger.LogDebug("response",[response]);
            _logger.LogDebug("ex", [ex]);
            _logger.LogError("Exception",[ex]);
            return BadRequest(response);
        }
    }
    
    // [HttpGet("codigo")]
    // public IActionResult Get(string codigo)
    // {
    //     _logger.LogTrace("Get codigo");
    //     var response = new Models.ProdutoResponse();
    //     try
    //     {
    //         _logger.LogTrace("GetProduto codigo");
    //         var produto = _core.GetProduto(codigo);
    //         _logger.LogDebug("produto",[produto]);
    //         if (produto == null)
    //         {
    //             _logger.LogTrace("Return NotFound");
    //             return NotFound();
    //         }
    //         else
    //         {
    //             var m = new Models.Produto();
    //             m.Fill(produto);
    //             response.Data = m;
    //             _logger.LogDebug("m.Fill", [m]);
    //             _logger.LogTrace("Return OK");
    //             return Ok(response);
    //         }
    //     }
    //     catch(Exception ex)
    //     {
    //         response.Validation = new Core.Validate.Validation();
    //         response.Validation.Add(new Core.Validate.ModelValid()
    //         {
    //             Type = Core.Validate.ValidType.Error,
    //             Message = ex.Message
    //         });
    //         _logger.LogDebug("response",[response]);
    //         _logger.LogDebug("ex", [ex]);
    //         _logger.LogError("Exception",[ex]);
    //         return BadRequest(response);
    //     }
    // }

    [HttpPost()]
    public IActionResult Post(Models.ProdutoRequest request)
    {
        _logger.LogTrace("Post");
        var response = new Models.ProdutoResponse();

        try
        {
            _logger.LogTrace("Produto.Fill");
            var produto = new Models.Produto();
            produto.Fill(request);
            _logger.LogDebug("Produto.Fill",[produto]);
            var produtoCore = produto.ToModel();
             _logger.LogTrace("_core.Add");
            produtoCore = _core.Add(produtoCore);
            _logger.LogDebug("produtoCore",[produtoCore]);
            if (produtoCore.ID.Equals(0))
            {
                throw new Exception("Não foi possível inserir o produto.");
            }
            else
            {
                produto.Fill(produtoCore);
                response.Data = produto;
                _logger.LogDebug("response", [response]);
                _logger.LogTrace("Return OK");
                return Ok(response);
            }
        }
        catch(Exception ex)
        {
            response.Validation = new Core.Validate.Validation();
            response.Validation.Add(new Core.Validate.ModelValid()
            {
                Type = Core.Validate.ValidType.Error,
                Message = ex.Message
            });
            _logger.LogDebug("response",[response]);
            _logger.LogDebug("ex", [ex]);
            _logger.LogError("Exception",[ex]);
            return BadRequest(response);
        }
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, Models.ProdutoRequest request)
    {
        _logger.LogTrace("Put");
        var response = new Models.ProdutoResponse();

        try
        {
            _logger.LogTrace("produto.Fill");
            var produto = new Models.Produto();
            produto.Fill(request);
            _logger.LogDebug("produto",[produto]);
            _logger.LogTrace("produto.ToModel");
            var produtoCore = produto.ToModel();
            _logger.LogTrace("_core.Update");
            produtoCore = _core.Update(produtoCore);
            _logger.LogDebug("produtoCore",[produtoCore]);
            produto.Fill(produtoCore);
            response.Data = produto;
            _logger.LogDebug("response", [response]);
            _logger.LogTrace("Return OK");
            return Ok(response);
        }
        catch(Exception ex)
        {
            response.Validation = new Core.Validate.Validation();
            response.Validation.Add(new Core.Validate.ModelValid()
            {
                Type = Core.Validate.ValidType.Error,
                Message = ex.Message
            });
            _logger.LogDebug("response",[response]);
            _logger.LogDebug("ex", [ex]);
            _logger.LogError("Exception",[ex]);
            return BadRequest(response);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _logger.LogTrace("Delete");
        var response = new Models.ProdutoResponse();

        try
        {
            _logger.LogTrace("_core.GetProduto");
            var produtoCore = _core.GetProduto(id);
            _logger.LogDebug("produtoCore",[produtoCore]);
            if (produtoCore == null)
            {
                _logger.LogTrace("Return NotFound");
                return NotFound();
            }
            else
            {
                _logger.LogTrace("_core.Delete");
                if (_core.Delete(produtoCore))
                {
                    response.Validation = new Core.Validate.Validation();
                    response.Validation.Add(new Core.Validate.ModelValid()
                    {
                        Type = Core.Validate.ValidType.Info,
                        Message = "Registro apagado."
                    });
                    _logger.LogDebug("response",[response]);
                    _logger.LogTrace("Return OK");
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
            response.Validation = new Core.Validate.Validation();
            response.Validation.Add(new Core.Validate.ModelValid()
            {
                Type = Core.Validate.ValidType.Error,
                Message = ex.Message
            });
            _logger.LogDebug("response",[response]);
            _logger.LogDebug("ex", [ex]);
            _logger.LogError("Exception",[ex]);
            return BadRequest(response);
        }
    }
}