using Products.Api.Entities;

namespace Products.Api.Dtos.Responses
{
    public class ProductResponse
    {
        public ProductResponse(Guid id, string name, decimal price)
        {
            Id = id.ToString();
            Name = name;
            Price = price;
        }

        public string Id { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public decimal Price { get; private set; }

        public static ProductResponse FromtEntity(Product product) =>
            new ProductResponse(product.Id, product.Name, product.Price);
    }
}
