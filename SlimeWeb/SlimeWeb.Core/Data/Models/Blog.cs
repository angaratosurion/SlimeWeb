﻿using ExtCore.Data.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SlimeWeb.Core.Data.Models
{
  public  class Blog : IEntity
    {
        [Required]
        public int Id { get; set; }
        [Required]        
        public string Name  { get; set; }
        //[Required]
        // public string AuthorId { get; set; }
        [Required]
        public string Title { get; set; }
        [Display(Name="Last Updated At")]
        [Required]
        public DateTime LastUpdate { get; set; }
        [Display(Name = "Created  At")]
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public string Administrator { get; set; }
        public string engine { get; set; }
       
        
    }

}

