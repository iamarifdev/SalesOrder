using System.Net.Http.Json;
using SalesOrder.Common.Helpers;
using SalesOrder.Common.Models;

namespace SalesOrder.App.Services;

public abstract class BaseService<TDto, TCreateDto, TUpdateDto>
{
    private readonly HttpClient _httpClient;
    protected abstract string EndpointUrl { get; }

    protected BaseService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    protected async Task<PaginatedList<TDto>> GetAllAsync(PaginationQuery paginationQuery)
    {
        var response = await _httpClient.GetJsonAsync(EndpointUrl, paginationQuery);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ApiPaginatedResponse<TDto>>();
        return result.Result;
    }

    protected async Task<TDto> GetByIdAsync(int id)
    {
        var response = await _httpClient.GetJsonAsync($"{EndpointUrl}/{id}");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<TDto>>();
        return result.Result;
    }

    protected async Task<TDto> AddAsync(TCreateDto dto)
    {
        var response = await _httpClient.PostJsonAsync(EndpointUrl, dto);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<TDto>>();
        return result.Result;
    }

    protected async Task<TDto> UpdateAsync(int id, TUpdateDto dto)
    {
        var response = await _httpClient.PutJsonAsync($"{EndpointUrl}/{id}", dto);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<TDto>>();
        return result.Result;
    }

    protected async Task<TDto> DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{EndpointUrl}/{id}");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<TDto>>();
        return result.Result;
    }
}
