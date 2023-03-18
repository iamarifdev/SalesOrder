using SalesOrder.Common.DTO.Element;
using SalesOrder.Common.Models;

namespace SalesOrder.App.Services;

public interface ISubElementService
{
    Task<PaginatedList<SubElementDto>> GetAllSubElementsAsync(PaginationQuery paginationQuery);
    Task<SubElementDto> GetSubElementByIdAsync(int subElementId);
    Task<SubElementDto> AddSubElementAsync(SubElementCreateDto dto);
    Task<SubElementDto> UpdateSubElementAsync(int subElementId, SubElementUpdateDto dto);
    Task<SubElementDto> DeleteSubElementAsync(int subElementId);
}

public class SubElementService : BaseService<SubElementDto, SubElementCreateDto, SubElementUpdateDto>,
    ISubElementService
{
    public SubElementService(HttpClient httpClient) : base(httpClient)
    {
    }

    protected override string EndpointUrl => "SubElements";

    public async Task<PaginatedList<SubElementDto>> GetAllSubElementsAsync(PaginationQuery paginationQuery)
    {
        return await GetAllAsync(paginationQuery);
    }

    public async Task<SubElementDto> GetSubElementByIdAsync(int subElementId)
    {
        return await GetByIdAsync(subElementId);
    }

    public async Task<SubElementDto> AddSubElementAsync(SubElementCreateDto dto)
    {
        return await AddAsync(dto);
    }

    public async Task<SubElementDto> UpdateSubElementAsync(int subElementId, SubElementUpdateDto dto)
    {
        return await UpdateAsync(subElementId, dto);
    }

    public async Task<SubElementDto> DeleteSubElementAsync(int subElementId)
    {
        return await DeleteAsync(subElementId);
    }
}