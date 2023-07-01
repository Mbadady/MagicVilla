using System;
using System.Linq.Expressions;
using MagicVilla_VillaAPI.Model;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
	public interface IVillaNumberRepository
	{
		Task<List<VillaNumber>> GetAllVillaNumberAsync(Expression<Func<VillaNumber, bool>> filter = null, string? includeProperties = null, int pageSize = 0, int pageNumber = 1);
		Task<VillaNumber> GetVillaNumberAsync(Expression<Func<VillaNumber, bool>> filter = null, bool tracked = true, string? includeProperties = null);
		Task CreateAsync(VillaNumber entity);
		Task RemoveAsync(VillaNumber entity);
		Task<VillaNumber> UpdateAsync(VillaNumber entity);
		Task SaveAsync();

	}
}

