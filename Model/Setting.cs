using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeafLINQWebAPI.Model;

public class Setting
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Display(Name = "Setting ID")]
    public int id {  get; set; }
    [Display(Name = "Temperature Units"), RegularExpression(@"^[CF]$", ErrorMessage = "Must be either (C)elsius or (F)ahrenheit")]
    public char TemperatureUnit { get; set; }
    [Display(Name = "Phone Preference")]
    public bool PhonePreference { get; set; } = false;
    [Display(Name = "Email Preference")]
    public bool EmailPreference { get; set; } = true;

    [ForeignKey(nameof(User)), Display(Name = "User ID")]
    public int UserId { get; set; }

    public virtual User User { get; set; }
}
