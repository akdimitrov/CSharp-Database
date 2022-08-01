using System;
using System.Collections.Generic;
using System.Linq;
using RealEstates.Data;
using RealEstates.Models;

namespace RealEstates.Services
{
    public class TagService : BaseService, ITagService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IPropertiesService propertiesService;

        public TagService(ApplicationDbContext dbContext, IPropertiesService propertiesService)
        {
            this.dbContext = dbContext;
            this.propertiesService = propertiesService;
        }

        public void Add(string name, int? importance = null)
        {
            var tag = new Tag
            {
                Name = name,
                Importance = importance
            };

            dbContext.Tags.Add(tag);
            dbContext.SaveChanges();
        }

        public void BulkTagToPropertiesRelation()
        {
            var allPropertis = dbContext.Properties.ToList();
            var tags = dbContext.Tags.ToList();

            foreach (var property in allPropertis)
            {

                var averagePriceForDistrict = propertiesService
                    .AveragePricePerSquareMeter(property.DistrictId);
                if (property.Price.HasValue && property.Price >= averagePriceForDistrict)
                {
                    var tag = GetTag(tags, "скъп-имот");
                    property.Tags.Add(tag);
                }
                else if (property.Price.HasValue && property.Price < averagePriceForDistrict)
                {
                    var tag = GetTag(tags, "евтин-имот");
                    property.Tags.Add(tag);
                }

                var currentDate = DateTime.Now.AddYears(-15);
                if (property.Year.HasValue && property.Year > currentDate.Year)
                {
                    var tag = GetTag(tags, "новo-строителство");
                    property.Tags.Add(tag);
                }
                else if (property.Year.HasValue && property.Year <= currentDate.Year)
                {
                    var tag = GetTag(tags, "старо-строителство");
                    property.Tags.Add(tag);
                }

                var averageProperySize = propertiesService.AverageSize(property.DistrictId);
                if (property.Size >= averageProperySize)
                {
                    var tag = GetTag(tags, "голям-имот");
                    property.Tags.Add(tag);
                }
                else if (property.Size < averageProperySize)
                {
                    var tag = GetTag(tags, "малък-имот");
                    property.Tags.Add(tag);
                }

                if (property.Floor.HasValue && property.Floor.Value == 1)
                {
                    var tag = GetTag(tags, "първи-етаж");
                    property.Tags.Add(tag);
                }
                else if (property.Floor.HasValue && property.TotalFloors.HasValue &&
                    property.Floor == property.TotalFloors && property.TotalFloors > 1)
                {
                    var tag = GetTag(tags, "последен-етаж");
                    property.Tags.Add(tag);
                }
            }

            dbContext.SaveChanges();
        }

        private Tag GetTag(ICollection<Tag> tags, string tagName)
        {
            return tags.FirstOrDefault(t => t.Name == tagName);
        }
    }
}
