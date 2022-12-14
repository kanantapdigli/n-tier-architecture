using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Web.Services.Abstract;
using Web.ViewModels.Category;

namespace Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await _categoryService.GetAllAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateVM model)
        {
            var isSucceeded = await _categoryService.CreateAsync(model);
            if (isSucceeded) return RedirectToAction(nameof(Index));

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _categoryService.GetDetailsModelAsync(id);
            if (model == null) return NotFound();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var model = await _categoryService.GetUpdateModelAsync(id);
            if (model == null) return NotFound();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, CategoryUpdateVM model)
        {
            if (id != model.Id) return BadRequest();

            var isSucceeded = await _categoryService.UpdateAsync(model);
            if (!isSucceeded) return View(model);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var isSucceeded = await _categoryService.DeleteAsync(id);
            if (isSucceeded) return RedirectToAction(nameof(Index));

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> AddTags(int id)
        {
            var model = await _categoryService.GetAddTagsModelAsync(id);
            if (model == null) return NotFound();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddTags(int id, CategoryAddTagsVM model)
        {
            if (id != model.CategoryId) return BadRequest();
            var isSucceeded = await _categoryService.AddTagsAsync(model);
            if (isSucceeded) return RedirectToAction(nameof(Index));

            model = await _categoryService.GetAddTagsModelAsync(model.CategoryId);
            return View(model);
        }
    }
}
