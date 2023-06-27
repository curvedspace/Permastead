namespace Models;

/// <summary>
/// For storing people's contact information like email, phone numbers, aliases.
/// </summary>
public class ContactInformation
{
    public ContactInformationType? Type { get; set; }

    public string? Value { get; set; }
}