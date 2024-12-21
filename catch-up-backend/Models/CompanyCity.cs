using System.ComponentModel.DataAnnotations;

namespace catch_up_backend.Models
{
    public class CompanyCity
    {
        [Key]
        public string CityName { get; set; }  
        public double Latitude { get; set; }   
        public double Longitude { get; set; }  
        public double RadiusKm { get; set; }
    }
}
