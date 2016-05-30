using System;

namespace LogEntryDemo.Logger
{
    public interface ILogger
    {
        void Audit(string message);
        void Audit(object message);

        void Critical(string message, Exception exception = null);
        void Critical(object message, Exception exception = null);

        void Debug(string message, Exception exception = null);
        void Debug(object message, Exception exception = null);

        void Error(string message, Exception exception = null);
        void Error(object message, Exception exception = null);

        void Informational(string message);

        /// <summary>
        /// This method is added to get the class and method values and add those to the log.
        /// To measure the cost of adding this information to every log message.
        /// </summary>
        /// <param name="message"></param>
        void LogDetailedInfoExperimentally(string message);


        void Informational(object message);

        /// <summary>
        /// TODO: additional entry points for writing the informational json are needed (one without userid)
        /// </summary>
        /// <param name="identifyingString"></param>
        /// <param name="userId"></param>
        /// <param name="objectToLog"></param>
        /// <returns></returns>
        string InformationalJson(String identifyingString, int userId, object objectToLog);

        void Warning(string message);
        void Warning(object message);

        void Funnel(string message);
        void Funnel(object message);

        string GetLoggerName();
    }
}
