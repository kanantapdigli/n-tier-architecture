using Microsoft.AspNetCore.Mvc;
using Web.Services.Abstract;
using Web.ViewModels.Tag;

namespace Web.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddCategories(int id)
        {
            var model = await _tagService.GetTagAddCategoriesModel(id);
            if (model == null) return NotFound();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategories(int id, TagAddCategoriesVM model)
        {
            if (id != model.TagId) return BadRequest();

            var isSucceeded = await _tagService.AddCategoriesAsync(model);
            if (isSucceeded) return RedirectToAction(nameof(Index));

            model = await _tagService.GetTagAddCategoriesModel(model.TagId);
            return View(model);
        }
    }
}
