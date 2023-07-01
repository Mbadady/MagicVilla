using System;
using MagicVilla_VillaAPI.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.DataStore
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{

		}

		public DbSet<ApplicationUser> ApplicationUsers { get; set; }

		public DbSet<Villa> Villas { get; set; }
		public DbSet<VillaNumber> VillaNumbers { get; set; }
		public DbSet<LocalUser> LocalUsers { get; set; }


		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			//To Seed data to DB on building
			//builder.Entity<Villa>().HasData(
			//	new Villa
			//	{

			//	},
			//	new Villa
			//	{

			//	}
			//	);


		}
	}
}

