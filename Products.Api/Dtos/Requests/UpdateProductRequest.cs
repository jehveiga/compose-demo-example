using Products.Api.Entities;
using System.ComponentModel.DataAnnotations;

namespace Products.Api.Dtos.Requests
{
    public class UpdateProductRequest
    {
        public UpdateProductRequest(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        [Required]
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public Product ToEntity() =>
            new Product(Name, Price);
    }
}
