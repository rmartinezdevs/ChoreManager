using static ChoreManager.Domain.Enums.ChoreEnums;

namespace ChoreManager.Domain.Entities
{
    public class Chore
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public ChoreStatus Status { get; set; }
        public Guid AssignedUserId { get; set; }

#pragma warning disable CS8618
        public Chore() { }
#pragma warning restore CS8618 

        public Chore(Guid id, string title, string description, DateTime dueDate, ChoreStatus status, Guid assignedUserId)
        {
            Id = id;
            Title = title;
            Description = description;
            DueDate = dueDate;
            Status = status;
            AssignedUserId = assignedUserId;
        }
    }
}