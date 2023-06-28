using System;
using System.Linq.Expressions;
using MagicVilla_VillaAPI.Model;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
	public interface IVillaRepository

	{
		Task CreateAsync(Villa entity);

		Task RemoveAsync(Villa entity);

		Task<Villa> UpdateAsync(Villa entity);

		Task SaveAsync();

		Task<List<Villa>> GetAllAsync(Expression<Func<Villa, bool>> filter = null);

		Task<Villa> GetAsync(Expression<Func<Villa, bool>> filter = null, bool tracked = true);


	}
}

