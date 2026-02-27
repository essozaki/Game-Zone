using GameZone.Models;
using GameZone.ViewModels;
using GameZone.Settings;
using Settings;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
namespace GameZone.Services;

public class GameService : IGameService
{
    private readonly ApplicationDbContext _context;
    //private readonly IMapper mapper;

    public GameService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Create(CreateGameFormViewModel model)
    {
        var coverName = FileControl.uploadFile("Images/Games", model.Cover);
        Game data = new()
        {
            Name = model.Name,
            Description = model.Description,
            CategoryId = model.CategoryId,
            price = model.price,
            Cover = coverName,
            Device = model.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList()
        };
        _context.Add(data);
        _context.SaveChanges();
    }

    public bool DeleteForEver(int id)
    {
        var isDeleted = false;
        var game=_context.Game.Find(id);
        if (game is null)
        {
            return isDeleted;
        }
        _context.Game.Remove(game);
        var deletedgame = _context.DeleetedGame.Where(x => x.GameId == game.Id).FirstOrDefault();
        _context.Remove(deletedgame);

        var effectedRows = _context.SaveChanges();
        if (effectedRows>0)
        {
            isDeleted = true;
             FileControl.RemoveFile("Images/Games/", game.Cover);
            //File.Delete("~/Images/Games/"+ game.Cover);
        }
        return isDeleted;
    }

    public IEnumerable<Game> GetAll()
    {
        return _context.Game
            .Where(x => x.isDeleted == false)
            .Include(g => g.category)
            //.Include(g => g.DeleetedGame)
            .Include(g => g.Device)
            .ThenInclude(d => d.Device)
            .AsNoTracking()
            .ToList();
    }
    public IEnumerable<Game> GetRecentlyDeleted()
    {
        return _context.Game
            .Where(x => x.isDeleted == true)
            .Include(g => g.category)
            .Include(g => g.Device)
            .ThenInclude(d => d.Device)
            .AsNoTracking()
            .ToList();
    }

    public Game? GetById(int id)
    {
        return _context.Game
            .Include(g => g.category)
            .Include(g => g.Device)
            .ThenInclude(d => d.Device)
            .AsNoTracking()
            .SingleOrDefault(g => g.Id == id);
    }
    public async Task<Game?> Update(EditGameFormViewModel model)
    {
        var game = _context.Game
            .Include(g => g.Device)
            .SingleOrDefault(g => g.Id == model.Id);

        if (game is null)
            return null;

        var hasNewCover = model.Cover is not null;
        var oldCover = game.Cover;

        game.Name = model.Name;
        game.Description = model.Description;
        game.CategoryId = model.CategoryId;
        game.price = model.price;
        game.Device = model.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList();

        if (hasNewCover)
        {
            game.Cover = FileControl.uploadFile("Images/Games/", model.Cover!);
        }


        //var obj = mapper.Map<Game>(model);
        //_context.Entry(obj).State = EntityState.Modified;
        var effectedRows = _context.SaveChanges();

        if (effectedRows > 0)
        {
            if (hasNewCover)
            {
                FileControl.RemoveFile("Images/Games/",oldCover);
            }

            return game;
        }
        else
        {
            FileControl.RemoveFile("Images/Games/", game.Cover);
            return null;
        }
    }



    public bool TemporaryDelete(int id)
    {
        var isTemporaryDeleted = false;
        var game = _context.Game.Find(id);
        if (game is null)
        {
            return isTemporaryDeleted;
        }
        game.isDeleted = true;

        var deleetedgame = new DeleetedGame() {
        GameId =game.Id,
        DeletedDate = DateTime.Now,
        };
        _context.Add(deleetedgame);
        var effectedRows = _context.SaveChanges();
        if (effectedRows > 0)
        {
            
            isTemporaryDeleted = true;
        }
        return isTemporaryDeleted;
    }
    public bool UndoTemporaryDelete(int id)
    {
        var isUndoTemporaryDeleted = false;
        var game = _context.Game.Find(id);
        if (game is null)
        {
            return isUndoTemporaryDeleted;
        }
        game.isDeleted = false;
        var deletedgame = _context.DeleetedGame.Where(x => x.GameId == game.Id).FirstOrDefault();
            _context.Remove(deletedgame);

        var effectedRows = _context.SaveChanges();
        if (effectedRows > 0)
        {
            isUndoTemporaryDeleted = true;
        }
        return isUndoTemporaryDeleted;
    }


}
