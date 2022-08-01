using System.Collections.Generic;
using System.Linq;
using AutoMapper.QueryableExtensions;
using RealEstates.Data;
using RealEstates.Services.Models;

namespace RealEstates.Services
{
    public class DistrictService : BaseService, IDistrictsService
    {
        private readonly ApplicationDbContext dbContext;

        public DistrictService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<DistrictInfoDto> GetMostExpensiveDistricts(int count)
        {
            var districts = dbContext.Districts
                .ProjectTo<DistrictInfoDto>(Mapper.ConfigurationProvider)
                .OrderByDescending(x => x.AveragePricePerSquareMeter)
                .Take(count)
                .ToList();

            return districts;
        }
    }
}
