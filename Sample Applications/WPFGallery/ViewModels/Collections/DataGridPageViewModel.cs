// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using WPFGallery.Models;

namespace WPFGallery.ViewModels;

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
        //var units = new[] { "grams", "kilograms", "milliliters" };

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
                    UnitsInStock = random.Next(0, 100)
                }
            );
        }

        return products;
    }    
}