using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtCore.Data.Entities.Abstractions;

namespace SlimeWeb.Core.Data.Models
{
    public class FilesPages : IEntity
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int PageId { get; set; }
        [Required]
        public int FileId { get; set; }
        
    }
}
