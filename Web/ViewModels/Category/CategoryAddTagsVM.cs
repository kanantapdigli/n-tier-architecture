using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.ViewModels.Category
{
    public class CategoryAddTagsVM
    {
        public int CategoryId { get; set; }
        public List<int> TagsIds { get; set; }
        public List<SelectListItem>? Tags { get; set; }
    }
}
