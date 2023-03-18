using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SalesOrder.API.Helpers;
using SalesOrder.Common.DTO.Window;
using SalesOrder.Common.Models;
using SalesOrder.Service;

namespace SalesOrder.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WindowsController : ControllerBase
{
    private readonly IWindowService _windowService;

    public WindowsController(IWindowService windowService)
    {
        _windowService = windowService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiPaginatedResponse<WindowDto>>> GetWindows([FromQuery] WindowPaginationQuery query)
    {
        try
        {
            var windows = await _windowService.GetWindows(query);
            return windows.ToApiResponse();
        }
        catch (Exception e)
        {
            return BadRequest(e.ToErrorResponse());
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<WindowDto>>> GetWindow(int id)
    {
        try
        {
            var window = await _windowService.GetWindow(id);
            return window.ToApiResponse();
        }
        catch (Exception e)
        {
            return BadRequest(e.ToErrorResponse());
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<WindowDto>>> AddWindow(WindowCreateDto dto)
    {
        try
        {
            var window = await _windowService.AddWindow(dto);
            return CreatedAtAction(nameof(GetWindow), new { id = window.Id }, window.ToApiResponse());
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
    public async Task<ActionResult<ApiResponse<WindowDto>>> PutWindow(int id, WindowUpdateDto dto)
    {
        try
        {
            var window = await _windowService.UpdateWindow(id, dto);
            return window.ToApiResponse();
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
    public async Task<ActionResult<ApiResponse<WindowDto>>> DeleteWindow(int id)
    {
        try
        {
            var window = await _windowService.DeleteWindow(id);
            return window.ToApiResponse();
        }
        catch (Exception e)
        {
            return BadRequest(e.ToErrorResponse());
        }
    }
}