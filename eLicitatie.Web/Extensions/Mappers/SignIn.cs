using Common.Authentication;
using eLicitatie.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eLicitatie.Web.Extensions.Mappers
{
    public static class SignIn
    {
        public static SignInRequest MapToRequest(this SignInViewModel model) 
        {
            return new SignInRequest
            {
                Email = model.Email,
                Password = model.Password
            };
        }
    }
}
