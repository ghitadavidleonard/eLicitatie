using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Authentication
{
    public class SignUpRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
