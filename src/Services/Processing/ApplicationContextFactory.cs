using Data;

namespace Services.Processing;

public interface IApplicationContextFactory
{
    ApplicationContext CreateNew();
}

public class ApplicationContextFactory : IApplicationContextFactory
{
    public ApplicationContext CreateNew()
    {
        return new ApplicationContext();
    }
}

public class ApplicationContext()
{
    public Telemetry? CurrentTelemetry { get; set; }
}