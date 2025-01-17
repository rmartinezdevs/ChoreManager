using ChoreManager.Application.UseCases.ChoreUseCases.Interfaces;
using ChoreManager.Domain.Interfaces;

namespace ChoreManager.Application.UseCases.ChoreUseCases.Implementations
{
    public class DeleteChoreUseCase : IDeleteChoreUseCase
    {
        private readonly IChoreRepository _choreRepository;
        public DeleteChoreUseCase(IChoreRepository choreRepository)
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
