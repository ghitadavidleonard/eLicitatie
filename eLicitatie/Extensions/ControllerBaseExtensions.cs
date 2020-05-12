using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eLicitatie.Api.Extensions
{
    public static class ControllerBaseExtensions
    {
        public static StatusCodeResult InternalServerError(this ControllerBase controller)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}
