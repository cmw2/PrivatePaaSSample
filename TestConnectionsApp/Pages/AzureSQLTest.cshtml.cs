using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Text;
using TestConnectionsApp.Util;

namespace TestConnectionsApp.Pages
{
    public class AzureSQLTestModel : PageModel
    {
        private IConfiguration Configuration;

        public Dictionary<string, string> SqlMessages = new Dictionary<string, string>();

        public AzureSQLTestModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void OnGet()
        {
            string connectionString = Configuration.GetConnectionString("SampleDB");
            SqlMessages.Add("Connection String", MaskPassword.Mask(connectionString));

            string command = "Create Connection Class";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlMessages.Add(command, "Success!");

                    string command1 = "Connect";
                    try
                    {
                        connection.Open();
                        SqlMessages.Add(command1, "Success!");
                    }
                    catch (Exception ex)
                    {
                        SqlMessages.Add(command1, "Failed: " + ex.Message);                        
                    }

                    string command2 = "Get Table Count";
                    try
                    {
                        var cmd = connection.CreateCommand();
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "select count(*) as [tables] from sys.tables";
                        int tableCount = (int) cmd.ExecuteScalar();
                        SqlMessages.Add(command2, $"Number of tables = {tableCount}");                        
                    }
                    catch (Exception ex)
                    {
                        SqlMessages.Add(command, "Failed: " + ex.Message);
                    }

                }
            }
            catch (Exception ex)
            {
                SqlMessages.Add(command, "Failed: " + ex.Message);                
            }
        }
    }
}
