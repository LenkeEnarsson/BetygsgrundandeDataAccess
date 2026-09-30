using models.CuDto;
using Models;
using Models.DTO;

namespace Services;

public interface IAttractionPropertyService
{
    public Task<ResponsePageDto<ICategory>> ReadCategoryListAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);
    public Task<ResponsePageDto<ICity>> ReadCityListAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);
    public Task<ResponsePageDto<ICountry>> ReadCountryListAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);
}
