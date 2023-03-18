using AutoMapper;
using FluentValidation;
using SalesOrder.Common.DTO.Element;
using SalesOrder.Common.Exceptions;
using SalesOrder.Common.Models;
using SalesOrder.Common.Validators;
using SalesOrder.Data.Models;
using SalesOrder.Data.Repositories;

namespace SalesOrder.Service;

public interface ISubElementService
{
    Task<PaginatedList<SubElementDto>> GetSubElements(SubElementPaginationQuery queryParams);
    Task<SubElementDto> GetSubElement(int id);
    Task<SubElementDto> AddSubElement(SubElementCreateDto dto);
    Task<SubElementDto> UpdateSubElement(int id, SubElementUpdateDto dto);
    Task<SubElementDto> DeleteSubElement(int id);
}

public class SubElementService : ISubElementService
{
    private readonly IMapper _mapper;
    private readonly ISubElementRepository _subElementRepository;

    public SubElementService(IMapper mapper, ISubElementRepository subElementRepository)
    {
        _subElementRepository = subElementRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<SubElementDto>> GetSubElements(SubElementPaginationQuery queryParams)
    {
        var subElements = await _subElementRepository.GetSubElements(queryParams);
        return _mapper.Map<PaginatedList<SubElementDto>>(subElements);
    }

    public async Task<SubElementDto> GetSubElement(int id)
    {
        var subElement = await _subElementRepository.GetSubElement(id);
        if (subElement == null) throw new SubElementNotFoundException();
        return _mapper.Map<SubElementDto>(subElement);
    }

    public async Task<SubElementDto> AddSubElement(SubElementCreateDto dto)
    {
        var validator = new SubElementCreateDtoValidator();
        var result = await validator.ValidateAsync(dto);
        if (!result.IsValid) throw new ValidationException(result.Errors);

        var order = _mapper.Map<SubElement>(dto);
        await _subElementRepository.AddSubElement(order);
        return _mapper.Map<SubElementDto>(order);
    }

    public async Task<SubElementDto> UpdateSubElement(int id, SubElementUpdateDto dto)
    {
        if (id != dto.Id) throw new InvalidSubElementIdException();

        var validator = new SubElementUpdateDtoValidator();
        var result = await validator.ValidateAsync(dto);
        if (!result.IsValid) throw new ValidationException(result.Errors);

        var subElement = _mapper.Map<SubElement>(dto);
        await _subElementRepository.UpdateSubElement(subElement);
        return _mapper.Map<SubElementDto>(subElement);
    }

    public async Task<SubElementDto> DeleteSubElement(int id)
    {
        var order = await _subElementRepository.GetSubElement(id);
        if (order == null) throw new OrderNotFoundException();

        await _subElementRepository.DeleteSubElement(id);
        return _mapper.Map<SubElementDto>(order);
    }
}