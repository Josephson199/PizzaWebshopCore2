using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PizzaWebshopCore2.Controllers;
using PizzaWebshopCore2.Data;
using Xunit;

namespace PizzaWebshopCore2Tests
{
    public class ManageControllerTests
    {
        [Fact]
        public void Test()
        {
            //arrange
            var mockRepo = new Mock<ApplicationDbContext>();
            
            var sut = new ManageController(mockRepo.Object);
           
            //act
            var result = sut.Index();
            //assert
            Assert.IsType<ViewResult>(result);

        }
    }
}
