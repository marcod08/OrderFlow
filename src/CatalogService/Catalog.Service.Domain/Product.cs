namespace Catalog.Service.Domain;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = default!;
    public decimal Price { get; private set; }
    public int StockQuantity { get; private set; }

    private Product() { }

    public Product(string name, decimal price, int stockQuantity)
    {
        if(string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Product name cannot be null or empty.", nameof(name));
        }

        if (price < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative.");
        }

        if (stockQuantity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(stockQuantity), "Stock quantity cannot be negative.");
        }

        Id = Guid.NewGuid();
        Name = name;
        Price = price;
        StockQuantity = stockQuantity;
    }

    public void ReserveStock(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        }

        if (quantity > StockQuantity)
        {
            throw new InvalidOperationException("Not enough stock available.");
        }

        StockQuantity -= quantity;
    }

    public void ReleaseStock(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        }

        StockQuantity += quantity;
    }

}