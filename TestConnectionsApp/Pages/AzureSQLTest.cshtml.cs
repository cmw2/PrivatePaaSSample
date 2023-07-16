using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Text;

namespace TestConnectionsApp.Pages
{
    public class AzureSQLTestModel : PageModel
    {
        private IConfiguration Configuration;
        
        public AzureSQLTestModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void OnGet()
        {
            try
            {
                using (SqlConnection connection =
                new SqlConnection(Configuration.GetConnectionString("SampleDB")))
                {
                    ViewData["ConnectionMessage"] = "Going to connect to : " + maskPassword(connection.ConnectionString);

                    try
                    {
                        connection.Open();
                        ViewData["ConnectMessage"] = "Connection opened successfully";
                    }
                    catch (Exception ex)
                    {
                        ViewData["ConnectMessage"] = "Unable to open connection! Exception: " + ex.Message;
                    }

                    try
                    {
                        var cmd = connection.CreateCommand();
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "select count(*) as [tables] from sys.tables";
                        int tableCount = (int) cmd.ExecuteScalar();
                        ViewData["TableMessage"] = $"Number of tables = {tableCount}";
                    }
                    catch (Exception ex)
                    {
                        ViewData["TableMessage"] = "Unable to check number of tables! Exception: " + ex.Message;
                    }

                }
            }
            catch (Exception ex)
            {
                ViewData["ConnectionMessage"] = "Unable to create connection! Exception: " + ex.Message;
            }
        }
        private string maskPassword(string connectionString)
        {
            var parts = connectionString.Split(";");
            var newConnectionString = new StringBuilder();
            foreach (var part in parts)
            {
                var kv = part.Split("=");
                if (kv.Length == 2 && kv[0].ToLower() == "password")
                {
                    newConnectionString.Append(kv[0]);
                    newConnectionString.Append("=********");
                }
                else
                {
                    newConnectionString.Append(part);
                }
                newConnectionString.Append(";");
            }
            
            return newConnectionString.ToString();
        }
    }
}
