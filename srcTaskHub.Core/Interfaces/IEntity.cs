namespace TaskHubCore.Interfaces;

public interface IEntity<TKey>{
    TKey Id { get; init; }
}