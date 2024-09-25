using ExtCore.Data.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SlimeWeb.Core.Data.Models
{
    public class BannedUsers : IEntity
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string User { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        [Required]
        public string BannedBy { get; set; }
        [ConcurrencyCheck]
        public Byte[] RowVersion { get; set; }

    }
}
