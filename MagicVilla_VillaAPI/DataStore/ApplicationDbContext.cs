using System;
using MagicVilla_VillaAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.DataStore
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{

		}
		public DbSet<Villa> Villas { get; set; }
		public DbSet<VillaNumber> VillaNumbers { get; set; }
		public DbSet<LocalUser> LocalUsers { get; set; }


	}
}

