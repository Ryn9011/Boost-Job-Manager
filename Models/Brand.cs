using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace JobTracker.API.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Job> Jobs { get; set; }
        public Brand()
        {
            Jobs = new Collection<Job>();
        }
    }
}