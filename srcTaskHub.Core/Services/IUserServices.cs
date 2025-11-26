using TaskHubCore.Core.DTO;

namespace TaskHubCore.Core.Services;

public interface IUserService<TKey> where TKey : class{
    Task<TKey> CreateUserAsync(UserCreateDTO taskCreateDTO, CancellationToken cancellationToken = default);
    Task<TKey> GetUserDetailsByID(Guid guid, CancellationToken cancellationToken = default);
    Task<TKey> GetAllUsersAsync(CancellationToken cancellationToken = default);
    Task<TKey?> ModifyUserName(Guid id, string Name, CancellationToken cancellationToken = default);
}