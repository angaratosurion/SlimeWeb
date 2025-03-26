using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SlimeWeb.Core.Data.Models.Interfaces
{
    public interface ICategory
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        int Id { get; set; }

        string Name { get; set; }

        [Required]
        int BlogId { get; set; }

        [Required]
        string BlogAndCategory { get; set; }
    }

}
