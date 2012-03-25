namespace Sitereactor.CloudProviders.Extensions
{
    public static class IntegerExtensions
    {
        public static int ToHours(this int seconds)
        {
            var hours = (seconds/60) / 60;
            return hours;
        }

        public static long ToHours(this long seconds)
        {
            var hours = (seconds / 60) / 60;
            return hours;
        }
    }
}
