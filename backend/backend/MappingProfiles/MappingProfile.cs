using AutoMapper;
using backend.Models;
using Data.Models;

namespace backend.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserResponse>();
            CreateMap<User, UserRequest>();

            CreateMap<UserResponse, User>();
            CreateMap<UserRequest, User>();
        }
    }
}
