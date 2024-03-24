namespace DatalexionBackend.Core.Helpers;

public static class Messages
{
    public static class Generic
    {
        public const string NotValid = "Datos de entrada inválidos.";
        public const string InternalError = "Ocurrió un error en el servidor.";
        public const string NameAlreadyExists = "El nombre {0} ya existe en el sistema.";
    }

    public static class Client
    {
        public const string NotFoundUsername = "Cliente no encontrado para el Username: {0}.";
        public const string NotFoundGeneric = "El sistema no tiene cliente asignado.";
        public const string NotValid = "Datos de entrada inválidos para cliente.";
        public const string NotFound = "Cliente no encontrado Id: {0}.";
        public const string Created = "Cliente creado con éxito.";
        public const string Updated = "Cliente actualizado correctamente Id: {0}.";
        public const string Deleted = "Cliente eliminado con éxito.";
        public const string ActionLog = "Cliente Id: {0}, Nombre: {1}";
    }

    public static class Delegados
    {
        public const string NotFoundUsername = "Delegado no encontrado Username: {0}.";
        public const string NotValid = "Datos de entrada inválidos para delegado.";
        public const string NotFound = "Delegado no encontrado Id: {0}.";
        public const string Created = "Delegado creado con éxito Id: {0}.";
        public const string Updated = "Delegado actualizado correctamente Id: {0}.";
        public const string Deleted = "Delegado eliminado con éxito.";
        public const string ActionLog = "Delegado Id: {0}, Nombre: {1}";
    }
    public static class DatalexionUser
    {
        public const string NotValid = "Datos de entrada inválidos para usuario.";
        public const string NotFound = "Usuario no encontrado Id: {0}.";
        public const string Created = "Usuario creado con éxito Id: {0}.";
        public const string Updated = "Usuario actualizado correctamente Id: {0}.";
        public const string Deleted = "Usuario eliminado con éxito.";
        public const string ActionLog = "Usuario Id: {0}, Nombre: {1}";
    }
    public static class DatalexionRole
    {
        public const string NotValid = "Datos de entrada inválidos para rol.";
        public const string NotFound = "Rol no encontrado Id: {0}.";
        public const string Created = "Rol creado con éxito Id: {0}.";
        public const string Updated = "Rol actualizado correctamente Id: {0}.";
        public const string Deleted = "Rol eliminado con éxito.";
        public const string ActionLog = "Rol Id: {0}, Nombre: {1}";
    }
    public static class Candidate
    {
        public const string NotValid = "Datos de entrada inválidos para candidato.";
        public const string NotFound = "Candidato no encontrado Id: {0}.";
        public const string Created = "Candidato creado con éxito Id: {0}.";
        public const string Updated = "Candidato actualizado correctamente Id: {0}.";
        public const string Deleted = "Candidato eliminado con éxito.";
        public const string ActionLog = "Candidato Id: {0}, Nombre: {1}";
    }
    public static class Circuit
    {
        public const string NotValid = "Datos de entrada inválidos para circuito.";
        public const string NotFound = "Circuito no encontrado Id: {0}.";
        public const string Created = "Circuito creado con éxito Id: {0}.";
        public const string Updated = "Circuito actualizado correctamente Id: {0}.";
        public const string Deleted = "Circuito eliminado con éxito.";
        public const string ActionLog = "Circuito Id: {0}, Nombre: {1}";
    }
    public static class Municipality
    {
        public const string NotValid = "Datos de entrada inválidos para municipio.";
        public const string NotFound = "Municipio no encontrado Id: {0}.";
        public const string Created = "Municipio creado con éxito Id: {0}.";
        public const string Updated = "Municipio actualizado correctamente Id: {0}.";
        public const string Deleted = "Municipio eliminado con éxito.";
        public const string ActionLog = "Municipio Id: {0}, Nombre: {1}";
    }
    public static class Participant
    {
        public const string NotValid = "Datos de entrada inválidos para participante.";
        public const string NotFound = "Participante no encontrado Id: {0}.";
        public const string Created = "Participante creado con éxito Id: {0}.";
        public const string Updated = "Participante actualizado correctamente Id: {0}.";
        public const string Deleted = "Participante eliminado con éxito.";
        public const string ActionLog = "Participante Id: {0}, Nombre: {1}";
    }
    public static class Party
    {
        public const string NotValid = "Datos de entrada inválidos para partido.";
        public const string NotFound = "Partido no encontrado Id: {0}.";
        public const string Created = "Partido creado con éxito Id: {0}.";
        public const string Updated = "Partido actualizado correctamente Id: {0}.";
        public const string Deleted = "Partido eliminado con éxito.";
        public const string ActionLog = "Partido Id: {0}, Nombre: {1}";
    }
    public static class Photo
    {
        public const string NotValid = "Datos de entrada inválidos para foto.";
        public const string NotFound = "Foto no encontrada Id: {0}.";
        public const string Created = "Foto creada con éxito Id: {0}.";
        public const string Updated = "Foto actualizada correctamente Id: {0}.";
        public const string Deleted = "Foto eliminada con éxito.";
        public const string ActionLog = "Foto Id: {0}, Nombre: {1}";
    }
    public static class Province
    {
        public const string NotValid = "Datos de entrada inválidos para provincia.";
        public const string NotFound = "Provincia no encontrada Id: {0}.";
        public const string Created = "Provincia creada con éxito Id: {0}.";
        public const string Updated = "Provincia actualizada correctamente Id: {0}.";
        public const string Deleted = "Provincia eliminada con éxito.";
        public const string ActionLog = "Provincia Id: {0}, Nombre: {1}";
    }
    public static class Slate
    {
        public const string NotValid = "Datos de entrada inválidos para lista.";
        public const string NotFound = "Lista no encontrada Id: {0}.";
        public const string Created = "Lista creada con éxito Id: {0}.";
        public const string Updated = "Lista actualizada correctamente Id: {0}.";
        public const string Deleted = "Lista eliminada con éxito.";
        public const string ActionLog = "Lista Id: {0}, Nombre: {1}";
    }
    public static class Wing
    {
        public const string NotValid = "Datos de entrada inválidos para ala.";
        public const string NotFound = "Ala no encontrada Id: {0}.";
        public const string Created = "Ala creada con éxito Id: {0}.";
        public const string Updated = "Ala actualizada correctamente Id: {0}.";
        public const string Deleted = "Ala eliminada con éxito.";
        public const string ActionLog = "Ala Id: {0}, Nombre: {1}";
    }
}
