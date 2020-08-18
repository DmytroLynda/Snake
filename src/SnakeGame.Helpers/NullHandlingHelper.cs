using System;
using System.Diagnostics;
using System.Linq;

namespace SnakeGame.Helpers
{
    public static class NullHandlingHelper
    {
        #region Check for internal null
        public static void InternalCheckForNull<TObject>(object verifiable, string additionalMessage)
            where TObject : class
        {
            if (verifiable is null)
            {
                string issue = $"{nameof(TObject)} can't be null.";

                string cause = "This issue occurred internally or missing a null check from an external source.";

                string message = CreateMessageForNull(issue, cause, additionalMessage);

                Debug.Fail(message);
            }
        }

        public static void InternalCheckForNull<TObject>(object verifiable)
            where TObject : class
        {
            string noAdditionalMessage = string.Empty;
            InternalCheckForNull<TObject>(verifiable, noAdditionalMessage);
        }
        #endregion

        #region Check for external null
        public static void ExternalCheckForNull<TObject>(object verifiable, string additionalMessage)
            where TObject : class
        {
            if (verifiable is null)
            {
                string issue = $"{nameof(TObject)} can't be null.";

                string cause = "This null was received from an external source.";

                string message = CreateMessageForNull(issue, cause, additionalMessage);

                throw new ArgumentNullException(message);
            }
        }

        public static void ExternalCheckForNull<TObject>(object verifiable)
            where TObject : class
        {
            string noAdditionalMessage = string.Empty;
            ExternalCheckForNull<TObject>(verifiable, noAdditionalMessage);
        }
        #endregion

        private static string CreateMessageForNull(string issue, string cause, string additionalMessage)
        {
            StackFrame calledMethod = StackTraceCorrector.GetCalledMethodFrame(new StackTrace(skipFrames: 0, fNeedFileInfo: true));
            string calledMethodLine = string.Format(
                $"This issue occurred in {calledMethod.GetMethod()?.DeclaringType} at {calledMethod.GetMethod()} on {calledMethod.GetFileLineNumber()} line.");

            string dividingLine = new string('-', 50);
            string message = string.Format($"\n{dividingLine}" +
                                           $"\nCalled method: {calledMethodLine}" +
                                           $"\nIssue: {issue}" +
                                           $"\nCause: {cause}" +
                                           $"\nAdditional message: {additionalMessage}" +
                                           $"\nAdditional info: \n");
            return message;
        }

        private static class StackTraceCorrector
        {
            public static StackFrame GetCalledMethodFrame(StackTrace trace)
            {
                #region  Check for null
                if (trace == null) throw new ArgumentNullException(nameof(trace));
                #endregion


                var thisFileName = trace
                    .GetFrames()
                    .First(x => x?.GetFileName() != null)
                    .GetFileName();

                var calledFrame = trace
                    .GetFrames()
                    .FirstOrDefault(x => x?.GetFileName() != null && x.GetFileName() != thisFileName);

                return calledFrame;
            }
        }
    }
}
