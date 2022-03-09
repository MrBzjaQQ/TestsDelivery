using System.Data;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace TestsDelivery.Infrastructure.Logging
{
    public static class LoggingConfiguration
    {
        private static readonly string OutputTemplate = @"[{Timestamp:yy-MM-dd HH:mm:ss} {Level}]{ApplicationName}:
            {SourceContext}{NewLine} Message:{Message}{NewLine}in method {MemberName} at {FilePath}:{NewLine}{Exception}{NewLine}";

        private static readonly ColumnOptions ColumnOptions = new()
        {
            AdditionalColumns = new List<SqlColumn>
            {
                new()
                {
                    DataType = SqlDbType.VarChar,
                    ColumnName = "ApplicationName"
                },
                new()
                {
                    DataType = SqlDbType.VarChar,
                    ColumnName = "MachineName"
                },
                new()
                {
                    DataType = SqlDbType.VarChar,
                    ColumnName = "MemberName"
                },
                new()
                {
                    DataType = SqlDbType.VarChar,
                    ColumnName = "FilePath"
                },
                new()
                {
                    DataType = SqlDbType.Int,
                    ColumnName = "LineNumber"
                },
                new()
                {
                    DataType = SqlDbType.VarChar,
                    ColumnName = "SourceContext"
                },
                new()
                {
                    DataType = SqlDbType.VarChar,
                    ColumnName = "RequestPath"
                },
                new()
                {
                    DataType = SqlDbType.VarChar,
                    ColumnName = "ActionName"
                },
            }
        };

        public static IHostBuilder ConfigureSerilog(this IHostBuilder builder)
        {
            builder.ConfigureLogging((context, logging) =>
            {
                logging.ClearProviders();
            }).UseSerilog((hostingContext, loggerConfiguration) =>
            {
                var config = hostingContext.Configuration;
                var connectionString = config.GetConnectionString("TestsDeliveryConnection").ToString(CultureInfo.InvariantCulture);

                var tableName = config["Logging:MSSqlServer:tableName"].ToString(CultureInfo.InvariantCulture);
                var schema = config["Logging:MSSqlServer:schema"].ToString(CultureInfo.InvariantCulture);
                string restrictedToMinimumLevel = config["Logging:MSSqlServer:restrictedToMinimumLevel"].ToString(CultureInfo.InvariantCulture);

                if (!Enum.TryParse(restrictedToMinimumLevel, out LogEventLevel logLevel))
                {
                    logLevel = LogEventLevel.Debug;
                }

                var level = (LogEventLevel)Enum.Parse(typeof(LogEventLevel), restrictedToMinimumLevel);

                var sqlOptions = new MSSqlServerSinkOptions
                {
                    AutoCreateSqlTable = true,
                    SchemaName = schema,
                    TableName = tableName
                };

                if (hostingContext.HostingEnvironment.IsDevelopment())
                {
                    sqlOptions.BatchPeriod = new TimeSpan(0, 0, 0, 1);
                    sqlOptions.BatchPostingLimit = 1;
                }

                loggerConfiguration
                    .Enrich.FromLogContext()
                    .WriteTo.File(
                        config.GetValue<string>("LoggingPath"),
                        rollingInterval: RollingInterval.Day,
                        restrictedToMinimumLevel: logLevel,
                        outputTemplate: OutputTemplate)
                    .WriteTo.Console(logLevel)
                    .WriteTo.MSSqlServer(
                        connectionString,
                        sqlOptions,
                        restrictedToMinimumLevel: level,
                        columnOptions: ColumnOptions);
            });

            return builder;
        }
    }
}
