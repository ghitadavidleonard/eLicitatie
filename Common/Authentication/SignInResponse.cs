using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Authentication
{
    public class SignInResponse
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public int Role { get; set; }
    }
}
