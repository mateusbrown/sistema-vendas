using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SistemaVendasApi.Data.Models;

public class Vendas
{
    [Required]
    public int ID {get;set;} = 0;
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime DH_Inclusao {get;set;} = DateTime.Now;
    [Required]
    [DataType(DataType.Currency)]
    public decimal VL_Total_Produtos {get;set;} = 0;
    [Required]
    public int QT_Total_Produtos {get;set;} = 0;
}