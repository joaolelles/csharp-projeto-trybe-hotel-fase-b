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
            User? user = _context.Users.FirstOrDefault(u => u.Email == email);
            var bookingResponse = _context.Bookings.Where(b => b.UserId == user!.UserId && b.BookingId
            == bookingId).Select(booking => new BookingResponse
            {
                BookingId = booking.BookingId,
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                GuestQuant = booking.GuestQuant,
                Room = new RoomDto
                {
                    RoomId = booking.Room!.RoomId,
                    Name = booking.Room.Name,
                    Capacity = booking.Room.Capacity,
                    Image = booking.Room.Image,
                    Hotel = new HotelDto
                    {
                        HotelId = booking.Room.Hotel!.HotelId,
                        Name = booking.Room.Hotel.Name,
                        Address = booking.Room.Hotel.Address,
                        CityId = booking.Room.Hotel.CityId,
                        CityName = booking.Room.Hotel.City!.Name,
                    }
                }
            }).FirstOrDefault();
            return bookingResponse!;
        }

        public Room GetRoomById(int RoomId)
        {
            Room? room = _context.Rooms.FirstOrDefault(r => r.RoomId == RoomId);
            return room!;
        }

    }

}