using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Text;

namespace SlimeWeb.Core.Data.Models
{
    public class Post
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public int Id { get; set; }
        //   public int revision { get; set; }
        public string Title { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Published { get; set; }
        //[DataType(DataType.Html)]
        public string content { get; set; }
        public string Author { get; set; }


        //[ConcurrencyCheck]
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        //public string RowVersion { get; set; }
        [Required]
        public int BlogId { get; set; }
       

    }
}
