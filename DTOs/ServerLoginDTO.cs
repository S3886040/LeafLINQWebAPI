using System.ComponentModel.DataAnnotations;

namespace LeafLINQWebAPI.DTOs;

public class ServerLoginDTO
{
    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;
}
