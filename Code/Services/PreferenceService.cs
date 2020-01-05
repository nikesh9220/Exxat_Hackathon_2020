using Hackathon.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hackathon.Services
{
    public class PreferenceService:IPreferenceService<UserPreferences,int>
    {
        private readonly _DbContext ctx;
        /// <summary>
        /// Injecting the DbContext class in the Service
        /// </summary>
        /// <param name="ctx"></param>
        public PreferenceService(_DbContext ctx)
        {
            this.ctx = ctx;
        }

        public async Task<IEnumerable<UserPreferences>> GetHeadersAsync()
        {
            var res = await ctx.userPreferences.ToListAsync();
            return res;
        }

        public async Task<UserPreferences> Update(int id, string preference)
        {
            try
            {
                var res = await ctx.userPreferences.FindAsync(id);
                res.PreferanceValue = preference;
                await ctx.SaveChangesAsync();
                return res;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
