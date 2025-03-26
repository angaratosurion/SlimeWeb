using ExtCore.Data.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimeWeb.Core.Data.Models.Interfaces;

namespace SlimeWeb.Core.Data.Models
{
    public class SlimeWebPage  : IEntity, ISlimeWebPage
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //   public int revision { get; set; }
        [Required]
    
        public string Title { get; set; }
        [Required]
        //[Key]
        [RegularExpression(@"^[\w\-. ]+$")]
        public string Name { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Published { get; set; }
        //[DataType(DataType.Html)]
        public string content { get; set; }
        public string Author { get; set; }


        //[ConcurrencyCheck]
        //public Byte[] RowVersion { get; set; }
        [Display(Name = "Top")]
        public Boolean TopPosition { get; set; }
        [Display(Name = "Bottom")]
        public Boolean BottomPosition { get; set; }
    }
}
