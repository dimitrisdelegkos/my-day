namespace MyDay.Integrations.Helpers
{
    public class IntegrationHelper
    {
        public static string GetCorrelationId() => Guid.NewGuid().ToString().Trim('{', '}').Replace("-", "");
    }
}
