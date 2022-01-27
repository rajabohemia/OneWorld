using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace OneWorld.Models
{
    public class Service
    {
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public DateTime DateStamp { get; set; }

        public ICollection<Servicefaq> Servicefaqs { get; set; }
    }
}