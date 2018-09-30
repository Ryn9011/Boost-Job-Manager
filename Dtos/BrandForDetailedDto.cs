using System.Collections.Generic;
using System.Collections.ObjectModel;
using JobTracker.API.Models;

namespace JobTracker.API.Dtos
{
    public class BrandForDetailedDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<JobDetailedDto> Jobs { get; set; }
        public BrandForDetailedDto()
        {
            Jobs = new Collection<JobDetailedDto>();
        }
    }
}