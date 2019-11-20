using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SlimeWeb.Core.Data.Models
{
  public  class Blog
    {
        [Required]
        public int Id { get; set; }
        [Required]        
        public string Name  { get; set; }
        [Required]
         public int AuthorId { get; set; }
        [Required]
        public string Title { get; set; }
        [Display(Name="Last Update At")]
        [Required]
        public DateTime LastUpdate { get; set; }
        [Display(Name = "Created Update At")]
        [Required]
        public DateTime Created { get; set; }

    }
}
