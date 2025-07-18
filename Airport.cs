// Airport.cs
public class Airport
{
    public string Code { get; set; }
    public string Name { get; set; }

    public Airport(string code, string name)
    {
        Code = code;
        Name = name;
    }

    public override string ToString()
    {
        return $"{Code} - {Name}";
    }
}