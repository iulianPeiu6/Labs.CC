using AutoMapper;
using UScheduler.WebApi.Boards.Data.Entities;
using UScheduler.WebApi.Boards.Models;

namespace UScheduler.WebApi.Boards.MappingProfiles
{
    public class BoardsProfile : Profile
    {
        public BoardsProfile()
        {
            CreateMap<Board, BoardDto>();
            CreateMap<BoardDto, Board>();
            CreateMap<CreateBoardModel, Board>();
        }
    }
}
