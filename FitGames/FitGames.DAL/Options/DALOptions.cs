namespace CookBook.DAL.Options;

public record DALOptions
{
    public string DatabaseDirectory { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string DatabaseFilePath => Path.Combine(DatabaseDirectory, DatabaseName);

    /// <summary>
    /// Seeds DemoData from DbContext on database creation.
    /// </summary>
    public bool SeedDemoData { get; set; } = false;
}
