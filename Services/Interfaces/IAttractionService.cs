using models.CuDto;
using Models;
using Models.DTO;

namespace Services;

public interface IAttractionService
{
    public Task<ResponsePageDto<IAttraction>> ReadAttractionListAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);
    public Task<ResponseItemDto<IAttraction>> ReadAttractionAsync(Guid id, bool flat);
    public Task<ResponseItemDto<IAttraction>> DeleteAttractionAsync(Guid id);
    public Task<ResponseItemDto<IAttraction>> UpdateAttractionAsync(AttractionCuDto item);
    public Task<ResponseItemDto<IAttraction>> CreateAttractionAsync(AttractionCuDto item);
}
