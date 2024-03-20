
namespace Win11ThemeGallery.Models;

public class Product
{
    public int ProductId { get; set; }

    public int ProductCode { get; set; }

    public string ProductName { get; set; }

    public string QuantityPerUnit { get; set; }

    public double UnitPrice { get; set; }

    public string UnitPriceString => UnitPrice.ToString("F2");

    public int UnitsInStock { get; set; }

    public bool IsVirtual { get; set; }
}
