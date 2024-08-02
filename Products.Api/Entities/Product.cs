using System.ComponentModel.DataAnnotations;

namespace Products.Api.Entities
{
    public class Product
    {
        public Product(string name, decimal price)
        {
            Id = Guid.NewGuid();
            Name = name;
            Price = price;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }

        public void AtualizarProduto(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
    }
}
