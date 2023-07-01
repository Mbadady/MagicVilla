using System;
using Microsoft.AspNetCore.Identity;

namespace MagicVilla_VillaAPI.Model
{
	public class ApplicationUser : IdentityUser
	{
		public string Name { get; set; }

	}
}

