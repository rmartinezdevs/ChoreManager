﻿using AutoMapper;
using ChoreManager.Application.DTOs;
using ChoreManager.Domain.Interfaces;

namespace ChoreManager.Application.UseCases.ChoreUseCases
{
    public class SelectAllChoresUseCase
    {
        private readonly IChoreRepository _choreRepository;
        private readonly IMapper _mapper;

        public SelectAllChoresUseCase(IChoreRepository choreRepository, IMapper mapper)
        {
            _choreRepository = choreRepository;
            _mapper = mapper;
        }

        public async Task<List<ChoreDto>> ExecuteAsync()
        {
            var chores = await _choreRepository.GetAllAsync();
            return _mapper.Map<List<ChoreDto>>(chores);

            //return chores.Select(chore => new ChoreDTO
            //{
            //    Id = chore.Id,
            //    Title = chore.Title,
            //    Description = chore.Description,
            //    DueDate = chore.DueDate,
            //    Status = chore.Status,
            //    AssignedUserId = chore.AssignedUserId
            //}).ToList();
        }
    }
}
