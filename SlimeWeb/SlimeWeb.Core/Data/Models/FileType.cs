﻿using ExtCore.Data.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SlimeWeb.Core.Data.Models
{
    public class FileType : IEntity
    {
        [System.ComponentModel.DataAnnotations.Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Extention { get; set; }
        [Timestamp]
        public Byte[] RowVersion { get; set; }
    }
}
