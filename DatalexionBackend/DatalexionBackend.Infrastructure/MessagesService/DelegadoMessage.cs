﻿using DatalexionBackend.Core.Domain.Entities;

namespace DatalexionBackend.Infrastructure.MessagesService;

public class DelegadoMessage : IMessage<Delegado>
{
    private readonly string _entityName = "Delegado";
    private readonly string _entityGender = "o";
    public string PartyNotFound(int partyId)
    {
        return $"No existe el partido Id: {partyId}.";
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
