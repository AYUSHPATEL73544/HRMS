namespace Hrms.Core.Utilities
{
    public static class Utility
    {
        public static DateTime GetDateTime()
        {
            return DateTime.UtcNow;
        }

        public static string GetRandomCode(int length = 4)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            var random = new Random();
            var result = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            return result;
        }
    }
}
