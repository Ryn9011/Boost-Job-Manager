using System;
using System.Collections.Generic;
using JobTracker.API.Models;

namespace JobTracker.API.Dtos
{
    public class JobForUpdateDto
    {
        public int JobNumber { get; set; }
        public string Item { get; set; }
        public string Status { get; set; }
        public string OrderedBy { get; set; }
        public string OrderDate { get; set; }
        public int Quantity { get; set; }
        public string UnitPrice { get; set; }
        public string LineValue { get; set; }
        public string DpBp { get; set; }
        public string Eta { get; set; }
        public string EtaText { get; set; }
        public string Sku { get; set; }
        public string DetailStatus { get; set; }
        public string ShippingInfo { get; set; }
        public string Comments { get; set; }
        public string OrderDetails{ get; set; }
        public DateTime LastModifed { get; set; }
        public string modificationDetails { get; set; }
    


    }
}