using System;
using System.IO;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EIA.SEVEN_XML
{
    public static class CSharp_Seven_XML
    {
        [FunctionName("CSharp_Seven_XML")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
                log.LogInformation("Triggered");
                var str = Environment.GetEnvironmentVariable("sqldb_connection");
                var connString = Environment.GetEnvironmentVariable("sqldb_connection_enc");
                using (SqlConnection conn = new SqlConnection(str))
                {
                    log.LogInformation("Connecting");
                    log.LogInformation($"Connecting using {str}");
                    log.LogInformation($"But could use {connString}");
                    log.LogInformation(str);
                    log.LogInformation(connString);
                    conn.Open();
                    log.LogInformation("Connected");
                    var text = "select id, name from Person";

                    using (SqlCommand cmd = new SqlCommand(text, conn))
                    {
                        log.LogInformation("Querying");
                        var reader = await cmd.ExecuteReaderAsync();
                        while (reader.Read()) {
                            log.LogInformation(String.Format("id: {0}, name: {1}",
                                reader[0], reader[1]));
                        }
                        log.LogInformation("Querying");

                    }
                    conn.Close();
                    return new OkObjectResult("All Good!");
                }
        }
    }
}