using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using Serilog.Core.Enrichers;

namespace AdminPanel.Logging
{
    public class AppLogging<T> : IAppLogging<T>
    {
        private readonly ILogger<T> _logger;
        private readonly IConfiguration _config;
        private readonly string _applicationName;

        public AppLogging(ILogger<T> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _applicationName = config.GetValue<string>("ApplicationName");
        }

        public void LogAppError(
            Exception exception,
            string message,
            string memberName = "",
            string sourceFilePath = "",
            int sourceLineNumber = 0)
        {
            var list = PushProperties(memberName, sourceFilePath, sourceLineNumber);
            _logger.LogError(exception, message);

            foreach (var item in list)
            {
                item.Dispose();
            }
        }

        public void LogAppError(
            string message,
            string memberName = "",
            string sourceFilePath = "",
            int sourceLineNumber = 0)
        {
            var list = PushProperties(memberName, sourceFilePath, sourceLineNumber);
            _logger.LogError(message);

            foreach (var item in list)
            {
                item.Dispose();
            }
        }

        public void LogAppCritical(
            Exception exception,
            string message,
            string memberName = "",
            string sourceFilePath = "",
            int sourceLineNumber = 0)
        {
            var list = PushProperties(memberName, sourceFilePath, sourceLineNumber);
            _logger.LogCritical(exception, message);

            foreach (var item in list)
            {
                item.Dispose();
            }
        }

        public void LogAppCritical(
            string message,
            string memberName = "",
            string sourceFilePath = "",
            int sourceLineNumber = 0)
        {
            var list = PushProperties(memberName, sourceFilePath, sourceLineNumber);
            _logger.LogCritical(message);

            foreach (var item in list)
            {
                item.Dispose();
            }
        }

        public void LogAppDebug(
            string message,
            string memberName = "",
            string sourceFilePath = "",
            int sourceLineNumber = 0)
        {
            var list = PushProperties(memberName, sourceFilePath, sourceLineNumber);
            _logger.LogDebug(message);

            foreach (var item in list)
            {
                item.Dispose();
            }
        }

        public void LogAppTrace(
            string message,
            string memberName = "",
            string sourceFilePath = "",
            int sourceLineNumber = 0)
        {
            var list = PushProperties(memberName, sourceFilePath, sourceLineNumber);
            _logger.LogTrace(message);

            foreach (var item in list)
            {
                item.Dispose();
            }
        }

        public void LogAppInformation(
            string message,
            string memberName = "",
            string sourceFilePath = "",
            int sourceLineNumber = 0)
        {
            var list = PushProperties(memberName, sourceFilePath, sourceLineNumber);
            _logger.LogInformation(message);

            foreach (var item in list)
            {
                item.Dispose();
            }
        }

        public void LogAppWarning(
            string message,
            string memberName = "",
            string sourceFilePath = "",
            int sourceLineNumber = 0)
        {
            var list = PushProperties(memberName, sourceFilePath, sourceLineNumber);
            _logger.LogWarning(message);

            foreach (var item in list)
            {
                item.Dispose();
            }
        }

        internal List<IDisposable> PushProperties(
            string memberName,
            string sourceFilePath,
            int sourceLineNumber)
        {
            return new()
            {
                LogContext.Push(new PropertyEnricher("MemberName", memberName)),
                LogContext.Push(new PropertyEnricher("FilePath", sourceFilePath)),
                LogContext.Push(new PropertyEnricher("LineNumber", sourceLineNumber)),
                LogContext.Push(new PropertyEnricher("ApplicationName", _applicationName))
            };
        }
    }
}
