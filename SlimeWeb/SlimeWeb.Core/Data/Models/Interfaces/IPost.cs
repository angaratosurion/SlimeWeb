using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SlimeWeb.Core.Data.Models.Interfaces
{
     

    public interface IPost
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        int Id { get; set; }

        string Title { get; set; }

        [DataType(DataType.DateTime)]
        DateTime Published { get; set; }

        string content { get; set; }

        string Author { get; set; }

        [Required]
        int BlogId { get; set; }
    }

}
