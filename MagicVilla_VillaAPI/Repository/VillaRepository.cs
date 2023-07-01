using System;
using System.Linq.Expressions;
using MagicVilla_VillaAPI.DataStore;
using MagicVilla_VillaAPI.Model;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaRepository : IVillaRepository
    {
        private readonly DataStore.ApplicationDbContext _db;

        public VillaRepository(DataStore.ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task CreateAsync(Villa entity)
        {
           await _db.Villas.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<Villa> GetAsync(Expression<Func<Villa, bool>> filter = null, bool tracked = true)
        {
            IQueryable<Villa> query = _db.Villas;

            if (!tracked)
            {
                query.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<Villa>> GetAllAsync(Expression<Func<Villa, bool>> filter = null, int pageSize = 0, int pageNumber = 1)
        {
            IQueryable<Villa> query = _db.Villas;

            if(filter != null)
            {
                query = query.Where(filter);
            }

            if(pageSize > 0)
            {
                if(pageSize > 100)
                {
                    pageSize = 100;
                }
                query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            }

            return await query.ToListAsync();
        }

        public async Task RemoveAsync(Villa entity)
        {
            _db.Villas.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<Villa> UpdateAsync(Villa entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _db.Villas.Update(entity);
            await SaveAsync();

            return entity;
        }
    }
}

