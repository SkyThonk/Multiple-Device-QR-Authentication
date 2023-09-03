using MultiDeviceQrLogin.Exceptions;

namespace MultiDeviceQrLogin.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            // Handle custom NotFoundException
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new ApiResponse<object> { Success = false, Message = ex.Message });
        }
        catch (ApplicationException ex)
        {
            
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new ApiResponse<object> { Success = false, Message = ex.Message });

        }
        catch (ValidationException ex)
        {
            // Handle validation errors
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new ApiResponse<object> { Success = false, Message = ex.Message });
        }
        catch (DbUpdateException)
        {
            // Handle database update errors (Entity Framework)
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new ApiResponse<object> { Success = false, Message = "A database error occurred." });
        }
        catch (SqlException)
        {
            // Handle SQL Server unique constraint violations
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new ApiResponse<object> { Success = false, Message = "Data already exists." });
        }
        catch (UnauthorizedAccessException)
        {
            // Handle unauthorized access
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new ApiResponse<object> { Success = false, Message = "Unauthorized access." });
        }
        catch (ForbiddenAccessException)
        {
            // Handle forbidden access
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsJsonAsync(new ApiResponse<object> { Success = false, Message = "Forbidden access." });
        }
        catch (ConflictDataException ex){
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            await context.Response.WriteAsJsonAsync(new ApiResponse<object> {Success = false, Message= ex.Message});

        }
        catch (Exception)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new ApiResponse<object> { Success = false, Message = "An error occurred while processing your request." });
        }
    }
}
