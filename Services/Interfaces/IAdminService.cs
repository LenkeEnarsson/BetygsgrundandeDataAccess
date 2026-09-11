namespace Services;

public interface IAdminService
{
    public Task SeedAsync();
    public Task RemoveSeedAsync(bool seeded);
}
