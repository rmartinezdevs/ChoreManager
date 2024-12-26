using ChoreManager.Domain.Entities;
using ChoreManager.Domain.Interfaces;

namespace ChoreManager.Application.UseCases.ChoreUseCases
{
    public class DeleteChore
    {
        private readonly IChoreRepository _choreRepository;
        public DeleteChore(IChoreRepository choreRepository)
        {
            _choreRepository = choreRepository;
        }

        public async Task ExecuteAsync(Chore chore)
        {
            await _choreRepository.DeleteAsync(chore);
        }
    }
}
