using AutoMapper;
using ChoreManager.Application.DTOs;
using ChoreManager.Domain.Interfaces;

namespace ChoreManager.Application.UseCases.ChoreUseCases
{
    public class SelectChoreByIdUseCase
    {
        private readonly IChoreRepository _choreRepository;
        private readonly IMapper _mapper;


        public SelectChoreByIdUseCase(IChoreRepository choreRepository, IMapper mapper)
        {
            _choreRepository = choreRepository;
            _mapper = mapper;
        }

        public async Task<ChoreDto?> ExecuteAsync(Guid choreId)
        {
            if (choreId == Guid.Empty)
                throw new ArgumentException("El ID de la tarea no puede ser Guid.Empty.");
            var chore = await _choreRepository.GetByIdAsync(choreId);

            return _mapper.Map<ChoreDto>(chore);
        }
    }
}
