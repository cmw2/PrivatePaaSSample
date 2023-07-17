using System.Text;

namespace TestConnectionsApp.Util
{
    public class MaskPassword
    {
        public static string Mask(string connectionString, string firstSeparator = ";", string secondSeparator = "=")
        {
            var parts = connectionString.Split(firstSeparator);
            var newConnectionString = new StringBuilder();
            foreach (var part in parts)
            {
                var kv = part.Split(secondSeparator);
                if (kv.Length >= 2 && kv[0].ToLower() == "password")
                {
                    newConnectionString.Append(kv[0]);
                    newConnectionString.Append(secondSeparator + "********");
                }
                else
                {
                    newConnectionString.Append(part);
                }
                newConnectionString.Append(firstSeparator);
            }

            return newConnectionString.ToString();
        }

    }
}
