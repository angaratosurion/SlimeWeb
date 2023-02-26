using ExtCore.Data.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SlimeWeb.Core.Data.Models
{
    public class GeneralSettings : IEntity
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Web Site Name")]
        public string WebSiteName { get; set; }
        
        public int ItemsPerPage { get; set; }
        public Boolean FeatureManagment { get; set; }
        public String OrderBy { get; set; }
        public String Direction { get; set;}
        
    }
}
