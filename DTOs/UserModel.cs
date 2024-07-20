using System.ComponentModel.DataAnnotations;

namespace LeafLINQWebAPI.DTOs;

public class UserModel
{
    public int Id { get; set; }
    [Required, StringLength(40), RegularExpression(@"^[a-zA-Z\s'.-]+$", ErrorMessage = "Letters Only Please"), Display(Name = "Full Name")]
    public string FullName { get; set; }
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Enter a Valid Email Please"),
        StringLength(50), Display(Name = "Email"), Required]
    public string Email { get; set; }
    [StringLength(200), Display(Name = "Profile Image"), Required]
    public string PicUrl { get; set; }
    //public string Desc { get; set; }

    public DateTime CurrentLoginDate { get; set; }
    public DateTime LastLoginDate { get; set; }
    [Required, RegularExpression(@"^[AU]$", ErrorMessage = "Must be Either A or U")]
    public char UserType { get; set; }
    public bool Block { get; set; }
    public string NewPassword { get; set; } = string.Empty;


}
