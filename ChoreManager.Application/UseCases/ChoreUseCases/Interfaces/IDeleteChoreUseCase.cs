namespace ChoreManager.Application.UseCases.ChoreUseCases.Interfaces
{
    public interface IDeleteChoreUseCase
    {
        Task ExecuteAsync(Guid id);
    }
}