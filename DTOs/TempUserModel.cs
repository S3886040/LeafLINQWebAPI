using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LeafLINQWebAPI.DTOs;


public class TempUserModel
{

    [Required]
    public int Id { get; set; }

    public int ConfirmationCode { get; set; }

    public string EncryptedCode { get; set; }

}
