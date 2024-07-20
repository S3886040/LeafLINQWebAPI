using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeafLINQWebAPI.Model
{
    public class Plant
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity),Display(Name = "Plant Id")]
        public int Id { get; set; }

        [Required, Display(Name = "Name"),StringLength(40)]
        public string Name { get; set; }
        [Required, Display(Name = "Desc"),StringLength(200)]
        public string Desc { get; set; }
        [Display(Name = "Plant Image"), Required, StringLength(200)]
        public string PicUrl { get; set; }
        [Display(Name = "Location"), Required, StringLength(10)]
        public string Location { get; set; }
        [Display(Name = "Level"), Required, StringLength(10)]
        public string Level { get; set; }

        public DateTime LastWateredDate { get; set; } = DateTime.UtcNow;
        public HealthCheckStatus HealthCheckStatus { get; set; } = HealthCheckStatus.Healthy;

        [Display(Name = "Plant Node"),StringLength(20)]
        public string DeviceId { get; set; }

        // *** Foreign keys ***
        // UserId to table User, DeviceId to table Device, and PlantGroupId to table PlantGroup
        [ForeignKey(nameof(Plant.User)),Display(Name = "User Id")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public string GetLastWaterDate()
        {
            return LastWateredDate.ToLocalTime().ToString();
        }

    }

    public enum HealthCheckStatus
    {
        Healthy,
        Alert,
        Inactive,
        Critical
    }

    
}
