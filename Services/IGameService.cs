using GameZone.Models;
using GameZone.ViewModels;

namespace GameZone.Services
{
    public interface IGameService
    {        
        Task Create(CreateGameFormViewModel model);
        IEnumerable<Game> GetAll();
        IEnumerable<Game> GetRecentlyDeleted();
        Game? GetById(int id);
        Task<Game?> Update(EditGameFormViewModel model);
        bool DeleteForEver(int id);
        bool TemporaryDelete(int id);
        bool UndoTemporaryDelete(int id);
    }
}
