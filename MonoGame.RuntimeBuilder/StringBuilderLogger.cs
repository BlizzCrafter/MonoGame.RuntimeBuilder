using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.Text;

namespace MonoGame.Forms.DX.RuntimeBuilder
{
    /// <summary>
    /// A string builder logger logs information around the <see cref="RuntimeBuilder"/>.
    /// <remarks>
    /// Create your own logger by inheritting from <see cref="ContentBuildLogger"/>.
    /// </remarks>
    /// </summary>
    public class StringBuilderLogger : ContentBuildLogger, IDisposable
    {
        /// <summary>
        /// Get the actual StringBuilder.
        /// </summary>
        public StringBuilder Log { get; private set; } = new StringBuilder();

        /// <summary>
        /// Triggers when a new message was logged.
        /// </summary>
        public event Action<string> OnMessageLogged = delegate { };

        /// <summary>
        /// Clears the log.
        /// </summary>
        public void Clear()
        {
            Log.Clear();

            OnMessageLogged?.Invoke(Log.ToString());
        }

        /// <summary>
        /// Append a line.
        /// </summary>
        /// <param name="line"></param>
        public void AppendLine(string line)
        {
            Log.AppendLine(line);
        }

        /// <summary>
        /// Use this command to return the contents of the StringBuilder / Log.
        /// </summary>
        /// <returns>The log output.</returns>
        public override string ToString()
        {
            return Log.ToString();
        }

        /// <summary>
        /// Logs a simple message.
        /// </summary>
        /// <param name="message">The message beeing logged.</param>
        /// <param name="messageArgs">Formatting of the message.</param>
        public override void LogMessage(string message, params object[] messageArgs)
        {
            Log.AppendFormat(IndentString + message, messageArgs);
            Log.AppendLine();

            OnMessageLogged?.Invoke(Log.ToString());
        }

        /// <summary>
        /// Logs an important message.
        /// </summary>
        /// <param name="message">The message beeing logged.</param>
        /// <param name="messageArgs">Formatting of the message.</param>
        public override void LogImportantMessage(string message, params object[] messageArgs)
        {
            Log.AppendFormat(IndentString + message, messageArgs);
            Log.AppendLine();

            OnMessageLogged?.Invoke(Log.ToString());
        }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// /// <param name="helpLink">The help link to the message.</param>
        /// /// <param name="contentIdentity">The content identity.</param>
        /// <param name="message">The message beeing logged.</param>
        /// <param name="messageArgs">Formatting of the message.</param>
        public override void LogWarning(string helpLink, ContentIdentity contentIdentity, string message, params object[] messageArgs)
        {
            var warning = string.Empty;
            if (contentIdentity != null && !string.IsNullOrEmpty(contentIdentity.SourceFilename))
            {
                warning = contentIdentity.SourceFilename;
                if (!string.IsNullOrEmpty(contentIdentity.FragmentIdentifier))
                    warning += "(" + contentIdentity.FragmentIdentifier + ")";
                warning += ": ";
            }

            if (messageArgs != null && messageArgs.Length != 0)
                warning += string.Format(message, messageArgs);
            else if (!string.IsNullOrEmpty(message))
                warning += message;

            Log.AppendLine(warning);
            Log.AppendLine();

            OnMessageLogged?.Invoke(Log.ToString());
        }

        /// <summary>
        /// Disposes the StringBuilder / Log.
        /// </summary>
        public void Dispose()
        {
            Log = null;
        }
    }

}
