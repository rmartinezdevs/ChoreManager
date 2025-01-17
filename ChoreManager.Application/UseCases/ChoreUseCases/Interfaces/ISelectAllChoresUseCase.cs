using ChoreManager.Application.DTOs;

namespace ChoreManager.Application.UseCases.ChoreUseCases.Interfaces
{
    public interface ISelectAllChoresUseCase
    {
        Task<List<ChoreDto>> ExecuteAsync();
    }
}
