using System;

namespace OneWorld.Models
{
    public class Servicefaq
    {
        public Guid ServicefaqId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public Guid ServiceId { get; set; }
        public Service Service { get; set; }
    }
}