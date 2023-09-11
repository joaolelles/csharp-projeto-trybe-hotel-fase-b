using TrybeHotel.Models;
using TrybeHotel.Dto;
using Microsoft.EntityFrameworkCore;

namespace TrybeHotel.Repository
{
    public class BookingRepository : IBookingRepository
    {
        protected readonly ITrybeHotelContext _context;
        public BookingRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public BookingResponse Add(BookingDtoInsert booking, string email)
        {
            Room? room = GetRoomById(booking.RoomId);
            var hotel = _context.Hotels.FirstOrDefault(h => h.HotelId == room.HotelId);
            var city = _context.Cities.FirstOrDefault(c => c.CityId == hotel.CityId);
            if (room == null || booking.GuestQuant > room.Capacity || hotel == null || city == null)
            {
                return null;
            }


            Booking newBooking = new()
            {
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                GuestQuant = booking.GuestQuant,
                Room = room,
            };

            _context.Bookings.Add(newBooking);
            _context.SaveChanges();

            var bookingResponse = new BookingResponse
            {
                BookingId = newBooking.BookingId,
                CheckIn = newBooking.CheckIn,
                CheckOut = newBooking.CheckOut,
                GuestQuant = newBooking.GuestQuant,
                Room = new RoomDto
                {
                    RoomId = room.RoomId,
                    Name = room.Name,
                    Capacity = room.Capacity,
                    Image = room.Image,
                    Hotel = new HotelDto
                    {
                        HotelId = hotel.HotelId,
                        Name = hotel.Name,
                        Address = hotel.Address,
                        CityId = hotel.CityId,
                        CityName = city.Name,
                    }
                }
            };
            return bookingResponse;
        }

        public BookingResponse GetBooking(int bookingId, string email)
        {
            throw new NotImplementedException();
        }

        public Room GetRoomById(int RoomId)
        {
            Room? room = _context.Rooms.FirstOrDefault(r => r.RoomId == RoomId);
            return room!;
        }

    }

}