using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using Win11ThemeGallery.Models;

namespace Win11ThemeGallery.ViewModels;

public partial class ListViewPageViewModel : ObservableObject 
{
	[ObservableProperty]
	private string _pageTitle = "ListView";

	[ObservableProperty]
	private string _pageDescription = "";

    private int _listViewSelectionModeComboBoxSelectedIndex = 0;

    public int ListViewSelectionModeComboBoxSelectedIndex
    {
        get => _listViewSelectionModeComboBoxSelectedIndex;
        set
        {
            SetProperty<int>(ref _listViewSelectionModeComboBoxSelectedIndex, value);
            UpdateListViewSelectionMode(value);
        }
    }

    [ObservableProperty]
    private SelectionMode _listViewSelectionMode = SelectionMode.Single;

    [ObservableProperty]
    private ObservableCollection<Person> _basicListViewItems;

    public ListViewPageViewModel()
    {
        _basicListViewItems = GeneratePersons();
    }

    private ObservableCollection<Person> GeneratePersons()
    {
        var random = new Random();
        var persons = new ObservableCollection<Person>();

        var names = new[]
        {
            "John",
            "Winston",
            "Adrianna",
            "Spencer",
            "Phoebe",
            "Lucas",
            "Carl",
            "Marissa",
            "Brandon",
            "Antoine",
            "Arielle",
            "Arielle",
            "Jamie",
            "Alexzander"
        };
        var surnames = new[]
        {
            "Doe",
            "Tapia",
            "Cisneros",
            "Lynch",
            "Munoz",
            "Marsh",
            "Hudson",
            "Bartlett",
            "Gregory",
            "Banks",
            "Hood",
            "Fry",
            "Carroll"
        };
        var companies = new[]
        {
            "Pineapple Inc.",
            "Macrosoft Redmond",
            "Amazing Basics Ltd",
            "Megabyte Computers Inc",
            "Roude Mics",
            "XD Projekt Red S.A.",
            "Lepo.co"
        };

        for (int i = 0; i < 50; i++)
            persons.Add(
                new Person(
                    names[random.Next(0, names.Length)],
                    surnames[random.Next(0, surnames.Length)],
                    companies[random.Next(0, companies.Length)]
                )
            );

        return persons;
    }

    private void UpdateListViewSelectionMode(int selectionModeIndex)
    {
        ListViewSelectionMode = selectionModeIndex switch
        {
            1 => SelectionMode.Multiple,
            2 => SelectionMode.Extended,
            _ => SelectionMode.Single
        };
    }
}
