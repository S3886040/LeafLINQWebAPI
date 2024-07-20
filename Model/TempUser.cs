using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeafLINQWebAPI.Model
{
    public class TempUser
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None), Required, Display(Name = "Temp User ID")]
        public int Id { get; set; }

        [Display(Name = "Code")]
        public int ConfirmationCode { get; set; } = 0;

        [StringLength(94)]
        public string EncryptedCode { get; set; }
    }
}
