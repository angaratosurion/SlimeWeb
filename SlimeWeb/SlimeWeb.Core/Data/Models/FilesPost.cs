using ExtCore.Data.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Data.Models
{
   public class FilesPost : IEntity
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int PostId { get; set; }
        [Required]
        public int FileId { get; set; }
        [Required]
        public int BlogId { get; set; }


    }
}
