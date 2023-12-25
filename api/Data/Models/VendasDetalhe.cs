using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SistemaVendasApi.Data.Models;

public class VendasDetalhe
{
    [Required]
    public int ID {get;set;} = 0;
    [Required]
    public int ID_Venda {get;set;} = 0;
    [Required]
    public int ID_Produto {get;set;} = 0;
    [Required]
    public int QT_Produto {get;set;} = 0;
    [Required]
    [DataType(DataType.Currency)]
    public decimal VL_Unitario_Produto {get;set;} = 0;
    [Required]
    [DataType(DataType.Currency)]
    public decimal VL_Produto_Total {get;set;} = 0;    
}