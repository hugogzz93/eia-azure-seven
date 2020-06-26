using System;
using System.IO;
using System.Xml;
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
                    var text = "exec GET_PEOPLE_XML";

                    using (SqlCommand cmd = new SqlCommand(text, conn))
                    {
                        log.LogInformation("Querying");

                    XmlReader reader = await cmd.ExecuteXmlReaderAsync();
                        while(reader.Read()) {
                            log.LogInformation(String.Format("Name: {0}, Id: {1}", reader.GetAttribute("name"), reader.GetAttribute("id")));
                        }

                        reader.Close();
                    }
                    conn.Close();
                    return new OkObjectResult("All Good!");
                }
        }
    }
}