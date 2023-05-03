using Ci_Platform.Repositories.Interfaces;
using CI_Platform_web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NUnit
{
    [TestClass]
    public class HomeControllerTest
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAuthentication _Authentication;
        private readonly IConfiguration _configuration;
        private readonly IFilters _filters;


        public HomeControllerTest(ILogger<HomeController> logger, IAuthentication authentication, IConfiguration config, IFilters filters)
        {
            _logger = logger;
            _Authentication = authentication;
            _configuration = config;
            _filters = filters;
        }
        [TestMethod]
        public void Index()
        {
            // arrange
            HomeController controller = new(_logger, _Authentication, _configuration, _filters);

            // act
            ViewResult result = controller.Index() as ViewResult;

            // assert
            Assert.IsNotNull(result);
        }
    }
}