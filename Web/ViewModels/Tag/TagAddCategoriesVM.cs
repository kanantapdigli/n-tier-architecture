using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.ViewModels.Tag
{
    public class TagAddCategoriesVM
    {
        public int TagId { get; set; }

        public List<int> CategoriesIds { get; set; }

        public List<SelectListItem>? Categories { get; set; }
    }
}
