using ChoreManager.Domain.Entities;
using ChoreManager.Domain.Interfaces;

namespace ChoreManager.Application.UseCases.ChoreUseCases
{
    public class SelectAllChores
    {
        private readonly IChoreRepository _choreRepository;

        public SelectAllChores(IChoreRepository choreRepository)
        {
            _choreRepository = choreRepository;
        }

        public async Task<List<Chore>> ExecuteAsync()
        {
            return await _choreRepository.GetAllAsync();
        }
    }
}
