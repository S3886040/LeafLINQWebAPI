using LeafLINQWebAPI.Model;

namespace LeafLINQWebAPI.DTOs;

public class LoginResponse
{
    public char UserType { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public int Id { get; set; }
    public string SessionID { get; set; }
}
