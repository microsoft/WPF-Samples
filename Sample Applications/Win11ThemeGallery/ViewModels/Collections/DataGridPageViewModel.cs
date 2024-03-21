using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using Win11ThemeGallery.Models;

namespace Win11ThemeGallery.ViewModels;

public partial class DataGridPageViewModel : ObservableObject 
{
	[ObservableProperty]
	private string _pageTitle = "DataGrid";

	[ObservableProperty]
	private string _pageDescription = "";

    [ObservableProperty]
    private ObservableCollection<Product> _productsCollection;

    public DataGridPageViewModel()
    {
        _productsCollection = GenerateProducts();
    }

    private ObservableCollection<Product> GenerateProducts()
    {
        var random = new Random();
        var products = new ObservableCollection<Product> { };

        var adjectives = new[] { "Red", "Blueberry" };
        var names = new[] { "Marmalade", "Dumplings", "Soup" };
        var units = new[] { "grams", "kilograms", "milliliters" };

        for (int i = 0; i < 50; i++)
        {
            products.Add(
                new Product
                {
                    ProductId = i,
                    ProductCode = i,
                    ProductName =
                        adjectives[random.Next(0, adjectives.Length)]
                        + " "
                        + names[random.Next(0, names.Length)],
                    UnitPrice = Math.Round(random.NextDouble() * 20.0, 3),
                    UnitsInStock = random.Next(0, 100),
                    IsVirtual = random.Next(0, 2) == 1
                }
            );
        }

        return products;
    }    
}