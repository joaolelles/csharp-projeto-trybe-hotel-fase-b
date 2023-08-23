using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class CityRepository : ICityRepository
    {
        protected readonly ITrybeHotelContext _context;
        public CityRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public IEnumerable<CityDto> GetCities()
        {
            var getCities = _context.Cities.Select(City => new CityDto
            {
                CityId = City.CityId,
                Name = City.Name,
            });

            return getCities;
        }

        public CityDto AddCity(City city)
        {
            _context.Cities.Add(city);
            _context.SaveChanges();
            var postCities = new CityDto
            {
                CityId = city.CityId,
                Name = city.Name,
            };
            return postCities;
        }

    }
}