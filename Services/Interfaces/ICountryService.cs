using models.CuDto;
using Models;
using Models.DTO;

namespace Services;

public interface ICountryService
{
    public Task<ResponsePageDto<ICountry>> ReadCountryListAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);
    public Task<ResponseItemDto<ICountry>> ReadCountryAsync(Guid id, bool flat);
    public Task<ResponseItemDto<ICountry>> DeleteCountryAsync(Guid id);
    public Task<ResponseItemDto<ICountry>> UpdateCountryAsync(CountryCuDto item);
    public Task<ResponseItemDto<ICountry>> CreateCountryAsync(CountryCuDto item);
}
