using Microsoft.AspNetCore.Mvc;
using PizzaWebshopCore2.Controllers;
using Xunit;

namespace PizzaWebshopCore2Tests
{
    public class HomerControllerTests
    {
        [Fact]
        public void HomeControllerTest()
        {
            // Arrange
            var controller = new HomeController();
            
            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
            
        }
    }
}
