using FluentValidation;
using SalesOrder.Common.Models;

namespace SalesOrder.API.Helpers;

public static class ApiResponseHelper
{
    public static ApiPaginatedResponse<TResult> ToApiResponse<TResult>(this PaginatedList<TResult> result,
        string message = null)
    {
        return new ApiPaginatedResponse<TResult>
        {
            Message = message,
            Result = result,
            Success = true
        };
    }

    public static ApiResponse<TResult> ToApiResponse<TResult>(this TResult result, string message = null)
    {
        return new ApiResponse<TResult>
        {
            Message = message,
            Result = result,
            Success = true
        };
    }

    public static ApiResponse<TResult> ToErrorResponse<TResult>(this TResult result) where TResult : Exception
    {
        return new ApiResponse<TResult> { Message = $"{result.Message}", Result = null, Success = false };
    }

    public static ApiResponse<Dictionary<string, string>> ToValidationErrorResponse(this ValidationException ex)
    {
        var errorMessagesMap = new Dictionary<string, string>();

        ex.Errors.ToList().ForEach(error => { errorMessagesMap.Add(error.PropertyName, error.ErrorMessage); });

        return new ApiResponse<Dictionary<string, string>>
        {
            Message = "Request payload validation failed.",
            Result = errorMessagesMap,
            Success = false
        };
    }
}