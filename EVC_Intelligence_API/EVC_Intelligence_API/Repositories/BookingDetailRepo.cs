using EVC_Intelligence_API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace EVC_Intelligence_API.Repositories
{
    public class BookingDetailRepo : IBookingDetail
    {
        private readonly EVC_IntelligenceContext _context;

        public BookingDetailRepo(EVC_IntelligenceContext context)
        {
            _context = context;
        }

        // Get Booking Details By userId
        public async Task<BookingDetail> GetBookingByUser(int userId)
        {
            var User = await _context.BookingDetails.ToListAsync();
            var booking = User.Find(x => x.UserId == userId);
            if (booking == null) return booking;
            else
            {
                if(DateTime.Now.Date > booking.Date)
                {
                    _context.BookingDetails.Remove(booking);
                    await _context.SaveChangesAsync();
                    return null;
                }
                else if(DateTime.Now.Date == booking.Date)
                {
                    if(DateTime.Now.TimeOfDay.Hours > ((TimeSpan)booking.SlotTime).Hours+1)
                    {
                        _context.BookingDetails.Remove(booking);
                        await _context.SaveChangesAsync();
                        return null;
                    }
                    else
                    {
                        return booking;
                    }
                }
                else
                {
                    return booking;
                }
            }
        }

        //Get List Of All Booking Details
        public async Task<List<BookingDetail>> GetBookingDetail()
        {
            return await _context.BookingDetails.ToListAsync();
        }

        //Add Booking Details
        public async Task<BookingDetail> Create(BookingDetail bookingDetail)
        {
            _context.BookingDetails.Add(bookingDetail);
            await _context.SaveChangesAsync();

            return bookingDetail;
        }

        // Delete Booking of User
        public async Task CancelBooking(int userid)
        {
            var bookingDetails = await _context.BookingDetails.ToListAsync();
            foreach (var item in bookingDetails)
            {
                if (item.UserId == userid)
                {
                    _context.BookingDetails.Remove(item);
                    await _context.SaveChangesAsync();
                }
            }
        }

    }
}
