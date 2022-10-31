using EVC_Intelligence_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVC_Intelligence_API.Repositories
{
    public interface IBookingDetail
    {
        Task<BookingDetail> GetBookingByUser(int userId);
        Task<List<BookingDetail>> GetBookingDetail();
        Task<BookingDetail> Create(BookingDetail bookingDetail);
        Task CancelBooking(int userid);

    }
}
