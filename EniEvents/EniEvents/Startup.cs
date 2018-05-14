using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using Bo;
using System.Web.Security;
using EniEvents.Models;

[assembly: OwinStartupAttribute(typeof(EniEvents.Startup))]
namespace EniEvents
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app); using (var context = new ApplicationDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    var roleManager = new RoleManager<IdentityRole>
                    (new RoleStore<IdentityRole>(context));

                    var userManager = new UserManager<ApplicationUser>
                        (new UserStore<ApplicationUser>(context));

                    if (!roleManager.RoleExists(UserRoles.ROLE_ADMIN))
                    {
                        roleManager.Create(new IdentityRole(UserRoles.ROLE_ADMIN));
                        var admin = new ApplicationUser
                        {
                            UserName = "hsorais@graph-et-in.com",
                            Email = "hsorais@graph-et-in.com"
                        };

                        var result = userManager.Create(admin, "Pa$$w0rd");
                        if (result.Succeeded)
                        {
                            userManager.AddToRole(admin.Id, UserRoles.ROLE_ADMIN);

                            if (!roleManager.RoleExists(UserRoles.ROLE_ADMIN))
                                roleManager.Create(new IdentityRole(UserRoles.ROLE_ADMIN));

                            if (!roleManager.RoleExists(UserRoles.ROLE_USER))
                                roleManager.Create(new IdentityRole(UserRoles.ROLE_USER));

                            transaction.Commit();
                        }
                    }
                }
            }
        }
    }
}
