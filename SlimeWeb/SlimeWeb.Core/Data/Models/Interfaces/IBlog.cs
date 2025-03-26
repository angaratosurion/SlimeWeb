using ExtCore.Data.Entities.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace SlimeWeb.Core.Data.Models.Interfaces
{
    public interface IBlog
    {
        int Id { get; set; }

        [Required]
        [RegularExpression(@"^[^<>:""/\\|?*\x00-\x1F]+$")]
        string Name { get; set; }

        [Required]
        string Title { get; set; }

        [Display(Name = "Last Updated At")]
        [Required]
        DateTime LastUpdate { get; set; }

        [Display(Name = "Created At")]
        [Required]
        DateTime Created { get; set; }

        [Required]
        string Administrator { get; set; }
    }


}
