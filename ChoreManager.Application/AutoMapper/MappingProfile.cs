using AutoMapper;
using ChoreManager.Application.DTOs;
using ChoreManager.Domain.Entities;
using System.Xml.Serialization;
using static ChoreManager.Domain.Enums.ChoreEnums;

namespace ChoreManager.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Chore, ChoreDto>();
            CreateMap<CreateChoreDto, Chore>();
        }
    }
}