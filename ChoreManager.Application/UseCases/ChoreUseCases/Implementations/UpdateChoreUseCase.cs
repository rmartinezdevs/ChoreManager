﻿using AutoMapper;
using ChoreManager.Application.DTOs;
using ChoreManager.Application.UseCases.ChoreUseCases.Interfaces;
using ChoreManager.Domain.Interfaces;
using FluentValidation;

namespace ChoreManager.Application.UseCases.ChoreUseCases.Implementations
{
    public class UpdateChoreUseCase : IUpdateChoreUseCase
    {
        private readonly IChoreRepository _choreRepository;
        private readonly IValidator<ChoreDto> _validator;
        private readonly IMapper _mapper;

        public UpdateChoreUseCase(IChoreRepository choreRepository, IValidator<ChoreDto> validator, IMapper mapper)
        {
            _choreRepository = choreRepository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task ExecuteAsync(ChoreDto choreDto)
        {
            var validationResult = await _validator.ValidateAsync(choreDto);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existingChore = await _choreRepository.GetByIdAsync(choreDto.Id);

            if (existingChore == null)
            {
                throw new KeyNotFoundException($"No chore found with ID {choreDto.Id}");
            }

            _mapper.Map(choreDto, existingChore);

            await _choreRepository.UpdateAsync(existingChore);
        }
    }

}
