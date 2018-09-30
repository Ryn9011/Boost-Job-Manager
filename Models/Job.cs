using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace JobTracker.API.Models
{
    public class Job
    {
        public int Id { get; set; }
        public int JobNumber { get; set; }
        public string Item { get; set; }
        public string Status { get; set; }
        public string OrderedBy { get; set; }
        public DateTime OrderDate { get; set; }
        public int Quantity { get; set; }
        public Decimal UnitPrice { get; set; }
        public Decimal LineValue { get; set; }
        public string DpBp { get; set; }
        public DateTime? Eta { get; set; }
        public string EtaText { get; set; }
        public string DetailStatus { get; set; }
        public string Comments { get; set; }
        public string OrderDetails { get; set; }
        public string Sku { get; set; }
        public string ShippingInfo { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }
        public string ModificationDetails { get; set; }
        public Brand Brand { get; set; }
        public int BrandId { get; set; }

        public ICollection<Photo> Photos { get; set; }

        public Job() {
            Photos = new Collection<Photo>();
        }
    }
}