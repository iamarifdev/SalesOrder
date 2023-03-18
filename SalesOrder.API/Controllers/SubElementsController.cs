using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SalesOrder.API.Helpers;
using SalesOrder.Common.DTO.Element;
using SalesOrder.Common.Models;
using SalesOrder.Service;

namespace SalesOrder.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubElementsController : ControllerBase
{
    private readonly ISubElementService _subElementService;

    public SubElementsController(ISubElementService subElementService)
    {
        _subElementService = subElementService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiPaginatedResponse<SubElementDto>>> GetSubElements(
        [FromQuery] SubElementPaginationQuery query)
    {
        try
        {
            var subElements = await _subElementService.GetSubElements(query);
            return subElements.ToApiResponse();
        }
        catch (Exception e)
        {
            return BadRequest(e.ToErrorResponse());
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<SubElementDto>>> GetSubElement(int id)
    {
        try
        {
            var subElement = await _subElementService.GetSubElement(id);
            return subElement.ToApiResponse();
        }
        catch (Exception e)
        {
            return BadRequest(e.ToErrorResponse());
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<SubElementDto>>> AddSubElement(SubElementCreateDto dto)
    {
        try
        {
            var subElement = await _subElementService.AddSubElement(dto);
            return CreatedAtAction(nameof(GetSubElement), new { id = subElement.Id }, subElement.ToApiResponse());
        }
        catch (ValidationException validationException)
        {
            return BadRequest(validationException.ToValidationErrorResponse());
        }
        catch (Exception e)
        {
            return BadRequest(e.ToErrorResponse());
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<SubElementDto>>> PutSubElement(int id, SubElementUpdateDto dto)
    {
        try
        {
            var subElement = await _subElementService.UpdateSubElement(id, dto);
            return subElement.ToApiResponse();
        }
        catch (ValidationException validationException)
        {
            return BadRequest(validationException.ToValidationErrorResponse());
        }
        catch (Exception e)
        {
            return BadRequest(e.ToErrorResponse());
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<SubElementDto>>> DeleteSubElement(int id)
    {
        try
        {
            var subElement = await _subElementService.DeleteSubElement(id);
            return subElement.ToApiResponse();
        }
        catch (Exception e)
        {
            return BadRequest(e.ToErrorResponse());
        }
    }
}