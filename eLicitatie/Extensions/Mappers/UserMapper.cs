using Common.Authentication;
using eLicitatie.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eLicitatie.Api.Extensions.Mappers
{
    public static class UserMapper
    {
        public static User MapToEntity(this SignUpRequest signInRequest)
        {
            if (signInRequest is null) throw new ArgumentNullException(nameof(signInRequest));

            return new User
            {
                FirstName = signInRequest.FirstName,
                LastName = signInRequest.LastName,
                Email = signInRequest.Email,
                Password = signInRequest.Password
            };
        }

        public static SignInResponse MapToResponse(this User user)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));

            return new SignInResponse
            {
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role
            };
        }

        public static UserResponse MapToUserResponse(this User user)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));

            return new UserResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }
    }
}
