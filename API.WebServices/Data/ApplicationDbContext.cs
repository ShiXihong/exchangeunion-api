using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using API.WebServices.Models;

namespace API.WebServices.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Subscribe> Subscribe { get; set; }

        public DbSet<Contact> Contact { get; set; }

        public DbSet<Press> Press { get; set; }

        public DbSet<Email> Email { get; set; }

        public DbSet<Portfolio> Portfolio { get; set; }

        public DbSet<XUCTeam> Team { get; set; }

        public DbSet<DFGTeam> DFGTeam { get; set; }

        public DbSet<XUCExchange> XUCExchange { get; set; }

        public DbSet<Translation> Translation { get; set; }

        public DbSet<Blog> Blog { get; set; }

        public DbSet<Jobs> Jobs { get; set; }

        public DbSet<Transparency> Transparency { get; set; }

        public DbSet<Events> Events { get; set; }

        public DbSet<Education> Education { get; set; }
    }
}
