namespace YC.Result;

public readonly record struct Error : IEquatable<Error>
{
    public string Title { get; }
    public string Detail { get; }
    public int Status { get; }

    private Error(string title, string detail, int status)
    {
        Title = title;
        Detail = detail;
        Status = status;
    }
    
    public bool Equals(Error? other)
    {
        if (other is null)
        {
            return false;
        }

        return Title == other.Value.Title && Detail == other.Value.Detail && Status == other.Value.Status;
    }

    public static Error Create(string title, string detail, int status = 400) => new(title, detail, status);

    public static Error Create(string detail) => new(string.Empty, detail, 400);

    public static Error Create(int status = 400) => new(string.Empty, string.Empty, status);


    public static readonly Error None = new(string.Empty, string.Empty, 0);


}