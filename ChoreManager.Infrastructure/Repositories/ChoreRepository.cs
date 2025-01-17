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

        public async Task DeleteAsync(Guid choreId)
        {
            try
            {
                var choreToDelete = await _dbContext.Chores.FirstOrDefaultAsync(c => c.Id == choreId);

                if (choreToDelete == null)
                {
                    throw new ArgumentException($"No se encontró ninguna tarea con el ID {choreId}");
                }

                _dbContext.Chores.Remove(choreToDelete);

                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error al intentar eliminar la tarea", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ocurrió un error inesperado al eliminar la tarea", ex);
            }
        }

        public async Task UpdateAsync(Chore chore)
        {
            try
            {
                var choreToUpdate = await _dbContext.Chores.FirstOrDefaultAsync(c => c.Id == chore.Id);

                if (choreToUpdate == null)
                {
                    throw new ArgumentException($"No se encontró ninguna tarea con el ID {chore.Id}");
                }

                choreToUpdate.Title = chore.Title;
                choreToUpdate.Description = chore.Description;
                choreToUpdate.DueDate = chore.DueDate;
                choreToUpdate.Status = chore.Status;
                choreToUpdate.AssignedUserId = chore.AssignedUserId;

                _dbContext.Chores.Update(choreToUpdate);

                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error al intentar eliminar la tarea", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ocurrió un error inesperado al eliminar la tarea", ex);
            }
        }

        public async Task<IEnumerable<Chore>> GetAllAsync()
        {
            return await _dbContext.Chores
                .Select(c => new Chore(c.Id, c.Title, c.Description, c.DueDate, c.Status, c.AssignedUserId))
                .ToListAsync();
        }

        public async Task<Chore?> GetByIdAsync(Guid choreId)
        {
            return await _dbContext.Chores.FirstOrDefaultAsync(chore => chore.Id == choreId);
        }

        #endregion
    }
}
