using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestConnectionsApp.Redis;
using TestConnectionsApp.Util;

namespace TestConnectionsApp.Pages
{
    public class AzureRedisTestModel : PageModel
    {
        private readonly ILogger<AzureRedisTestModel> _logger;
        private readonly IConfiguration _configuration;
        private readonly Task<RedisConnection> _redisConnectionFactory;
        
        public Dictionary<string, string> RedisMessages = new Dictionary<string, string>();

        public AzureRedisTestModel(ILogger<AzureRedisTestModel> logger, IConfiguration configuration, Task<RedisConnection> redisConnectionFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _redisConnectionFactory = redisConnectionFactory;
        }

        public async Task OnGetAsync()
        {
            RedisMessages.Add("Connection String", MaskPassword.Mask(_configuration["CacheConnection"], ","));

            string connectCommand = "Create Connection";
            try
            {
                RedisConnection redisConnection = await _redisConnectionFactory;
                RedisMessages.Add(connectCommand, "Success!");

                string pingCommand = "PING";
                try
                {
                    string? pingResult = (await redisConnection.BasicRetryAsync(async (db) => await db.ExecuteAsync(pingCommand))).ToString();
                    RedisMessages.Add(pingCommand, pingResult + "");
                }
                catch (Exception ex)
                {
                    RedisMessages.Add(pingCommand, "Failed: " + ex.Message);
                }
                
                // Simple get and put of integral data types into the cache
                string key = "Message";
                string value = "Hello! The cache is working from ASP.NET Core!";

                string command2 = $"SET {key} \"{value}\"";
                try
                {
                    string? command2Result = (await redisConnection.BasicRetryAsync(async (db) => await db.StringSetAsync(key, value))).ToString();
                    RedisMessages.Add(command2, command2Result + "");
                }
                catch (Exception ex)
                {
                    RedisMessages.Add(command2, "Failed: " + ex.Message);
                }

                string command3 = $"GET {key}";
                try
                {                    
                    string? command3Result = (await redisConnection.BasicRetryAsync(async (db) => await db.StringGetAsync(key))).ToString();
                    RedisMessages.Add(command3, command3Result + "");
                }
                catch (Exception ex)
                {
                    RedisMessages.Add(command3, "Failed: " + ex.Message);
                }

                key = "LastUpdateTime";
                value = DateTime.UtcNow.ToString();

                string command4 = $"GET {key}";
                try
                {
                    string? command4Result = (await redisConnection.BasicRetryAsync(async (db) => await db.StringGetAsync(key))).ToString();
                    RedisMessages.Add(command4, command4Result + "");
                }
                catch (Exception ex)
                {
                    RedisMessages.Add(command4, "Failed: " + ex.Message);
                }

                string command5 = $"SET {key}";
                try
                {
                    string command5Result = (await redisConnection.BasicRetryAsync(async (db) => await db.StringSetAsync(key, value))).ToString();
                    RedisMessages.Add(command5, command5Result + "");
                }
                catch (Exception ex)
                {
                    RedisMessages.Add(command5, "Failed: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                RedisMessages.Add(connectCommand, "Unable to create connection! Message: " + ex.Message);
            }
            
        }

    }
}
