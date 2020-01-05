using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;
using OnTrackWebService.Repository;

namespace OnTrackWebService.Data
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        //public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        //{
        //    using (UserRepository _userRepository = new UserRepository())
        //    {
        //        var user = await _userRepository.SignIn(context.UserName, context.Password, "");

        //        if(user == null)
        //        {
        //            context.SetError("invalid_grant", "Incorrect email and/or password");

        //            return;
        //        }

        //        var identity = new ClaimsIdentity(context.Options.AuthenticationType);

        //        identity.AddClaim(new Claim(ClaimTypes.Role, user.UserType.Name));
        //        identity.AddClaim(new Claim(ClaimTypes.Name, user.Email));

        //        context.Validated(identity);
        //    }
        //}
    }
}
