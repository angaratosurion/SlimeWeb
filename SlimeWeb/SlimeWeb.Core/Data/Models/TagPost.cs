using ExtCore.Data.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Data.Models
{
   public  class TagPost : IEntity
    {
        [Required]
        public int Id { get; set; }

        public int TagId { get; set; }
        public int BlogId { get; set; }
        public int PostId { get; set; }
    }
}
