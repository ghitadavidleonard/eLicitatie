using Common.Authentication;
using eLicitatie.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eLicitatie.Web.Extensions.Mappers
{
    public static class Users
    {
        public static UserModel MapToViewModel(this UserResponse response)
        {
            if (response is null) throw new ArgumentNullException(nameof(response));

            return new UserModel
            {
                Id = response.Id,
                FirstName = response.FirstName,
                LastName = response.LastName
            };
        }
    }
}
