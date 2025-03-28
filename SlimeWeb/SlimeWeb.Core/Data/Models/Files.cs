﻿using ExtCore.Data.Entities.Abstractions;
using SlimeWeb.Core.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SlimeWeb.Core.Data.Models
{
    public class Files : IEntity, IFiles
    {

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
       
        public string FileName { get; set; }
        [Required]
        public string Path { get; set; }
        [Required]
        public string RelativePath { get; set; }
        //[ConcurrencyCheck]
        //public Byte[] RowVersion { get; set; }
        [Required]
        public string ContentType { get; set; }

        public string Owner { get; set; }

        

    }
}
