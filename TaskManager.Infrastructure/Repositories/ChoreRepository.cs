using ChoreManager.Domain.Entities;
using ChoreManager.Domain.Interfaces;
using ChoreManager.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ChoreManager.Infrastructure.Repositories
{
    public class ChoreRepository : IChoreRepository
    {
        #region Variables

        private readonly AppDbContext _dbContext;

        #endregion

        #region Constructor

        public ChoreRepository (AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region Methods

        public async Task AddAsync(Chore chore)
        {
            var existChore = await _dbContext.Chores.FirstOrDefaultAsync(c => c.Id == chore.Id);

            if(existChore != null) {
                return;
            }

            _dbContext.Chores.Add(chore);

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Chore chore)
        {
            _dbContext.Chores.Remove(chore);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Chore>> GetAllAsync()
        {
            return await _dbContext.Chores
                .Select(c => new Chore(c.Id, c.Title, c.Description, c.DueDate, c.Status, c.AssignedUserId))
                .ToListAsync();
        }

        public async Task<Chore?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Chores.FirstOrDefaultAsync(chore => chore.Id == id);
        }

        #endregion
    }
}
