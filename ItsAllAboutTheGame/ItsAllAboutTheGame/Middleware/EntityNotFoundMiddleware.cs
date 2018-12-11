using ItsAllAboutTheGame.Services.Data.Exceptions;
using ItsAllAboutTheGame.Services.External.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.Middleware
{
    public class EntityNotFoundMiddleware
    {
        private readonly RequestDelegate next;

        public EntityNotFoundMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.next.Invoke(context);

                if (context.Response.StatusCode == 404)
                {
                    context.Response.Redirect("/404");
                }

            }
            catch (EntityNotFoundException ex)
            {
                context.Response.Redirect("/404");
            }
            catch (LockoutUserException ex)
            {
                context.Response.Redirect("/404");
            }
            catch (ArgumentException ex)
            {
                context.Response.Redirect("/404");
            }
            catch (HttpRequestException ex)
            {
                context.Response.Redirect("/ForeignApiError");
            }
            catch (HttpStatusCodeException ex)
            {
                context.Response.Redirect("/ForeignApiError");
            }
            catch (Exception ex)
            {
                context.Response.Redirect("/Error/Index");
            }
        }
    }
}
