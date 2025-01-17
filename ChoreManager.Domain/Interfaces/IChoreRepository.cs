using ChoreManager.Domain.Entities;

namespace ChoreManager.Domain.Interfaces
{
    public interface IChoreRepository
    {
        Task AddAsync(Chore chore);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(Chore chore);
        Task<Chore?> GetByIdAsync(Guid id);
        Task<IEnumerable<Chore>> GetAllAsync();
    }
}
