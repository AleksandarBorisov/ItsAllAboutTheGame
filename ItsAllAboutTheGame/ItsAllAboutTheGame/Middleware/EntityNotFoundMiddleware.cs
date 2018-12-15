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
            catch (EntityNotFoundException)
            {
                context.Response.Redirect("/404");
            }
            catch (EntityAlreadyExistsException)
            {
                context.Response.Redirect("/404");
            }
            catch (LockoutUserException)
            {
                context.Response.Redirect("/404");
            }
            catch (ArgumentException)
            {
                context.Response.Redirect("/404");
            }
            catch (HttpRequestException)
            {
                context.Response.Redirect("/ForeignApiError");
            }
            catch (HttpStatusCodeException)
            {
                context.Response.Redirect("/ForeignApiError");
            }
            catch (Exception)
            {
                context.Response.Redirect("/Error/Index");
            }
        }
    }
}
