using static ChoreManager.Domain.Enums.ChoreEnums;

namespace ChoreManager.Application.DTOs
{
    public class CreateChoreDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public ChoreStatus Status { get; set; }
        public Guid AssignedUserId { get; set; }

        public CreateChoreDto()
        {
            Title = string.Empty;
            Description = string.Empty;
            DueDate = DateTime.MinValue;
            AssignedUserId = Guid.Empty;
            Status = ChoreStatus.Pending;
        }
    }
}
