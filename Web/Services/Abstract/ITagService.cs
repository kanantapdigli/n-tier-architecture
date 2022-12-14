using Web.ViewModels.Tag;

namespace Web.Services.Abstract
{
    public interface ITagService
    {
        Task<TagAddCategoriesVM> GetTagAddCategoriesModel(int id);
        Task<bool> AddCategoriesAsync(TagAddCategoriesVM model);
    }
}
