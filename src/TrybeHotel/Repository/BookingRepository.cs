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
            if (room == null)
            {
                return null!;
            }
            if (booking.GuestQuant > room.Capacity)
            {
                return null!;
            }

            var newBooking = new Booking
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
                Room = room,
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