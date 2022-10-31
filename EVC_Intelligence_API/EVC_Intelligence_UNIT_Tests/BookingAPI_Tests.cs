using System;
using EVC_Intelligence_API.Controllers;
using EVC_Intelligence_API.Models;
using EVC_Intelligence_API.Repositories;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EVC_Intelligence_UNIT_Tests
{
    [TestFixture]
    public class BookingAPI_Tests
    {
        
        
            EVC_IntelligenceContext context;
            ChargingStationRepo chargingstationrepo;
            BookingDetailRepo bookingdetailrepo;
            UserDetailRepo userdetailrepo;
            [SetUp]
            public void Init()
            {

                context = new EVC_IntelligenceContext();
                chargingstationrepo = new ChargingStationRepo(context);
                bookingdetailrepo = new BookingDetailRepo(context);
                userdetailrepo = new UserDetailRepo(context);
            }
            [TearDown]
            public void Dispose()
            {
                context = null;
            }
            [Test]
            [Category("Get Station By City")]
            public async Task GetAllStationByCity_ShouldReturnCorrectStatusCode()
            {
                GetChargingStationController controller = new GetChargingStationController(bookingdetailrepo, chargingstationrepo);
                DateTime date = new DateTime(2021, 08, 02, 00, 00, 00);
                IActionResult actionResult = await controller.GetAllChargingStations("Pune", date, 1);
                var okresult = actionResult as OkObjectResult;
                Assert.AreEqual(200, okresult.StatusCode);
            }
            [Test]
            [Category("Get Station By Pincode")]
            public async Task GetAllStationByPinCode_ShouldReturnCorrectStatusCode()
            {
                GetChargingStationController controller = new GetChargingStationController(bookingdetailrepo, chargingstationrepo);
                DateTime date = new DateTime(2021, 08, 02, 00, 00, 00);
                IActionResult actionResult = await controller.GetAllChargingStations("412207", date, 0);
                var okresult = actionResult as OkObjectResult;
                Assert.AreEqual(200, okresult.StatusCode);
            }
            [Test]
            [Category("Get Station By City")]
            public async Task GetAllStationByCity_ShouldReturnCountOfMatchingStations()
            {
                GetChargingStationController controller = new GetChargingStationController(bookingdetailrepo, chargingstationrepo);
                DateTime date = new DateTime(2021, 08, 02, 00, 00, 00);
                IActionResult actionResult = await controller.GetAllChargingStations("Pune", date, 1);
                var okresult = actionResult as OkObjectResult;
                var result = okresult.Value;
                IList collection = (IList)result;
                Assert.AreEqual(2, collection.Count);
            }
            [Test]
            [Category("Get Station By Pincode")]
            public async Task GetAllStationByPincode_ShouldReturnCountOfMatchingStations()
            {
                GetChargingStationController controller = new GetChargingStationController(bookingdetailrepo, chargingstationrepo);
                DateTime date = new DateTime(2021, 08, 02, 00, 00, 00);
                IActionResult actionResult = await controller.GetAllChargingStations("412207", date, 0);
                var okresult = actionResult as OkObjectResult;
                var result = okresult.Value;
                IList collection = (IList)result;
                Assert.AreEqual(1, collection.Count);
            }
            [Test]
            [Category("Get Station By Pincode")]
            public async Task GetStationByPincode_ShouldReturnCorrectCityID()
            {
                var result = await chargingstationrepo.GetAllByPincode(412207);
                // !confirm indexing result[0]
                var temp = result[0].CityId;
                Assert.AreEqual(1, temp);
            }
            [Test]
            [Category("Get Station By Pincode")]
            public async Task GetStationByPincode_ShouldReturnCorrectStationName()
            {
                ChargingStationRepo repo = new ChargingStationRepo(context);
                var result = await repo.GetStationName(1);
                Assert.AreEqual("Wagholi_Charging_Station", result);
            }
            [Test]
            [Category("Get Station By City")]
            public async Task GetStationByCityName_ShouldReturnCorrectStationName()
            {
                var result = await chargingstationrepo.GetCities("Pune");
                Assert.AreEqual(1, result[0].StationId);
                Assert.AreEqual("Wagholi_Charging_Station", result[0].StationName);
            }
            [Test]
            [Category("Booking Details")]
            public async Task GetBookingDetails_ShouldReturnAllBookingDetails()
            {
                var result = await bookingdetailrepo.GetBookingDetail();
                Assert.AreEqual(4, result.Count);
                Assert.AreEqual(1, result[0].StationId);
            }
    }
}
