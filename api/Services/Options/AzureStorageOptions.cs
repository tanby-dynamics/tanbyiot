namespace Shared.Services.Options;

public class AzureStorageOptions
{
    public const string Key = "AzureStorage";

    public string ConnectionString { get; set; } = string.Empty;

    public void Verify()
    {
        if (string.IsNullOrEmpty(ConnectionString))
        {
            throw new ArgumentException("ConnectionString is empty");
        }
    }
}