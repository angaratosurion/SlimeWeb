﻿using ExtCore.Data.Entities.Abstractions;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SlimeWeb.Core.Data.Models
{
  public  class Blog : IEntity
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        // [RegularExpression(@"^[\w\-. ]+$")]
        [RegularExpression(@"^[^<>:""/\\|?*\x00-\x1F]+$")]
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
      
       
        
    }

}

