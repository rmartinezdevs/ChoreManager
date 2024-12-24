using ChoreManager.Domain.Entities;
using ChoreManager.Domain.Interfaces;

namespace ChoreManager.Application.UseCases.ChoreUseCases
{
    public class SelectChoreById
    {
        private readonly IChoreRepository _choreRepository;

        public SelectChoreById(IChoreRepository choreRepository)
        {
            _choreRepository = choreRepository;
        }

        public async Task<Chore?> ExecuteAsync(Guid choreId)
        {
            if (choreId == Guid.Empty)
                throw new ArgumentException("El ID de la tarea no puede ser Guid.Empty.");

            return await _choreRepository.GetByIdAsync(choreId);
        }
    }
}
