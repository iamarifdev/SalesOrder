using AutoMapper;
using FluentValidation;
using SalesOrder.Common.DTO.Window;
using SalesOrder.Common.Exceptions;
using SalesOrder.Common.Models;
using SalesOrder.Common.Validators;
using SalesOrder.Data.Models;
using SalesOrder.Data.Repositories;

namespace SalesOrder.Service;

public interface IWindowService
{
    Task<PaginatedList<WindowDto>> GetWindows(WindowPaginationQuery queryParams);
    Task<WindowDto> GetWindow(int id);
    Task<WindowDto> AddWindow(WindowCreateDto dto);
    Task<WindowDto> UpdateWindow(int id, WindowUpdateDto dto);
    Task<WindowDto> DeleteWindow(int id);
}

public class WindowService : IWindowService
{
    private readonly IMapper _mapper;
    private readonly IWindowRepository _windowRepository;

    public WindowService(IMapper mapper, IWindowRepository windowRepository)
    {
        _windowRepository = windowRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<WindowDto>> GetWindows(WindowPaginationQuery queryParams)
    {
        var windows = await _windowRepository.GetWindows(queryParams);
        return _mapper.Map<PaginatedList<WindowDto>>(windows);
    }

    public async Task<WindowDto> GetWindow(int id)
    {
        var window = await _windowRepository.GetWindow(id);
        if (window == null) throw new WindowNotFoundException();
        return _mapper.Map<WindowDto>(window);
    }

    public async Task<WindowDto> AddWindow(WindowCreateDto dto)
    {
        var validator = new WindowCreateDtoValidator();
        var result = await validator.ValidateAsync(dto);
        if (!result.IsValid) throw new ValidationException(result.Errors);

        var window = _mapper.Map<Window>(dto);
        await _windowRepository.AddWindow(window);
        return _mapper.Map<WindowDto>(window);
    }

    public async Task<WindowDto> UpdateWindow(int id, WindowUpdateDto dto)
    {
        if (id != dto.Id) throw new InvalidWindowIdException();

        var validator = new WindowUpdateDtoValidator();
        var result = await validator.ValidateAsync(dto);
        if (!result.IsValid) throw new ValidationException(result.Errors);

        var window = _mapper.Map<Window>(dto);
        await _windowRepository.UpdateWindow(window);
        return _mapper.Map<WindowDto>(window);
    }

    public async Task<WindowDto> DeleteWindow(int id)
    {
        var window = await _windowRepository.GetWindow(id);
        if (window == null) throw new OrderNotFoundException();

        await _windowRepository.DeleteWindow(id);
        return _mapper.Map<WindowDto>(window);
    }
}