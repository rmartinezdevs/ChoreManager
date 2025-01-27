using AutoMapper;
using ChoreManager.Application.DTOs;
using ChoreManager.Application.UseCases.ChoreUseCases.Interfaces;
using ChoreManager.Domain.Interfaces;

namespace ChoreManager.Application.UseCases.ChoreUseCases.Implementations
{
    public class SelectAllChoresUseCase : ISelectAllChoresUseCase
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
        }
    }
}
