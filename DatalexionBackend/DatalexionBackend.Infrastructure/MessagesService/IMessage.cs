namespace DatalexionBackend.Infrastructure.MessagesService
{
    public interface IMessage<T> where T : class
    {
        string ClientNotFound(int clientId);
        string NotFoundGeneric();
        string InternalError();
        string NameAlreadyExists(string name);
        string NotFound(int id);
        string NotValid();
        string Created(int id, string name = "");
        string Updated(int id, string name = "");
        string Deleted(int id, string name = "");
        string ActionLog(int id, string name);
    }
}