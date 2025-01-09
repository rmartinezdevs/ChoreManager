using static ChoreManager.Domain.Enums.ChoreEnums;

namespace ChoreManager.Application.DTOs
{
    public class ChoreDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public ChoreStatus Status { get; set; }
        public Guid AssignedUserId { get; set; }

        public ChoreDto()
        {
            Id = Guid.NewGuid();
            Title = string.Empty;
            Description = string.Empty;
            DueDate = DateTime.MinValue;
            AssignedUserId = Guid.Empty;
        }
    }
}
