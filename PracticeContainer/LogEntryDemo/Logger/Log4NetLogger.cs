using System;
using System.Diagnostics;
using log4net;
using log4net.Config;
using log4net.Core;
using Newtonsoft.Json;

namespace LogEntryDemo.Logger
{
    public class Log4NetLogger : ILogger
    {
        private readonly string _mainLogName = "Main";
        private static readonly Level AuditLevel = new Level(32500, "AUDIT");
        private static readonly Level DiagnosticLevel = new Level(35000, "DIAGNOSTIC");
        private static readonly Level FunnelLevel = new Level(50000, "FUNNEL");

        protected virtual string UserName
        {
            get { return ""; }
        }

        public Log4NetLogger()
            : this("Main")
        {
        }

        public Log4NetLogger(string logName)
        {
            if (!string.IsNullOrEmpty(logName))
            {
                _mainLogName = logName;
            }

            XmlConfigurator.Configure();
            LogManager.GetRepository().LevelMap.Add(AuditLevel);
            LogManager.GetRepository().LevelMap.Add(DiagnosticLevel);
            LogManager.GetRepository().LevelMap.Add(FunnelLevel);
        }

        public void Audit(string message)
        {

            var log = LogManager.GetLogger(_mainLogName);

            if (!log.IsInfoEnabled)
                return;

            LogicalThreadContext.Properties["Class"] = "Unidentified";
            LogicalThreadContext.Properties["Method"] = "Unidentified";
            LogicalThreadContext.Properties["ElapsedTicks"] = "Unidentified";

            message = UserName + message;

            log.Info(message);
        }

        public void Audit(object message)
        {
            var log = LogManager.GetLogger(_mainLogName);

            if (!log.IsInfoEnabled)
                return;

            LogicalThreadContext.Properties["Class"] = "Unidentified";
            LogicalThreadContext.Properties["Method"] = "Unidentified";
            LogicalThreadContext.Properties["ElapsedTicks"] = "Unidentified";

            var infoMessage = new { UserName, Message = message };

            log.Info(infoMessage);
        }

        public void Critical(string message, Exception exception = null)
        {
            var log = LogManager.GetLogger(_mainLogName);

            if (!log.IsFatalEnabled)
                return;

            LogicalThreadContext.Properties["Class"] = "Unidentified";
            LogicalThreadContext.Properties["Method"] = "Unidentified";
            LogicalThreadContext.Properties["ElapsedTicks"] = "Unidentified";
            message = UserName + message;

            log.Fatal(message, exception);
        }

        public void Critical(object message, Exception exception = null)
        {
            var log = LogManager.GetLogger(_mainLogName);

            if (!log.IsFatalEnabled)
                return;

            LogicalThreadContext.Properties["Class"] = "Unidentified";
            LogicalThreadContext.Properties["Method"] = "Unidentified";
            LogicalThreadContext.Properties["ElapsedTicks"] = "Unidentified";
            var critMessage = new { UserName, Message = message };

            log.Fatal(critMessage, exception);
        }

        public void Error(string message, Exception exception = null)
        {
            var log = LogManager.GetLogger(_mainLogName);

            if (!log.IsErrorEnabled)
                return;

            LogicalThreadContext.Properties["Class"] = "Unidentified";
            LogicalThreadContext.Properties["Method"] = "Unidentified";
            LogicalThreadContext.Properties["ElapsedTicks"] = "Unidentified";
            message = UserName + message;

            log.Error(message, exception);
        }

        public void Error(object message, Exception exception = null)
        {
            var log = LogManager.GetLogger(_mainLogName);

            if (!log.IsErrorEnabled)
                return;

            LogicalThreadContext.Properties["Class"] = "Unidentified";
            LogicalThreadContext.Properties["Method"] = "Unidentified";
            LogicalThreadContext.Properties["ElapsedTicks"] = "Unidentified";
            var errorMessage = new { UserName, Message = message };

            log.Error(errorMessage, exception);
        }

        public void Informational(string message)
        {
            var log = LogManager.GetLogger(_mainLogName);

            if (!log.IsInfoEnabled)
                return;

            LogicalThreadContext.Properties["Class"] = "Unidentified";
            LogicalThreadContext.Properties["Method"] = "Unidentified";
            LogicalThreadContext.Properties["ElapsedTicks"] = "Unidentified";
            message = UserName + message;

            log.Info(message);
        }

        public void Informational(object message)
        {
            var log = LogManager.GetLogger(_mainLogName);

            if (!log.IsInfoEnabled)
                return;

            LogicalThreadContext.Properties["Class"] = "Unidentified";
            LogicalThreadContext.Properties["Method"] = "Unidentified";
            LogicalThreadContext.Properties["ElapsedTicks"] = "Unidentified";
            var infoMessage = new { UserName, Message = message };

            log.Info(infoMessage);
        }

        public void LogDetailedInfoExperimentally(string message)
        {
            var log = LogManager.GetLogger(_mainLogName);

            if (!log.IsInfoEnabled)
                return;

            CallingMethodInfo callingMethodInfo = GetCallingMethodInfo();
            LogicalThreadContext.Properties["Class"] = callingMethodInfo.ClassName;
            LogicalThreadContext.Properties["Method"] = callingMethodInfo.MethodName;
            LogicalThreadContext.Properties["ElapsedTicks"] = callingMethodInfo.ElapsedTicks;

            message = "Experimental log with class and method " + UserName + message;

            log.Info(message);
        }

        public string InformationalJson(String identifyingString, int userId, Object objectToLog)
        {
            return InformationalJson(identifyingString, userId, objectToLog, this);

        }

        /// <summary>
        /// This method serializes objectToLog to a json and concatenates the json with some identifying information. It then logs the concatenated string.
        /// </summary>
        /// <param name="identifyingString"></param>
        /// <param name="userId"></param>
        /// <param name="objectToLog">the object to be serialized and logged.</param>
        /// <param name="internalLogger"> The internal logger is there to help with testing.</param>
        /// <returns></returns>
        public string InformationalJson(String identifyingString, int userId, Object objectToLog, ILogger internalLogger)
        {
            StackTrace currentStackTrace = new StackTrace();

            try
            {
                String jsonString = JsonConvert.SerializeObject(objectToLog);
                LogicalThreadContext.Properties["Class"] = "Unidentified";
                LogicalThreadContext.Properties["Method"] = "Unidentified";
                LogicalThreadContext.Properties["ElapsedTicks"] = "Unidentified";
                LogicalThreadContext.Properties["UserId"] = userId;

                String logString = String.Format(
                   "{0} ::: {1}", identifyingString, jsonString);
                internalLogger.Informational(logString);
                return logString;
            }
            catch (Exception ex)
            {
                try
                {
                    var logString = "Logging failure";
                    Error(logString, ex);
                    return logString;
                }
                catch (Exception exInternal)
                {
                    // wrapping try block and exception swallowing to avoid any problems involving logging of the logging error affecting the actual code
                    return string.Format("could not log. Error: {0}", exInternal);
                }

            }
        }

        public void Debug(string message, Exception exception = null)
        {
            var log = LogManager.GetLogger(_mainLogName);

            if (!log.IsDebugEnabled)
                return;
            LogicalThreadContext.Properties["Class"] = "Unidentified";
            LogicalThreadContext.Properties["Method"] = "Unidentified";
            LogicalThreadContext.Properties["ElapsedTicks"] = "Unidentified";
            message = UserName + message;

            log.Debug(message);
        }

        public void Debug(object message, Exception exception = null)
        {
            var log = LogManager.GetLogger(_mainLogName);

            if (!log.IsDebugEnabled)
                return;
            LogicalThreadContext.Properties["Class"] = "Unidentified";
            LogicalThreadContext.Properties["Method"] = "Unidentified";
            LogicalThreadContext.Properties["ElapsedTicks"] = "Unidentified";
            var debugMessage = new { UserName, Message = message };

            log.Debug(debugMessage);
        }

        public void Warning(string message)
        {
            var log = LogManager.GetLogger(_mainLogName);

            if (!log.IsWarnEnabled)
                return;
            LogicalThreadContext.Properties["Class"] = "Unidentified";
            LogicalThreadContext.Properties["Method"] = "Unidentified";
            LogicalThreadContext.Properties["ElapsedTicks"] = "Unidentified";
            message = UserName + message;

            log.Warn(message);
        }

        public void Warning(object message)
        {
            var log = LogManager.GetLogger(_mainLogName);

            if (!log.IsWarnEnabled)
                return;
            LogicalThreadContext.Properties["Class"] = "Unidentified";
            LogicalThreadContext.Properties["Method"] = "Unidentified";
            LogicalThreadContext.Properties["ElapsedTicks"] = "Unidentified";
            var warnMessage = new { UserName, Message = message };

            log.Warn(warnMessage);
        }

        public void Funnel(string message)
        {
            var log = LogManager.GetLogger(_mainLogName);
            LogicalThreadContext.Properties["Class"] = "Unidentified";
            LogicalThreadContext.Properties["Method"] = "Unidentified";
            LogicalThreadContext.Properties["ElapsedTicks"] = "Unidentified";
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, FunnelLevel, message, null);
        }

        public void Funnel(object message)
        {
            var log = LogManager.GetLogger(_mainLogName);
            LogicalThreadContext.Properties["Class"] = "Unidentified";
            LogicalThreadContext.Properties["Method"] = "Unidentified";
            LogicalThreadContext.Properties["ElapsedTicks"] = "Unidentified";
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, FunnelLevel, message, null);
        }

        public string GetLoggerName()
        {
            return _mainLogName;
        }

        /// <summary>
        /// This method gets the calling method info
        /// using the stack frames. It avoids using the current class details
        /// but the call class details.
        /// </summary>
        /// <returns></returns>
        private CallingMethodInfo GetCallingMethodInfo()
        {
            string callingClassName = "Unidentified";
            string callingMethodName = "Unidentified";
            Stopwatch stopwatch = new Stopwatch();

            try
            {
                stopwatch.Start();
                StackTrace currentStackTrace = new StackTrace();
                StackFrame[] stackFrames = currentStackTrace.GetFrames();


                string currentClassName = typeof(Log4NetLogger).Name;
                if (stackFrames != null)
                {
                    int stackFrameLoopinglimit = GetStackFrameLoopingLimit(stackFrames);
                    for (int i = 0; i < stackFrameLoopinglimit; i++)
                    {
                        StackFrame stackFrame = stackFrames[i];
                        var declaringType = stackFrame.GetMethod().DeclaringType;
                        if (declaringType == null)
                        {
                            break;
                        }
                        else
                        {
                            string stackFrameClassName = declaringType.Name;
                            string stackFrameClassFullName = declaringType.FullName;
                            if (stackFrameClassName != currentClassName)
                            {
                                callingClassName = stackFrameClassFullName;
                                callingMethodName = stackFrame.GetMethod().Name;
                                break;
                            }
                        }
                    }
                }
                stopwatch.Stop();
            }
            catch (Exception ex)
            {
                // we deliberately swallow the exception as logging should not cause
                // failures.
            }
            return new CallingMethodInfo
            {
                ClassName = callingClassName,
                MethodName = callingMethodName,
                ElapsedTicks = stopwatch.ElapsedTicks
            };
        }

        /// <summary>
        /// This is a utility method that bounds the number of stack frames to
        /// loop through in case the stack is too large.
        /// </summary>
        /// <param name="stackFrames"></param>
        /// <returns></returns>
        private int GetStackFrameLoopingLimit(StackFrame[] stackFrames)
        {
            if (stackFrames == null)
            {
                return 0;
            }
            if (stackFrames.Length > 10)
            {
                // To avoid looping over all of the stack frames,
                // we loop over bounded number of stack frames.
                // we arbitrarily picked 10. this may be tuned.
                return 10;
            }
            else
            {
                return stackFrames.Length;
            }
        }
    }
    public class CallingMethodInfo
    {
        public string ClassName { get; set; }

        public string MethodName { get; set; }

        public long ElapsedTicks { get; set; }

    }
}

