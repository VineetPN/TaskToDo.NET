using TaskHubCore.Interfaces;

namespace TaskHubCore.Core.Tasks;

public record TaskItem : IEntity<Guid>{

    public string Name { get; init; }
    public string? Description { get; init; }
    public DateTime dateTime{ get; init; }
    public bool IsDone { get; set; } = false;
    public Guid Id { get ; init; }

    public TaskItem(string Name, string Description, DateTime dateTime, Guid guid){
        this.Name = Name;
        this.Description = Description;
        this.dateTime = dateTime;
        this.Id = guid;
    } // -> Ctor 1
    public TaskItem(string Name, string Description){
        this.Name = Name;
        this.Description= Description;
        this.dateTime = DateTime.Now;
        this.Id = Guid.NewGuid();
        this.IsDone = false;
    }

    public TaskItem Markdone() => this with {IsDone = true }; // this might cause problem if there are many tasks getting created as it creates a copy of the existing record and then modifies the data
}