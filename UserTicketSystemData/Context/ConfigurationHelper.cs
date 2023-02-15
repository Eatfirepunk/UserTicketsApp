using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.IO;


public static class ConfigurationHelper
{
    public static IConfigurationRoot Configuration { get; set; }

    static ConfigurationHelper()
    {
        // This will try to retrieve the config from the json file that uses this class library
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        Configuration = builder.Build();
    }

    //default to my localhost for testing purposes
    public static string GetConnectionString(string name)
    {
        string connectionString = Configuration.GetConnectionString(name);

        return connectionString ?? "Server=MSIGE63\\SQLEXPRESS;Database=UserTicketSystem;Trusted_Connection=True;User Id=MSIGE63\\admin;";
    }
}
