using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SlimeWeb.Core.Data.Models
{
  public  class News
    {
        [Required]
        public int Id { get; set; }
        //   public int revision { get; set; }
        public string Title { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Published { get; set; }
        [DataType(DataType.Html)]
        public string content { get; set; }
        public string Author { get; set; }

        public virtual List<Category> Categories { get; set; }
        public virtual List<Tag> Tags { get; set; }
        [Timestamp]
        public Byte[] RowVersion { get; set; }
    }
}
