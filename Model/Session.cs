using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using SimpleHashing.Net;
using Microsoft.AspNetCore.Identity;

namespace LeafLINQWebAPI.Model;

public class Session
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None), Display(Name = "Session Id") ]
    public string SessionId { get; set; }

    [ForeignKey(nameof(Plant.User)), Display(Name = "User Id")]
    public int UserId { get; set; }
    [StringLength(200)]
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }


}
