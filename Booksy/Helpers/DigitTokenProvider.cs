using Booksy.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booksy.Helpers
{
    public class DigitTokenProvider: PhoneNumberTokenProvider<ApplicationUser>
    {
        public override Task<string> GenerateAsync(string purpose, UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            return base.GenerateAsync(purpose, manager, user);
        }
    }
}
