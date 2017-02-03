using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Bapteme.Models;

namespace Bapteme.Data
{
    public class BaptemeDataContext : DbContext
    {
        public DbSet<Paroisse> Paroisses {get; set;}
		public DbSet<Clocher> Clochers { get; set;}
		public DbSet<UserParoisse> UserParoisse { get; set; }
		public DbSet<Celebrant> Celebrants { get; set; }
		public DbSet<Celebration> Celebrations { get; set; }
		public DbSet<Adresse> Adresses { get; set; }
		public DbSet<Preparation> Preparations { get; set; }

        public BaptemeDataContext(DbContextOptions<BaptemeDataContext> options)
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
    }
}
