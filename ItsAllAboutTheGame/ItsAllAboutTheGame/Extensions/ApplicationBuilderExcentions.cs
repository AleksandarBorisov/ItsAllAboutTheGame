using ItsAllAboutTheGame.Middleware;
using Microsoft.AspNetCore.Builder;


namespace ItsAllAboutTheGame.Extensions
{
    public static class ApplicationBuilderExcentions
    {
        public static void UseNotFoundExceptionHandler(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<EntityNotFoundMiddleware>();
        }
    }
}
