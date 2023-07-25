using System.Text;

namespace TestConnectionsApp.Util
{
    public class MaskPassword
    {
        /// <summary>
        /// Masks the password in the given connection string by replacing it with asterisks.
        /// </summary>
        /// <param name="connectionString">The connection string to mask.</param>
        /// <param name="firstSeparator">The separator used to split the connection string into parts. Default is ";"</param>
        /// <param name="secondSeparator">The separator used to split each part into key-value pairs. Default is "="</param>
        /// <returns>The masked connection string.</returns>
        public static string Mask(string connectionString, string firstSeparator = ";", string secondSeparator = "=")
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                return "";
            }
            
            var parts = connectionString.Split(firstSeparator);
            var newConnectionString = new StringBuilder();
            foreach (var part in parts)
            {
                if (string.IsNullOrEmpty(part))
                {
                    continue;
                }

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
