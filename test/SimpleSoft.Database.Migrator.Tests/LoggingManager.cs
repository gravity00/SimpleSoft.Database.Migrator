using System;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace SimpleSoft.Database.Migrator.Tests
{
    public static class LoggingManager
    {
        public static readonly IMigrationLoggerFactory LoggerFactory;
        
        static LoggingManager()
        {
            #region Microsoft Logging Test
            LoggerFactory = new MicrosoftMigrationLoggerFactory(
                new LoggerFactory()
                    .AddConsole(Microsoft.Extensions.Logging.LogLevel.Trace, true)
                    .AddDebug(Microsoft.Extensions.Logging.LogLevel.Trace));

            #endregion

            #region NLog Test
            DebuggerTarget debugTarget = new DebuggerTarget();
            debugTarget.Layout = "[${longdate}] [${uppercase:${level}}] [${logger}] [${gdc:item=assemblyVersion}] ${newline} [${ndlc}] ${newline} ${message} ${newline} ${exception:format=tostring}";

            var config = new NLog.Config.LoggingConfiguration();
            config.AddTarget("1", debugTarget);

            LoggingRule rule1 = new LoggingRule("*", NLog.LogLevel.Debug, debugTarget);
            LoggingRule rule2 = new LoggingRule("*", NLog.LogLevel.Info, debugTarget);
            LoggingRule rule3 = new LoggingRule("*", NLog.LogLevel.Error, debugTarget);
            LoggingRule rule4 = new LoggingRule("*", NLog.LogLevel.Fatal, debugTarget);
            LoggingRule rule5 = new LoggingRule("*", NLog.LogLevel.Trace, debugTarget);
            LoggingRule rule6 = new LoggingRule("*", NLog.LogLevel.Warn, debugTarget);
            LoggingRule rule7 = new LoggingRule("*", NLog.LogLevel.Off, debugTarget);
            config.LoggingRules.Add(rule1);
            config.LoggingRules.Add(rule2);
            config.LoggingRules.Add(rule3);
            config.LoggingRules.Add(rule4);
            config.LoggingRules.Add(rule5);
            config.LoggingRules.Add(rule6);
            config.LoggingRules.Add(rule7);

            LoggerFactory = new SimpleSoft.Database.Migrator.NLogMigrationLoggerFactory(
                new LogFactory(config)
                );

            #endregion
        }
    }
}
