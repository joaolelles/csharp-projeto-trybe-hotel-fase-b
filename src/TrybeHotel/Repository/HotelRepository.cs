using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class HotelRepository : IHotelRepository
    {
        protected readonly ITrybeHotelContext _context;
        public HotelRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public IEnumerable<HotelDto> GetHotels()
        {
            var getHotels = from hotel in _context.Hotels
                            join city in _context.Cities on hotel.CityId equals city.CityId
                            select new HotelDto
                            {
                                HotelId = hotel.HotelId,
                                Name = hotel.Name,
                                Address = hotel.Address,
                                CityId = city.CityId,
                                CityName = city.Name,
                            };


            return getHotels;
        }

        public HotelDto AddHotel(Hotel hotel)
        {
            _context.Hotels.Add(hotel);
            _context.SaveChanges();
            var city = _context.Cities.FirstOrDefault(city => city.CityId == hotel.CityId);
            // var postHotels = new HotelDto
            // {
            //     HotelId = hotel.HotelId,
            //     Name = hotel.Name,
            //     Address = hotel.Address,
            //     CityId = city.CityId,
            //     CityName = city.Name,
            // };
            // return postHotels;
            if (city != null)
            {
                var postHotels = new HotelDto
                {
                    HotelId = hotel.HotelId,
                    Name = hotel.Name,
                    Address = hotel.Address,
                    CityId = city.CityId,
                    CityName = city.Name,
                };
                return postHotels;
            }
            else
            {
                throw new ApplicationException("Cidade não encontrada.");
            }
        }
    }
}