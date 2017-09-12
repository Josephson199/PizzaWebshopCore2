using System.Linq;
using Moq;
using PizzaWebshopCore2.Models.Entities;
using PizzaWebshopCore2.Services;
using Xunit;

namespace PizzaWebshopCore2Tests
{
    public class DatabaseServiceTests
    {
        [Fact]
        public void Test_that_generic_get_method_returns_query_of_correct_type()
        {
            //arrange
            var mockDbService = new Mock<IDatabaseService>();

            //act
            var res = mockDbService.Object.GetAll<Dish>();

            //assert
            Assert.IsType<EnumerableQuery<Dish>>(res);
        }
    }
}
