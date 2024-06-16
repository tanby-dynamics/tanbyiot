using Data;
using Services.Tenants;

namespace Services.Users;

public class UserDto
{
    public Guid Id { get; private init; }
    public string ExternalId { get; init; } = string.Empty;
    public IEnumerable<TenantDto> Tenants { get; private init; } = default!;
    public TenantDto? CurrentTenant { get; init; }
    public string Email { get; init; } = string.Empty;

    public static UserDto FromEntity(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            ExternalId = user.ExternalId,
            Tenants = user.Tenants.Select(x => TenantDto.FromEntity(x)!),
            CurrentTenant = TenantDto.FromEntity(user.CurrentTenant),
            Email = user.Email
        };
    }
}