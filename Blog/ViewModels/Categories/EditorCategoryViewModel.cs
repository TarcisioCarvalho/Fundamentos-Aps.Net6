using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Categories
{
    public class EditorCategoryViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Slug { get; set; }
    }
}