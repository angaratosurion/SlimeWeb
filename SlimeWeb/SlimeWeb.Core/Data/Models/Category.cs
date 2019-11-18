using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SlimeWeb.Core.Data.Models
{
    public class Category
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        [Timestamp]
        public Byte[] RowVersion { get; set; }

    }
}
