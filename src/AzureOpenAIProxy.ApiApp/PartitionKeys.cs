namespace AzureOpenAIProxy.ApiApp;

/// <summary>
/// This represents the partition keys for azure table storage
/// </summary>
public class PartitionKeys
{
    /// <summary>
    /// Partition key for event details
    /// </summary>
    public const string EventDetails = "event-details";

    /// <summary>
    /// Partition key for resource details
    /// </summary>
    public const string ResourceDetails = "resource-details";
}