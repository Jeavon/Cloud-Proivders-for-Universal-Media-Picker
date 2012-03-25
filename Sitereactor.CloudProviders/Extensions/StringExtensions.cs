namespace Sitereactor.CloudProviders.Extensions
{
    public static class StringExtensions
    {
        public static string AddDefaultAlias(this string alias)
        {
            return string.Concat(alias, "uCloudAlias");
        }
    }
}
