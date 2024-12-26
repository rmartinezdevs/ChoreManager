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

        public async Task ExecuteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("El ID de la tarea no puede ser Guid.Empty.");

            await _choreRepository.DeleteAsync(id);
        }
    }
}
