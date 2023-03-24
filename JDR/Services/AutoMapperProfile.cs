
using AutoMapper;
using JDR.DTOs;
using JDR.Models;

namespace JDR.Services
{
    public class AutoMapperProfile : Profile 
    {
        public AutoMapperProfile()
        {
            CreateMap<Player, PlayerDTO>();
            CreateMap<PlayerDTO, Player>();
        }
    }
}
