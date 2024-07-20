using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SimpleHashing.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace LeafLINQWebAPI.Model;

[Index(nameof(User.Email), IsUnique = true)]
public class User
{
    
    public User()
    {
        hasher = new SimpleHash();
    }

    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity),Display(Name = "User Id")]
    public int Id { get; set; }   

    [Required, StringLength(40), RegularExpression(@"^[a-zA-Z\s'.-]+$", ErrorMessage = "Letters Only Please"), Display(Name = "Full Name")]
    public string FullName { get; set; }
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Enter a Valid Email Please"),
        StringLength(50), Display(Name = "Email"),Required]
    public string Email { get; set; }
    [StringLength(200), Display(Name = "Profile Image"),Required]
    public string PicUrl { get; set; }

    [Display(Name = "Curr Login Date"),Required]
    public DateTime CurrentLoginDate { get; set; } = DateTime.UtcNow;
    [Display(Name = "Last Login Date"),Required]
    public DateTime LastLoginDate { get; set; } = DateTime.UtcNow;

    [RegularExpression(@"^[UAS]$", ErrorMessage = "Must be (U)ser or (A)ccount manager"), Display(Name = "Type")]
    public char UserType { get; set; }
    [Display(Name = "Password Hash"), StringLength(94),Required]
    public string PasswordHash { get; set; }
    public bool Block { get; set; } = false;

    public virtual List<Plant> Plants { get; set; }
    public virtual Session Session { get; set; }

    private SimpleHash hasher;
    public bool VerifyPassword(string password)
    {
        return hasher.Verify(password, PasswordHash);
    }

    public string HashPassword(string password)
    {
        return hasher.Compute(password, 50000);
    }

    public string getLocalDateTimeCurrentLogin() 
    {
        return CurrentLoginDate.ToLocalTime().ToString();
    }

    public string getLocalDateTimeLastLogin()
    {
        return LastLoginDate.ToLocalTime().ToString();
    }
    
}
