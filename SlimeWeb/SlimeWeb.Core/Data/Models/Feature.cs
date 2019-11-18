using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace SlimeWeb.Core.Data.Models
{
    public class Feature
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public Controller Controller { get; set; }
        public Boolean Disabled { get; set; }
    }
}
