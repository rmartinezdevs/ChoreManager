namespace ChoreManager.Domain.Entities
{
    public class Chore
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public Guid AssignedUserId { get; set; }

        public Chore() { }

        public Chore(Guid id, string title, string description, DateTime dueDate, string status, Guid assignedUserId)
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
