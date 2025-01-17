using ChoreManager.Application.DTOs;

namespace ChoreManager.Application.UseCases.ChoreUseCases.Interfaces
{
    public interface IUpdateChoreUseCase
    {
        Task ExecuteAsync(ChoreDto updateChoreDto);
    }
}