using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SlimeWeb.Core.Data.Models
{
   public class BlogMods : IEntity
    {
        [Required]
        // [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]

        public virtual Blog Blog{ get; set; }
        [Required]
        //  [Key]
        public string Moderator { get; set; }
    }
}
