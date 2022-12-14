using Core.Entities;
using DataAccess.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Services.Abstract;
using Web.ViewModels.Category;

namespace Web.Services.Concrete
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ICategoryTagRepository _categoryTagRepository;
        private readonly ModelStateDictionary _modelState;

        public CategoryService(ICategoryRepository categoryRepository,
            ITagRepository tagRepository,
            ICategoryTagRepository categoryTagRepository,
            IActionContextAccessor actionContextAccessor)
        {
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
            _categoryTagRepository = categoryTagRepository;
            _modelState = actionContextAccessor.ActionContext.ModelState;
        }

        public async Task<CategoryIndexVM> GetAllAsync()
        {
            var model = new CategoryIndexVM
            {
                Categories = await _categoryRepository.GetAllAsync()
            };

            return model;
        }

        public async Task<CategoryUpdateVM> GetUpdateModelAsync(int id)
        {
            var category = await _categoryRepository.GetAsync(id);
            if (category == null) return null;

            var model = new CategoryUpdateVM
            {
                Id = category.Id,
                Title = category.Title
            };

            return model;
        }

        public async Task<bool> CreateAsync(CategoryCreateVM model)
        {
            if (!_modelState.IsValid) return false;

            var isExist = await _categoryRepository.AnyAsync(c => c.Title.Trim().ToLower() == model.Title.Trim().ToLower());
            if (isExist)
            {
                _modelState.AddModelError("Title", "Bu adda kateqoriya mövcuddur");
                return false;
            }

            var category = new Category
            {
                Title = model.Title,
                CreatedAt = DateTime.Now
            };

            await _categoryRepository.CreateAsync(category);
            return true;
        }

        public async Task<CategoryDetailsVM> GetDetailsModelAsync(int id)
        {
            var category = await _categoryRepository.GetWithTagsAsync(id);
            if (category == null) return null;

            var model = new CategoryDetailsVM
            {
                Category = category
            };

            return model;
        }

        public async Task<bool> UpdateAsync(CategoryUpdateVM model)
        {
            if (!_modelState.IsValid) return false;

            var category = await _categoryRepository.GetAsync(model.Id);
            if (category != null)
            {
                category.Title = model.Title;
                category.ModifiedAt = DateTime.Now;

                await _categoryRepository.UpdateAsync(category);
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetAsync(id);
            if (category != null)
            {
                await _categoryRepository.DeleteAsync(category);
                return true;
            }

            return false;
        }

        public async Task<bool> AddTagsAsync(CategoryAddTagsVM model)
        {
            if (!_modelState.IsValid) return false;

            var category = await _categoryRepository.GetAsync(model.CategoryId);
            if (category == null)
            {
                _modelState.AddModelError("CategoryId", "Kateqoriya tapılmadı");
                return false;
            }

            foreach (var tagId in model.TagsIds)
            {
                var tag = await _tagRepository.GetAsync(tagId);
                if (tag == null)
                {
                    _modelState.AddModelError(string.Empty, $"{tagId} id-li tag tapılmadı");
                    return false;
                }

                var isExist = await _categoryTagRepository.AnyAsync(ct => ct.CategoryId == category.Id && ct.TagId == tagId);
                if (isExist)
                {
                    _modelState.AddModelError(string.Empty, $"{tag.Id} id-li tag artıq bu kateqoriyaya əlavə olunub");
                    return false;
                }

                var categoryTag = new CategoryTag
                {
                    CategoryId = category.Id,
                    TagId = tag.Id
                };

                await _categoryTagRepository.CreateAsync(categoryTag);
            }

            return true;
        }

        public async Task<CategoryAddTagsVM> GetAddTagsModelAsync(int id)
        {
            var category = await _categoryRepository.GetAsync(id);
            if (category == null) return null;

            var tags = await _tagRepository.GetAllAsync();

            var model = new CategoryAddTagsVM
            {
                Tags = tags.Select(t => new SelectListItem
                {
                    Text = t.Title,
                    Value = t.Id.ToString()
                })
                .ToList()
            };

            return model;
        }
    }
}
