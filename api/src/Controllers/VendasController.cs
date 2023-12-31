using Microsoft.AspNetCore.Mvc;
using SistemaVendasApi.Data;
using SistemaVendasApi.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SistemaVendasApi.Controllers.Models;


namespace SistemaVendasApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VendasController : ControllerBase
{
    private readonly ILogger<VendasController> _logger;
    private readonly Vendas _coreVenda;

    public VendasController(ILogger<VendasController> logger, SVContext context)
    {
        _logger = logger;
        _coreVenda = new Vendas(context);
    }

    [HttpGet()]
    public IActionResult Get()
    {
        _logger.LogTrace("Get");
        var response = new Models.Response();
        
        try
        {
            var lst = new List<Models.VendaResponse>();
            _logger.LogTrace("GetVendas");
            var lstVendas = _coreVenda.GetVendas();
            _logger.LogDebug("_coreVenda.GetVendas",lstVendas.ToArray());
            foreach(var v in lstVendas)
            {
                var m = Models.Venda.ConvertResponse(v);
                if (v.Detalhes.Count > 0)
                {
                    m.Detalhes = new List<Models.VendaDetalheResponse>();
                }
                _logger.LogTrace("Venda.ConvertResponse");
                _logger.LogDebug("Venda.ConvertResponse", [m]);
                
                foreach (var d in v.Detalhes)
                {
                    _logger.LogDebug("VendaDetalhe", [d]);
                    _logger.LogTrace("VendaDetalhe.ConvertResponse");
                    var vd = Models.VendaDetalhe.ConvertResponse(d);
                    _logger.LogDebug("VendaDetalhe.ConvertResponse", [vd]);
                    m.Detalhes?.Add(vd);
                }                
                _logger.LogDebug("lst.Add", [m]);
                lst.Add(m);
            }
            Models.ExceptionResponse.Ok(ref response, lst, _logger);
            return Ok(response);

        }
        catch(Exception ex)
        {
            Models.ExceptionResponse.Error(ref response, ex, _logger);
            return BadRequest(response);
        }
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        _logger.LogTrace("Get");
        var response = new Models.Response();
        
        try
        {
            _logger.LogTrace("GetVenda");
            var venda = _coreVenda.GetVenda(id);
            _logger.LogDebug("_coreVenda.GetVendas",[venda]);
            if (venda != null)
            {
                var m = Models.Venda.ConvertResponse(venda);
                if (venda.Detalhes.Count > 0)
                {
                    m.Detalhes = new List<Models.VendaDetalheResponse>();
                }
                _logger.LogTrace("Venda.ConvertResponse");
                _logger.LogDebug("Venda.ConvertResponse", [m]);
                
                foreach (var d in venda.Detalhes)
                {
                    _logger.LogDebug("VendaDetalhe", [d]);
                    _logger.LogTrace("VendaDetalhe.ConvertResponse");
                    var vd = Models.VendaDetalhe.ConvertResponse(d);
                    _logger.LogDebug("VendaDetalhe.ConvertResponse", [vd]);
                    m.Detalhes?.Add(vd);
                }                
                
                Models.ExceptionResponse.Ok(ref response, m, _logger);
                return Ok(response);
            }
            else
            {
                Models.ExceptionResponse.Warning(ref response, "Venda não encontrada", _logger);
                return NotFound(response);
            }

        }
        catch(Exception ex)
        {
            Models.ExceptionResponse.Error(ref response,ex,_logger);
            return BadRequest(response);
        }
    }

    [HttpGet("{id_venda}/{id_detalhe}")]
    public IActionResult Get(int id_venda, int id_detalhe)
    {
        _logger.LogTrace("Get");
        var response = new Models.Response();
        
        try
        {
            _logger.LogTrace("GetVenda");
            var venda = _coreVenda.GetVenda(id_venda);
            _logger.LogDebug("_coreVenda.GetVendas",[venda]);
            if (venda != null)
            {
                _logger.LogTrace("GetDetalhe");
                var detalhe = (SistemaVendasApi.Models.VendasDetalhe)venda.Detalhes.Where(r => r.ID.Equals(id_detalhe));
                _logger.LogDebug("_coreVenda.GetDetalhes",[detalhe]);
                if (detalhe != null)
                {
                    _logger.LogTrace("Venda.ConvertResponse");
                    var m = Models.VendaDetalhe.ConvertResponse(detalhe);
                    Models.ExceptionResponse.Ok(ref response, m, _logger);
                    return Ok(response);
                }
                else
                {
                    ExceptionResponse.Warning(ref response, "Detalhe da venda não encontrado", _logger);
                    return NotFound(response);
                }
            }
            else
            {
                ExceptionResponse.Warning(ref response, "Venda não encontrada", _logger);
                return NotFound(response);
            }
        }
        catch(Exception ex)
        {
            Models.ExceptionResponse.Error(ref response,ex,_logger);
            return BadRequest(response);
        }
    }

    [HttpPost()]
    public IActionResult Post(VendaRequest request)
    {
        _logger.LogTrace("Post");
        var response = new Models.Response();

        try
        {
            _logger.LogTrace("ConvertModel");
            var vendaCore = Models.Venda.ConvertModel(request);
            _logger.LogDebug("vendaCore",[vendaCore]);
            _logger.LogTrace("_core.Add");
            vendaCore = _coreVenda.AddVenda(vendaCore);
            _logger.LogDebug("vendaCore",[vendaCore]);
            if (vendaCore.ID.Equals(0))
            {
                throw new Exception("Não foi possível inserir a venda.");
            }
            else
            {
                ExceptionResponse.Ok(ref response, Models.Venda.ConvertResponse(vendaCore), _logger);
                return Ok(response);
            }
        }
        catch(Exception ex)
        {
            Models.ExceptionResponse.Error(ref response,ex,_logger);
            return BadRequest(response);
        }
    }

    [HttpPost("{id}")]
    public IActionResult Post(int id, VendaDetalheRequest request)
    {
        _logger.LogTrace("Post");
        var response = new Models.Response();

        try
        {
            var venda = _coreVenda.GetVenda(id);
            if (venda != null)
            {
                _logger.LogTrace("ConvertModel");
                var vendaDetalheCore = Models.VendaDetalhe.ConvertModel(request);
                _logger.LogDebug("vendaDetalheCore",[vendaDetalheCore]);
                _logger.LogTrace("_core.Add");
                vendaDetalheCore = _coreVenda.AddDetalhe(venda, vendaDetalheCore);
                _logger.LogDebug("vendaDetalheCore",[vendaDetalheCore]);
                if (vendaDetalheCore.ID.Equals(0))
                {
                    throw new Exception("Não foi possível inserir o detalhe.");
                }
                else
                {
                    ExceptionResponse.Ok(ref response, VendaDetalhe.ConvertResponse(vendaDetalheCore), _logger);
                    return Ok(response);
                }
            }
            else
            {
                ExceptionResponse.Warning(ref response, "Venda não encontrada", _logger);
                return NotFound(response);
            }
            
        }
        catch(Exception ex)
        {
            Models.ExceptionResponse.Error(ref response,ex,_logger);
            return BadRequest(response);
        }
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, VendaRequest request)
    {
        _logger.LogTrace("Put");
        var response = new Models.Response();

        try
        {
            var venda = _coreVenda.GetVenda(id);
            if (venda != null)
            {
                _logger.LogTrace("ConvertModel");
                var vendaCore = Models.Venda.ConvertModel(request);
                _logger.LogDebug("vendaCore",[vendaCore]);
                _logger.LogTrace("_core.Add");
                vendaCore = _coreVenda.UpdateVenda(vendaCore);
                _logger.LogDebug("vendaCore",[vendaCore]);
                ExceptionResponse.Ok(ref response, Models.Venda.ConvertResponse(vendaCore), _logger);
                return Ok(response);
            }
            else
            {
                ExceptionResponse.Warning(ref response, "Venda não encontrada", _logger);
                return NotFound(response);
            }
        }
        catch(Exception ex)
        {
            Models.ExceptionResponse.Error(ref response,ex,_logger);
            return BadRequest(response);
        }
    }

    [HttpPut("{id_venda}/{id_detalhe}")]
    public IActionResult Put(int id_venda, int id_detalhe, VendaDetalheRequest request)
    {
        _logger.LogTrace("Put");
        var response = new Models.Response();

        try
        {
            var venda = _coreVenda.GetVenda(id_venda);
            if (venda != null)
            {
                var detalhe = (SistemaVendasApi.Models.VendasDetalhe)venda.Detalhes.Where(r => r.ID.Equals(id_detalhe));
                if (detalhe != null)
                {
                    _logger.LogTrace("ConvertModel");
                    var detalheCore = Models.VendaDetalhe.ConvertModel(request);
                    _logger.LogDebug("detalheCore",[detalheCore]);
                    _logger.LogTrace("_core.UpdateDetalhe");
                    detalheCore = _coreVenda.UpdateDetalhe(venda,detalheCore);
                    _logger.LogDebug("detalheCore",[detalheCore]);
                    ExceptionResponse.Ok(ref response, VendaDetalhe.ConvertResponse(detalheCore), _logger);
                    return Ok(response);
                }
                else
                {
                    ExceptionResponse.Warning(ref response, "Detalhe da venda não encontrado", _logger);
                    return NotFound(response);
                }
            }
            else
            {
                ExceptionResponse.Warning(ref response, "Venda não encontrada", _logger);
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
            var venda = _coreVenda.GetVenda(id);
            if (venda != null)
            {
                _logger.LogTrace("ConvertModel");
                _logger.LogTrace("_core.Add");
                if (_coreVenda.DeleteVenda(venda))
                {
                    ExceptionResponse.Ok(ref response, "Registro apagado", _logger);
                    return Ok(response);
                }
                else
                {
                    ExceptionResponse.Error(ref response, "Não possível apagar a venda", _logger);
                    return BadRequest(response);
                }
            }
            else
            {
                ExceptionResponse.Warning(ref response, "Venda não encontrada", _logger);
                return NotFound(response);
            }
        }
        catch(Exception ex)
        {
            Models.ExceptionResponse.Error(ref response,ex,_logger);
            return BadRequest(response);
        }
    }

    [HttpDelete("{id_venda}/{id_detalhe}")]
    public IActionResult Delete(int id_venda, int id_detalhe)
    {
        _logger.LogTrace("Delete");
        var response = new Models.Response();

        try
        {
            var venda = _coreVenda.GetVenda(id_venda);
            if (venda != null)
            {
                var detalhe = (SistemaVendasApi.Models.VendasDetalhe)venda.Detalhes.Where(r => r.ID.Equals(id_detalhe));
                if (detalhe != null)
                {
                    if (_coreVenda.DeleteDetalhe(venda,detalhe))
                    {
                        ExceptionResponse.Ok(ref response, "Registro apagado", _logger);
                        return Ok(response);
                    }
                    else
                    {
                        ExceptionResponse.Error(ref response, "Não possível apagar a venda", _logger);
                        return BadRequest(response);
                    }
                }
                else
                {
                    ExceptionResponse.Warning(ref response, "Detalhe da venda não encontrada", _logger);
                    return NotFound(response);
                }
            }
            else
            {
                ExceptionResponse.Warning(ref response, "Venda não encontrada", _logger);
                return NotFound(response);
            }
        }
        catch(Exception ex)
        {
            Models.ExceptionResponse.Error(ref response,ex,_logger);
            return BadRequest(response);
        }
    }
}