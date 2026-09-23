using models.CuDto;
using Models;
using Models.DTO;

namespace Services;

public interface IAdminService
{
    public Task SeedAsync();
    public Task RemoveSeedAsync(bool seeded);

    public Task<GstUsrInfoDbDto> GuestDbInfoAsync();
    public Task<ResponseItemDto<IUser>> ReadUserAsync(Guid id, bool flat);
    public Task<ResponseItemDto<IUser>> CreateUserAsync(UserCuDto item);
    public Task<ResponseItemDto<IUser>> UpdateUserAsync(UserCuDto item);
    public Task<ResponseItemDto<IUser>> DeleteUserAsync(Guid id);
}
