using SalesOrder.Common.DTO.Window;
using SalesOrder.Common.Models;

namespace SalesOrder.App.Services;

public interface IWindowService
{
    Task<PaginatedList<WindowDto>> GetAllWindowsAsync(PaginationQuery paginationQuery);
    Task<WindowDto> GetWindowByIdAsync(int windowId);
    Task<WindowDto> AddWindowAsync(WindowCreateDto dto);
    Task<WindowDto> UpdateWindowAsync(int windowId, WindowUpdateDto dto);
    Task<WindowDto> DeleteWindowAsync(int windowId);
}

public class WindowService : BaseService<WindowDto, WindowCreateDto, WindowUpdateDto>, IWindowService
{
    public WindowService(HttpClient httpClient) : base(httpClient)
    {
    }

    protected override string EndpointUrl => "Windows";

    public async Task<PaginatedList<WindowDto>> GetAllWindowsAsync(PaginationQuery paginationQuery)
    {
        return await GetAllAsync(paginationQuery);
    }

    public async Task<WindowDto> GetWindowByIdAsync(int windowId)
    {
        return await GetByIdAsync(windowId);
    }

    public async Task<WindowDto> AddWindowAsync(WindowCreateDto dto)
    {
        return await AddAsync(dto);
    }

    public async Task<WindowDto> UpdateWindowAsync(int windowId, WindowUpdateDto dto)
    {
        return await UpdateAsync(windowId, dto);
    }

    public async Task<WindowDto> DeleteWindowAsync(int windowId)
    {
        return await DeleteAsync(windowId);
    }
}