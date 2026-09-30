using models.CuDto;
using Models;
using Models.DTO;

namespace Services;

public interface IAdminService
{
    public Task SeedAsync();
    public Task<CountRowsInTablesDbDto> RemoveSeedAsync(bool seeded);

    public Task<CountRowsInTablesDbDto> GuestDbInfoAsync();

}
