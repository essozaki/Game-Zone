using GameZone.Services;
using GameZone.ViewModels;
using GameZone.Settings;
using Microsoft.AspNetCore.Mvc;

namespace GameZone.Controllers
{
    public class GameController : Controller
    {
        private readonly ICategoriesService _categoriesservices;
        private readonly IDevicesService _devicesservices;
        private readonly IGameService _gameservices;

        public GameController(ICategoriesService categories, IDevicesService devices, 
               IGameService gameservices)
        {
            _categoriesservices = categories;
            _devicesservices = devices;
            _gameservices = gameservices;
        }
        public IActionResult Index()
        {
            var games = _gameservices.GetAll();
            return View(games);
        }       

        [HttpGet]
        public IActionResult Create()
        {
            CreateGameFormViewModel model = new()
            {
                Categories = _categoriesservices.GetSelectList(),
                Devices = _devicesservices.GetSelectList()
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGameFormViewModel model)
        {
            if (!ModelState.IsValid) {
                model.Categories = _categoriesservices.GetSelectList();
                model.Devices = _devicesservices.GetSelectList();
                return View(model);
            }
            await _gameservices.Create(model);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Details(int id)
        {
            var game = _gameservices.GetById(id);

            if (game is null)
                return NotFound();

            return View(game);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var game = _gameservices.GetById(id);

            if (game is null)
                return NotFound();

            EditGameFormViewModel viewModel = new()
            {
                Id = id,
                Name = game.Name,
                Description = game.Description,
                CategoryId = game.CategoryId,
                price = game.price,
                SelectedDevices = game.Device.Select(d => d.DeviceId).ToList(),
                Categories = _categoriesservices.GetSelectList(),
                Devices = _devicesservices.GetSelectList(),
                CurrentCover = game.Cover
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditGameFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _categoriesservices.GetSelectList();
                model.Devices = _devicesservices.GetSelectList();
                return View(model);
            }

            var game = await _gameservices.Update(model);

            if (game is null)
                return BadRequest();

            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var isDeleted = _gameservices.DeleteForEver(id);

            return isDeleted ? Ok() : BadRequest();
        }
        
        //temporary  deleted after 30 days

        public IActionResult TemporaryDelete(int id)
        {

            var isTemporaryDeleted = _gameservices.TemporaryDelete(id);

            return isTemporaryDeleted ? Ok() : BadRequest();
        }
        //undo deleted game
        public IActionResult UndoTemporaryDelete(int id)
        {

            var isUndoTemporaryDeleted = _gameservices.UndoTemporaryDelete(id);

            return isUndoTemporaryDeleted ? Ok() : BadRequest();
        }
        //recently deleted 
        public IActionResult RecentlyDeleted()
        {
            var games = _gameservices.GetRecentlyDeleted();
            return View(games);
        }
        //Delete For Ever
        public IActionResult CompeletlDelete(int id)
        {

            var isTemporaryDeleted = _gameservices.DeleteForEver(id);

            return isTemporaryDeleted ? Ok() : BadRequest();
        }

    }
}