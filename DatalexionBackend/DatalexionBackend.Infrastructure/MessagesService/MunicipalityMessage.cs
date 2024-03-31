using DatalexionBackend.Core.Domain.Entities;

namespace DatalexionBackend.Infrastructure.MessagesService;

public class MunicipalityMessage : IMessage<Municipality>
{
    private readonly string _entityName = "Municipio";
    private readonly string _entityGender = "o";
    public string ProvinceNotFound(int provinceId)
    {
        return $"No existe el departamento Id: {provinceId}.";
    }
    public string ClientNotFound(int clientId)
    {
        return $"No existe el cliente Id: {clientId}.";
    }
    public string NotFoundGeneric()
    {
        return $"El sistema no tiene {_entityName} asignad{_entityGender}.";
    }
    public string InternalError()
    {
        return $"Ocurrió un error en el servidor.";
    }
    public string NameAlreadyExists(string name)
    {
        return $"El nombre {name} ya existe en el sistema.";
    }
    public string NotFound(int id)
    {
        return $"{_entityName} no encontrad{_entityGender} Id: {id}.";
    }
    public string NotValid()
    {
        return $"Datos de entrada inválidos para {_entityName}.";
    }
    public string Created(int id, string name = "")
    {
        return $"{_entityName} creado con éxito Id: {id}.";
    }
    public string Updated(int id, string name = "")
    {
        return $"{_entityName} actualizad{_entityGender} correctamente Id: {id}.";
    }
    public string Deleted(int id, string name = "")
    {
        return $"{_entityName} eliminad{_entityGender} con éxito.";
    }
    public string ActionLog(int id, string name)
    {
        return $"{_entityName} Id: {id}, Nombre: {name}";
    }
}
