using AutoMapper;
using UScheduler.WebApi.Tasks.Data.Entities;
using UScheduler.WebApi.Tasks.Models.Task;

namespace UScheduler.WebApi.Tasks.MappingProfiles
{
    public class TasksProfile : Profile
    {
        public TasksProfile()
        {
            CreateMap<Task, TaskDto>();
            CreateMap<TaskDto, Task>();
            CreateMap<CreateTaskModel, Task>();
        }
    }
}
