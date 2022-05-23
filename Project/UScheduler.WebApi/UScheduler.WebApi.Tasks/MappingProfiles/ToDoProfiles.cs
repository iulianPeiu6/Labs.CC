using AutoMapper;
using UScheduler.WebApi.Tasks.Data.Entities;
using UScheduler.WebApi.Tasks.Models.ToDo;

namespace UScheduler.WebApi.Tasks.MappingProfiles
{
    public class ToDoProfiles : Profile
    {
        public ToDoProfiles()
        {
            CreateMap<ToDo, ToDoDto>().ReverseMap();
            CreateMap<CreateToDoModel, ToDo>();
        }
    }
}
