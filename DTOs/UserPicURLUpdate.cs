using System.ComponentModel.DataAnnotations;

namespace LeafLINQWebAPI.DTOs;

public class UserPicURLUpdate
{
    public int Id { get; set; }
    [StringLength(200), Display(Name = "Profile Image"), Required]
    public string PicUrl { get; set; }
}
