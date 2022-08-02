using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AutoMapper.QueryableExtensions;
using RealEstates.Data;
using RealEstates.Models;
using RealEstates.Services.Models;

namespace RealEstates.Services
{
    public class PropertiesService : BaseService, IPropertiesService
    {
        private readonly ApplicationDbContext dbContext;

        public PropertiesService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Add(string district, int price,
            int floor, int maxFloor, int size, int yardSize,
            int year, string propertyType, string buildingType)
        {
            var property = new Property
            {
                Size = size,
                Price = price <= 0 ? null : price,
                Floor = floor <= 0 || floor >= 255 ? null : (byte)floor,
                TotalFloors = maxFloor <= 0 || floor >= 255 ? null : (byte)maxFloor,
                YardSize = yardSize <= 0 ? null : yardSize,
                Year = year <= 1800 ? null : year
            };

            var dbDistrict = dbContext.Districts.FirstOrDefault(x => x.Name == district);
            if (dbDistrict == null)
            {
                dbDistrict = new District { Name = district };
            }
            property.District = dbDistrict;

            var dbPropertyType = dbContext.PropertyTypes.FirstOrDefault(x => x.Name == propertyType);
            if (dbPropertyType == null)
            {
                dbPropertyType = new PropertyType { Name = propertyType };
            }
            property.Type = dbPropertyType;

            var dbBuildingType = dbContext.BuildingTypes.FirstOrDefault(x => x.Name == buildingType);
            if (dbBuildingType == null)
            {
                dbBuildingType = new BuildingType { Name = buildingType };
            }
            property.BuildingType = dbBuildingType;

            dbContext.Properties.Add(property);
            dbContext.SaveChanges();
        }

        public decimal AveragePricePerSquareMeter()
        {
            return dbContext.Properties
                .Where(x => x.Price.HasValue)
                .Average(x => x.Price / (decimal)x.Size) ?? 0;
        }

        public decimal AveragePricePerSquareMeter(int districtId)
        {
            return dbContext.Properties
                .Where(x => x.Price.HasValue && x.DistrictId == districtId)
                .Average(x => x.Price / (decimal)x.Size) ?? 0;
        }

        public double AverageSize(int districtId)
        {
            return dbContext.Properties
                .Where(x => x.DistrictId == districtId)
                .Average(x => x.Size);
        }

        public string GetFullData(int count, int minYear, int minSize, int maxSize, 
            int minFloor, int maxFloor, int minPrice, int maxPrice)
        {
            var properties = dbContext.Properties
                .Where(x => x.Floor.HasValue && x.Floor >= minFloor && x.Floor <= maxFloor &&
                    x.Year.HasValue && x.Year >= minYear &&
                    x.Size >= minSize && x.Size <= maxSize &&
                    x.Price.HasValue && x.Price >= minPrice && x.Price <= maxPrice)
                .ProjectTo<PropertyInfoFullDataDto>(Mapper.ConfigurationProvider)
                .OrderByDescending(x => x.Price)
                .ThenBy(x => x.Size)
                .ThenBy(x => x.Year)
                .Take(count)
                .ToArray();

            var result = XmlDeserializer(properties, "Properties");

            return result;
        }

        public IEnumerable<PropertyInfoDto> Search(int minPrice, int maxPrice, int minSize, int maxSize)
        {
            var properties = dbContext.Properties.Where(
                x => x.Price >= minPrice && x.Price <= maxPrice &&
                x.Size >= minSize && x.Size <= maxSize)
                .ProjectTo<PropertyInfoDto>(Mapper.ConfigurationProvider)
                .ToList();

            return properties;
        }

        private string XmlDeserializer<T>(T collection, string xmlRootAttributeName)
        {
            var sb = new StringBuilder();
            var writer = new StringWriter(sb);
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            var serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(xmlRootAttributeName));
            serializer.Serialize(writer, collection, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}
