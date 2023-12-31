using SistemaVendasApi.Core.Validate;

namespace SistemaVendasApi.Controllers.Models;
public class Response
{
    public object ?Data {get;set;}
    public Validation ?Validation {get;set;}
}