using AutoMapper;
using RecruitApi.Models.DTO;
using RecruitApi.Models;

namespace RecruitApi
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<User,UserDTO>().ReverseMap();
        }
    }
}
