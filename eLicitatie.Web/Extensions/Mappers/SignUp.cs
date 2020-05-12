using Common.Authentication;
using eLicitatie.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eLicitatie.Web.Extensions.Mappers
{
    public static class SignUp
    {
        public static SignUpRequest MapToRequest(this SignUpViewModel model)
        {
            return new SignUpRequest
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.EmailAddress,
                Password = model.Password
            };
        }
    }
}
