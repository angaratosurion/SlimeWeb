using ExtCore.Data.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SlimeWeb.Core.Data.Models
{
    public class Files : IEntity
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int PostId { get; set; }
        [Required]
       
        public string FileName { get; set; }
        [Required]
        public string Path { get; set; }
        [Required]
        public string RelativePath { get; set; }
        [Timestamp]
        public Byte[] RowVersion { get; set; }

        public string Owner { get; set; }

        [Required]
        public int BlogId { get; set; }

    }
}
