namespace MyDay.Core.Helpers
{
    public class ValuesHelper
    {
        public static string GetCorrelationId() => Guid.NewGuid().ToString().Trim('{', '}').Replace("-", "");
    }
}
