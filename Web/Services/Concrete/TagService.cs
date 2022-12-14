using Core.Entities;
using DataAccess.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Services.Abstract;
using Web.ViewModels.Tag;

namespace Web.Services.Concrete
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryTagRepository _categoryTagRepository;
        private readonly ModelStateDictionary _modelState;

        public TagService(ITagRepository tagRepository,
            ICategoryRepository categoryRepository,
            ICategoryTagRepository categoryTagRepository,
            IActionContextAccessor actionContextAccessor)
        {
            _tagRepository = tagRepository;
            _categoryRepository = categoryRepository;
            _categoryTagRepository = categoryTagRepository;
            _modelState = actionContextAccessor.ActionContext.ModelState;
        }

        public async Task<bool> AddCategoriesAsync(TagAddCategoriesVM model)
        {
            if (!_modelState.IsValid) return false;

            var tag = await _tagRepository.GetAsync(model.TagId);
            if (tag == null)
            {
                _modelState.AddModelError("TagId", "Tag tapılmadı");
                return false;
            }

            foreach (var categoryId in model.CategoriesIds)
            {
                var category = await _categoryRepository.GetAsync(categoryId);
                if (category == null)
                {
                    _modelState.AddModelError(string.Empty, $"{categoryId} id-li kateqoriya tapılmadı");
                    return false;
                }

                var isExist = await _categoryTagRepository.AnyAsync(ct => ct.CategoryId == categoryId && ct.TagId == tag.Id);
                if (isExist)
                {
                    _modelState.AddModelError(string.Empty, $"{categoryId} id-li kateqoriya artıq bu taga əlavə olunub");
                    return false;
                }

                var categoryTag = new CategoryTag
                {
                    TagId = tag.Id,
                    CategoryId = category.Id
                };

                await _categoryTagRepository.CreateAsync(categoryTag);
            }

            return true;
        }

        public async Task<TagAddCategoriesVM> GetTagAddCategoriesModel(int id)
        {
            var tag = await _tagRepository.GetAsync(id);
            if (tag == null) return null;

            var categories = await _categoryRepository.GetAllAsync();

            var model = new TagAddCategoriesVM
            {
                TagId = tag.Id,
                Categories = categories.Select(c => new SelectListItem
                {
                    Text = c.Title,
                    Value = c.Id.ToString()
                })
                .ToList()
            };

            return model;
        }
    }
}
