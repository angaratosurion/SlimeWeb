﻿using SlimeWeb.Core.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Text;

namespace SlimeWeb.Core.Data.Models
{
    public class Post:IPost
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public int Id { get; set; }
        //   public int revision { get; set; }
        [Required]
        public string Title { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Published { get; set; }
        //[DataType(DataType.Html)]
        public string content { get; set; }
        [Required]
        public string Author { get; set; }


        //[ConcurrencyCheck]
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        //public string RowVersion { get; set; }
        [Required]
        public int BlogId { get; set; }
        [Required]
        [Key]
        public string PostName { get; set; }


    }
}
