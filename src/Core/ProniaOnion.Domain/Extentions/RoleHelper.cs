using ProniaOnion.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Domain.Extentions
{
    public class AuthorizeRolesAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params UserRole[] allowedroles)
        {
            var allowedrolesAsString = allowedroles.Select(x => Enum.GetName(typeof(UserRole), x));
            Roles = string.Join(",", allowedrolesAsString);
        }
    }
}
