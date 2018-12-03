using ItsAllAboutTheGame.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            var errorModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier
            };

            return this.View(errorModel);
        }

        public IActionResult PageNotFound()
        {
            return this.View();
        }

        public IActionResult ForeignApiError()
        {
            return this.View();
        }

    }
}
