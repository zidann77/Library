using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Library.Global_Classes
{
    /// <summary>
    /// CLASS: clsLogger (LibrarySystem Event Logger)
    /// PURPOSE: Centralized logging service for Windows Event Log integration
    /// DESIGN PATTERN: Singleton-inspired static implementation
    /// USAGE: Replace try-catch blocks with structured event logging
    /// SECURITY: Requires administrative privileges for initial event source creation
    /// AUTHOR: [Your Name]
    /// VERSION: 1.0
    /// </summary>
    public static class clsLogger
    {
        /// <summary>
        /// CONST: sourceName (Event Log Source Identifier)
        /// PURPOSE: Unique application identifier in Windows Event Log
        /// IMPORTANT: Must be unique across system applications
        /// WARNING: Changing requires admin privileges for recreation
        /// BEST PRACTICE: Match application/assembly name
        /// </summary>
        const string sourceName = "LibrarySystem";

      
        /// <summary>
        /// METHOD: LogMessage (Primary Logging Workhorse)
        /// PURPOSE: Unified method for all event types with stack trace context
        /// PARAMETERS:
        ///   - message: Human-readable description of the event/error
        ///   - Type: EventLogEntryType (Error, Warning, Information, SuccessAudit, FailureAudit)
        /// PERFORMANCE: StackTrace generation has overhead - use judiciously in loops
        /// THREAD SAFETY: EventLog.WriteEntry is thread-safe
        /// EXCEPTIONS: May throw SecurityException, InvalidOperationException
        /// </summary>
        /// <param name="message">Descriptive message of the event</param>
        /// <param name="Type">Type of event (Error, Warning, Information)</param>
        public static void LogMessage(string message, EventLogEntryType Type)
        {
            // Create the event source if it does not exist
            if (!EventLog.SourceExists(sourceName))
            {
                EventLog.CreateEventSource(sourceName, "Application");

            }

            try
            {
                // STACK TRACE ANALYSIS: Captures calling method context for debugging
                // NOTE: GetFrame(1) gets the caller of this method, not LogMessage itself
                // CAUTION: Stack traces may not work optimally with compiler optimizations
                // PERFORMANCE: Consider conditional compilation for debug/release
                var stackTrace = new StackTrace();
                var callingFrame = stackTrace.GetFrame(1);
                var method = callingFrame.GetMethod();
                var className = method.DeclaringType.Name;
                string methodName = method.Name;

                // MESSAGE FORMATTING: Structured format for easy parsing and filtering
                // FORMAT: "In Class [ClassName],[Message] in [MethodName]"
                // EXAMPLE: "In Class UserManager,Database connection failed in ValidateUser"
                // ENHANCEMENT: Add timestamp and thread ID for better tracing
                string fullMessage = $"In Class {className},{message} in {methodName}";

                // EVENT LOG WRITE: Final write to Windows Event Log
                // SECURITY: May fail if application lacks permissions
                // RETRY: Consider implementing retry logic for production resilience
                EventLog.WriteEntry(sourceName, fullMessage, Type);
            }
            catch (Exception ex)
            {
                // FALLBACK: If event logging fails, write to console
                // IMPORTANT: Critical to avoid losing log information
                // TODO: Implement secondary logging mechanism (file, database)
                Console.WriteLine($"LOGGING FAILED: {ex.Message}");
                Console.WriteLine($"ORIGINAL MESSAGE: {message}");
            }
        }

        ///// <summary>
        ///// METHOD: LogInformation (Legacy - Use LogMessage with EventLogEntryType.Information)
        ///// PURPOSE: Specific method for informational messages
        ///// </summary>
        //public static void LogInformation(string message)
        //{
        //    var stackTrace = new StackTrace();
        //    var callingFrame = stackTrace.GetFrame(1);
        //    var method = callingFrame.GetMethod();
        //    var className = method.DeclaringType.Name;
        //    string methodName = method.Name;
        //    string fullMessage = $"In Class {className},{message} in {methodName}";
        //    EventLog.WriteEntry(sourceName, fullMessage, EventLogEntryType.Information);
        //}

        ///// <summary>
        ///// METHOD: LogWarning (Legacy - Use LogMessage with EventLogEntryType.Warning)
        ///// PURPOSE: Specific method for warning messages
        ///// </summary>
        //public static void LogWarning(string message)
        //{
        //    var stackTrace = new StackTrace();
        //    var callingFrame = stackTrace.GetFrame(1);
        //    var method = callingFrame.GetMethod();
        //    var className = method.DeclaringType.Name;
        //    string methodName = method.Name;
        //    string fullMessage = $"In Class {className},{message} in {methodName}";
        //    EventLog.WriteEntry(sourceName, fullMessage, EventLogEntryType.Warning);
        //}
    }

    

    //public class clsLogger
    //{

    //    const string sourceName = "LibrarySystem";

    //    public clsLogger()
    //    {
    //        // Create the event source if it does not exist
    //        if (!EventLog.SourceExists(sourceName))
    //        {
    //            EventLog.CreateEventSource(sourceName, "Application");
    //            Console.WriteLine("Event source created.");
    //        }

    //    }

    //    public static void LogMessage(string message , EventLogEntryType Type)
    //    {
    //        var stackTrace = new StackTrace();
    //        var callingFrame = stackTrace.GetFrame(1);
    //        var method = callingFrame.GetMethod();
    //        var className = method.DeclaringType.Name;
    //        string methodName = method.Name;
    //        string fullMessage = $"In Class {className},{message} in {methodName}";
    //        EventLog.WriteEntry(sourceName, fullMessage,Type);
    //    }

    //    //public static void LogInformation(string message)
    //    //{
    //    //    var stackTrace = new StackTrace();
    //    //    var callingFrame = stackTrace.GetFrame(1);
    //    //    var method = callingFrame.GetMethod();
    //    //    var className = method.DeclaringType.Name;
    //    //    string methodName = method.Name;
    //    //    string fullMessage = $"In Class {className},{message} in {methodName}";
    //    //    EventLog.WriteEntry(sourceName, fullMessage, EventLogEntryType.Information);
    //    //}

    //    //public static void LogWarning(string message)
    //    //{
    //    //    var stackTrace = new StackTrace();
    //    //    var callingFrame = stackTrace.GetFrame(1);
    //    //    var method = callingFrame.GetMethod();
    //    //    var className = method.DeclaringType.Name;
    //    //    string methodName = method.Name;
    //    //    string fullMessage = $"In Class {className},{message} in {methodName}";
    //    //    EventLog.WriteEntry(sourceName, fullMessage, EventLogEntryType.Warning);
    //    //}
    //}
}
