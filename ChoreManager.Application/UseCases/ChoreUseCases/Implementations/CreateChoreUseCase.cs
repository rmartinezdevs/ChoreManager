using AutoMapper;
using ChoreManager.Application.DTOs;
using ChoreManager.Application.UseCases.ChoreUseCases.Interfaces;
using ChoreManager.Domain.Entities;
using ChoreManager.Domain.Interfaces;
using FluentValidation;

namespace ChoreManager.Application.UseCases.ChoreUseCases.Implementations
{
    public class CreateChoreUseCase : ICreateChoreUseCase
    {
        private readonly IChoreRepository _choreRepository;
        private readonly IValidator<CreateChoreDto> _validator;
        private readonly IMapper _mapper;

        public CreateChoreUseCase(IChoreRepository choreRepository, IValidator<CreateChoreDto> validator, IMapper mapper)
        {
            _choreRepository = choreRepository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task ExecuteAsync(CreateChoreDto createChoreDto)
        {
            var validationResult = await _validator.ValidateAsync(createChoreDto);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var chore = _mapper.Map<Chore>(createChoreDto);

            await _choreRepository.AddAsync(chore);
        }
    }

}
