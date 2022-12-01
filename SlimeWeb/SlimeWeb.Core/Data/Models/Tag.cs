using ExtCore.Data.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SlimeWeb.Core.Data.Models
{
    public class Tag : IEntity
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        [Timestamp]
        public Byte[] RowVersion { get; set; }
        [Required]
        public int BlogId { get; set; }
        [Required]
        [Key]
        public string BlogAndTag { get; set; }
        
        
    }
}
