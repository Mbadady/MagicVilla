using System;
using System.Linq.Expressions;
using MagicVilla_VillaAPI.DataStore;
using MagicVilla_VillaAPI.Model;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaNumberRepository : IVillaNumberRepository
    {
        private readonly ApplicationDbContext _db;

        public VillaNumberRepository(ApplicationDbContext db)
        {
            _db = db;
          
        }
        public async Task CreateAsync(VillaNumber entity)
        {
            await _db.VillaNumbers.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<List<VillaNumber>> GetAllVillaNumberAsync(Expression<Func<VillaNumber, bool>> filter = null, string? includeProperties = null)
        {
            IQueryable<VillaNumber> query = _db.VillaNumbers;

            if ( filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            //return await query.ToListAsync();
            return await query.ToListAsync();

        }

        public async Task<VillaNumber> GetVillaNumberAsync(Expression<Func<VillaNumber, bool>> filter = null, bool tracked = true, string? includeProperties = null)
        {
            IQueryable<VillaNumber> query = _db.VillaNumbers;
            if (!tracked)
            {
                query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if(includeProperties != null)
            {
                foreach(var includeProp in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            //return await query.ToListAsync();
            return await query.FirstOrDefaultAsync();

        }

        public async Task RemoveAsync(VillaNumber entity)
        {
            _db.VillaNumbers.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<VillaNumber> UpdateAsync(VillaNumber entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _db.VillaNumbers.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }

        
    }
}

