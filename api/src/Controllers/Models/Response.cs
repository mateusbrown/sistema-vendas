using SistemaVendasApi.Validation;

namespace SistemaVendasApi.Controllers.Models;
public class Response
{
    public object ?Data {get;set;}
    public ValidateProcess ?Validation {get;set;}
}