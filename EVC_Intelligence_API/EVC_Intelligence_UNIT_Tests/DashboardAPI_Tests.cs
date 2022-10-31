using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using EVC_Intelligence_API.Models;
using EVC_Intelligence_API.Controllers;
using EVC_Intelligence_API.Repositories;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.Http.Results;


namespace EVC_Intelligence_UNIT_Tests
{
    [TestFixture]
    class DashboardAPI_Tests
    {
        EVC_IntelligenceContext context;
        [SetUp]
        public void init()
        {
            context = new EVC_IntelligenceContext();
        }

        [TearDown]
        public void cleanup()
        {
            context = null;
        }

        [Test, Category("Admin Dashboard")]
        public void GetAdminDetails_ShouldReturnCorrectInstanceType()
        {
            // Arrange
            //DashboardRepository repo = new DashboardRepository(context);
            ChargingStationRepo chargingstationrepo = new ChargingStationRepo(context);
            BookingDetailRepo bookingdetailrepo = new BookingDetailRepo(context);
            UserDetailRepo userdetailrepo = new UserDetailRepo(context);
            AdminDashboardController controller = new AdminDashboardController(userdetailrepo, bookingdetailrepo, chargingstationrepo);

            // Act
            var Stations = controller.GetAdminDetails(2);

            // Assert
            Assert.IsInstanceOf<Task<List<object>>>(Stations);

        }

        [Test, Category("Admin Dashboard")]
        public void GetAdminDetails_ReturnedStationDataShouldNotBeNull()
        {
            // Arrange
            ChargingStationRepo chargingstationrepo = new ChargingStationRepo(context);
            BookingDetailRepo bookingdetailrepo = new BookingDetailRepo(context);
            UserDetailRepo userdetailrepo = new UserDetailRepo(context);
            AdminDashboardController controller = new AdminDashboardController(userdetailrepo, bookingdetailrepo, chargingstationrepo);

            // Act
            var Stations = controller.GetAdminDetails(2);

            // Assert
            Assert.IsNotNull(Stations);

        }

        [Test, Category("Admin Dashboard")]
        public void GetAdminDetails_ShouldReturnCorrectStationCount()
        {
            // Arrange
            ChargingStationRepo chargingstationrepo = new ChargingStationRepo(context);
            BookingDetailRepo bookingdetailrepo = new BookingDetailRepo(context);
            UserDetailRepo userdetailrepo = new UserDetailRepo(context);
            AdminDashboardController controller = new AdminDashboardController(userdetailrepo, bookingdetailrepo, chargingstationrepo);

            // Act
            var Stations = controller.GetAdminDetails(2).Result;
            var StnCount = Stations.Count;

            // Assert
            Assert.AreEqual(6, StnCount);

        }

        [Test, Category("SuperAdmin Dashboard")]
        public void GetAllAdminDetails_ShouldReturnCorrectInstanceType()
        {
            // Arrange
            ChargingStationRepo chargingstationrepo = new ChargingStationRepo(context);
            BookingDetailRepo bookingdetailrepo = new BookingDetailRepo(context);
            UserDetailRepo userdetailrepo = new UserDetailRepo(context);
            SuperAdminDashboardController controller = new SuperAdminDashboardController(userdetailrepo, bookingdetailrepo, chargingstationrepo);

            // Act
            var Admins = controller.GetAllAdminDetails();

            // Assert
            Assert.IsInstanceOf<Task<List<object>>>(Admins);

        }

        [Test, Category("SuperAdmin Dashboard")]
        public void GetAllAdminDetails_ReturnedAdminsDataShouldNotBeNull()
        {
            // Arrange
            ChargingStationRepo chargingstationrepo = new ChargingStationRepo(context);
            BookingDetailRepo bookingdetailrepo = new BookingDetailRepo(context);
            UserDetailRepo userdetailrepo = new UserDetailRepo(context);
            SuperAdminDashboardController controller = new SuperAdminDashboardController(userdetailrepo, bookingdetailrepo, chargingstationrepo);

            // Act
            var Admins = controller.GetAllAdminDetails();

            // Assert
            Assert.IsNotNull(Admins);

        }

        [Test, Category("SuperAdmin Dashboard")]
        public void GetAllAdminDetails_ShouldReturnCorrectAdminsCount()
        {
            // Arrange
            ChargingStationRepo chargingstationrepo = new ChargingStationRepo(context);
            BookingDetailRepo bookingdetailrepo = new BookingDetailRepo(context);
            UserDetailRepo userdetailrepo = new UserDetailRepo(context);
            SuperAdminDashboardController controller = new SuperAdminDashboardController(userdetailrepo, bookingdetailrepo, chargingstationrepo);

            // Act
            var Admins = controller.GetAllAdminDetails().Result;
            var AdminCount = Admins.Count;

            // Assert
            Assert.AreEqual(2, AdminCount);

        }

    }
}
