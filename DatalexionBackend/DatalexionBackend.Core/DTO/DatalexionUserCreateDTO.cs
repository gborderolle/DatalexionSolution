namespace DatalexionBackend.Core.DTO;
public class DatalexionUserCreateDTO
{
    public string Username { get; set; }
    public required string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string UserRoleId { get; set; }
    public string UserRoleName { get; set; }
    public int ClientId { get; set; }

}
