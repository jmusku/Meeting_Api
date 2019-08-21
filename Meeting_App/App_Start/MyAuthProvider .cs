using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Meeting_App.ORM;
using Meeting_App.Services;
using Microsoft.Owin.Security.OAuth;

namespace Meeting_App.App_Start
{
    public class MyAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly Meeting_DBEntities _dbContext = new Meeting_DBEntities();
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            var encryptPassword = context.Password.EnCrypt();

            var userData = _dbContext.tbl_User.SingleOrDefault(u => u.User_Name == context.UserName && u.Password.ToString() == encryptPassword);

            if (userData != null)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, userData.User_Role));
                identity.AddClaim(new Claim(ClaimTypes.Name, userData.First_Name));
                context.Validated(identity);
            }
            else
            {
                context.SetError("invalid_grant", "Provided username and password is incorrect");
                context.Rejected();
            }
        }

    }
}