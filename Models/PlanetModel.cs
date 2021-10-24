using Microsoft.AspNetCore.Authentication;

namespace App.Models{

    public class PlanetModel{
        public int ID { get; set; }
        public string Name { get; set; }
        public string VnName { get; set; }
        public string Content { get; set; }
    }
}