using AutoMapper;
using Core.BindingModels;
using Core.Dto;
using Core.Models;
using Core.Utils;
using Data.Models;

namespace Core.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateUserBindingModel, UserDto>();
            CreateMap<CreateUserBindingModel, UserWithPasswordDto>();
            CreateMap<UserWithPasswordDto, User>();
            CreateMap<UpdateUserBindingModel, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>();

            CreateMap<PermissionBindingModel, PermissionDto>();
            CreateMap<PermissionDto, Permission>();
            CreateMap<Permission, PermissionDto>();

            CreateMap<PaginatedList<UserDto>, PaginatedResponse<UserDto>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.HasNextPage, opt => opt.MapFrom(src => src.HasNextPage))
                .ForMember(dest => dest.CurrentPage, opt => opt.MapFrom(src => src.PageIndex));
        }
    }
}
