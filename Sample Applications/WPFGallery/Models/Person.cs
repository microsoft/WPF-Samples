// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

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
