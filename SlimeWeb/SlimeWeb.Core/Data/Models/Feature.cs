using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ExtCore.Data.Entities.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace SlimeWeb.Core.Data.Models
{
    public class Feature : IEntity
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string ControllerName { get; set; }
        public Boolean Disabled { get; set; }
        [Required]
        public int BlogId { get; set; }
    }
}
