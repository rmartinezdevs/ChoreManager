using AutoMapper;
using ChoreManager.Application.DTOs;
using ChoreManager.Domain.Entities;

namespace ChoreManager.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Chore, ChoreDto>();
            CreateMap<ChoreDto, Chore>();
            CreateMap<CreateChoreDto, Chore>();
        }
    }
}