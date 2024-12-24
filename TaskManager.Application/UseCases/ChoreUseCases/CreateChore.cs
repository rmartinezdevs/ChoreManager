using ChoreManager.Domain.Entities;
using ChoreManager.Domain.Interfaces;

namespace ChoreManager.Application.UseCases.ChoreUseCases
{
    public class CreateChore
    {
        private readonly IChoreRepository _choreRepository;

        public CreateChore(IChoreRepository choreRepository)
        {
            _choreRepository = choreRepository;
        }

        public async Task ExecuteAsync(Chore chore)
        {
            if (string.IsNullOrEmpty(chore.Title))
                throw new ArgumentException("El título no puede estar vacío.");

            await _choreRepository.AddAsync(chore);
        }
    }

}
