using models.CuDto;
using Models;
using Models.DTO;

namespace Services;

public interface ICityService
{
    public Task<ResponsePageDto<ICity>> ReadCityListAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);
    public Task<ResponseItemDto<ICity>> ReadCityAsync(Guid id, bool flat);
    public Task<ResponseItemDto<ICity>> DeleteCityAsync(Guid id);
    public Task<ResponseItemDto<ICity>> UpdateCityAsync(CityCuDto item);
    public Task<ResponseItemDto<ICity>> CreateCityAsync(CityCuDto item);
}
