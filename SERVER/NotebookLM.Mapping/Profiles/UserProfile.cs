using AutoMapper;
using NotebookLM.Application.DTOs.Identity;
using NotebookLM.Domain.Entities;

namespace NotebookLM.Mapping.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterDto, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
    }
}
