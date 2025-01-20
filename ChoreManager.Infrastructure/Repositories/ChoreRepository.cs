using AutoMapper;
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
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public ChoreRepository(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        public async Task AddAsync(Chore chore)
        {
            var existChore = await FindChoreByIdAsync(chore.Id);

            if (existChore != null)
            {
                throw new InvalidOperationException($"A chore with ID {chore.Id} already exists.");
            }

            _dbContext.Chores.Add(chore);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid choreId)
        {
            try
            {
                var choreToDelete = await FindChoreByIdAsync(choreId);

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
        }

        public async Task UpdateAsync(Chore chore)
        {
            try
            {
                var choreToUpdate = await FindChoreByIdAsync(chore.Id);

                if (choreToUpdate == null)
                {
                    throw new ArgumentException($"No se encontró ninguna tarea con el ID {chore.Id}");
                }

                _mapper.Map(chore, choreToUpdate);

                _dbContext.Chores.Update(choreToUpdate);

                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error al intentar actualizar la tarea", ex);
            }
        }

        public async Task<IEnumerable<Chore>> GetAllAsync()
        {
            return await _dbContext.Chores.ToListAsync();
        }

        public async Task<Chore?> GetByIdAsync(Guid choreId)
        {
            var chore = await FindChoreByIdAsync(choreId);

            if (chore == null)
            {
                throw new ArgumentException($"No se encontró ninguna tarea con el ID {choreId}");
            }
            return chore;
        }

        #endregion

        #region Private Methods

        private async Task<Chore?> FindChoreByIdAsync(Guid id)
        {
            return await _dbContext.Chores.FirstOrDefaultAsync(c => c.Id == id);
        }

        #endregion
    }
}
