using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PizzaWebshopCore2.Models.Dishes
{
    public class IndexViewModel
    {
        public IReadOnlyList<DishModel> Dishes { get; set; }
        public IReadOnlyList<IngredientModel> Ingredients { get; set; }
        public PaymentInformationModel PaymentInformationModel { get; set; }
    }

    public class PaymentInformationModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        [Required]
        public string CardNumber { get; set; }
    }
}
