using ChoreManager.Domain.Entities;

namespace ChoreManager.Domain.Interfaces
{
    public interface IChoreRepository
    {
        Task AddAsync(Chore chore);
        Task DeleteAsync(Guid id);
        Task<Chore?> GetByIdAsync(Guid id);
        Task<List<Chore>> GetAllAsync();
    }
}
