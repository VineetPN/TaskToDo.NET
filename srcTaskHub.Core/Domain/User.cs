using TaskHubCore.Interfaces;

public record User : IEntity<Guid>
{
    public Guid Id { get ; init ; }
    public string Name { get ; init ; }
    public string? FullName { get; init; }
    public DateTime CreatedAt { get; init;}

    public User(Guid guid, string name, string? fullName, DateTime createdAt)
    {
        this.Id = guid;
        this.Name = name ?? throw new ArgumentNullException(nameof(name));
        this.FullName = fullName;
        this.CreatedAt = createdAt;
    }
    public User(string Name, string? FullName){
        this.Id = Guid.NewGuid();
        this.Name = Name ?? throw new ArgumentNullException(nameof(Name));
        this.FullName = FullName;
        this.CreatedAt = DateTime.Now;
    }
}