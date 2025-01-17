using ChoreManager.Application.DTOs;

namespace ChoreManager.Application.UseCases.ChoreUseCases.Interfaces
{
    public interface ISelectChoreByIdUseCase
    {
        Task<ChoreDto> ExecuteAsync(Guid id);
    }
}
