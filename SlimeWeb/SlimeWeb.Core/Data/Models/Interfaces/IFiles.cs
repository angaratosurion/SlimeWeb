using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SlimeWeb.Core.Data.Models.Interfaces
{
    public interface IFiles
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
