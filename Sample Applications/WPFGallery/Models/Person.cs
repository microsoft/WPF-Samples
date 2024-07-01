
namespace WPFGallery.Models;

/// <summary>
/// Person class for User Dashboard page
/// </summary>
public record Person
{
    public string FirstName { get; init; }

    public string LastName { get; init; }

    public string Name => FirstName + " " + LastName;

    public string Company { get; init; }

    public Person(string firstName, string lastName, string company)
    {
        FirstName = firstName;
        LastName = lastName;
        Company = company;
    }
}
