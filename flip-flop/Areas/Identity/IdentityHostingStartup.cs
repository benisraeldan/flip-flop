using System;
using flip_flop.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(flip_flop.Areas.Identity.IdentityHostingStartup))]
namespace flip_flop.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<flip_flopContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("flip_flopContextConnection")));

                //services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<flip_flopContext>();
            });
        }
    }
}