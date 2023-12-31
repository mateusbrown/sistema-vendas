using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SistemaVendasApi.Data.Models;
public class Produtos
{
    [Required]
    public int ID {get;set;} = 0;
    [Required]
    [StringLength(10)]
    public string CD_Produto {get;set;} = "";
    [Required]
    [StringLength(50)]
    public string DS_Produto {get;set;} = "";
    [Required]
    [DataType(DataType.Currency)]
    public decimal VL_Produto {get;set;} = 0;
}