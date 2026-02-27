using AutoMapper;
using GameZone.ViewModels;

namespace Mapper
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<Game, CreateGameFormViewModel>();
            CreateMap<CreateGameFormViewModel, Game>();

            CreateMap<Game, EditGameFormViewModel>();
            CreateMap<EditGameFormViewModel, Game>();
		}
    }
}
