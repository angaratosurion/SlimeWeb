using ExtCore.Data.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SlimeWeb.Core.Data.Models
{
    public class Content : IEntity
    {
    //    [Required]
    //    public int id
    //    {
    //        get; set;
    //    }
    public string engine { get; set; }
    }
}
