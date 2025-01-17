using ChoreManager.Application.DTOs;

namespace ChoreManager.Application.UseCases.ChoreUseCases.Interfaces
{
    public interface ICreateChoreUseCase
    {
        Task ExecuteAsync(CreateChoreDto dto);
    }
}