using AutoMapper;
using UScheduler.WebApi.Users.Data.Entities;
using UScheduler.WebApi.Users.Models;

namespace UScheduler.WebApi.Users.MappingProfiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<User, DisplayUserModel>();
            CreateMap<CreateUserModel, User>();
            CreateMap<UpdateUserModel, User>();
            CreateMap<AccountSettings, AccountSettings>();
        }
    }
}
