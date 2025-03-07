using Microsoft.Xna.Framework.Content.Pipeline;

namespace MonoGame.RuntimeBuilder
{
    /// <summary>
    /// A simple console logger logs information around the <see cref="RuntimeBuilder"/>.
    /// <remarks>
    /// Create your own logger by inheritting from <see cref="ContentBuildLogger"/>.
    /// </remarks>
    /// </summary>
    public class ConsoleLogger : ContentBuildLogger
    {
        /// <summary>
        /// Logs a simple message.
        /// </summary>
        /// <param name="message">The message beeing logged.</param>
        /// <param name="messageArgs">Formatting of the message.</param>
        public override void LogMessage(string message, params object[] messageArgs)
        {
            Console.WriteLine(IndentString + message, messageArgs);
        }

        /// <summary>
        /// Logs an important message.
        /// </summary>
        /// <param name="message">The message beeing logged.</param>
        /// <param name="messageArgs">Formatting of the message.</param>
        public override void LogImportantMessage(string message, params object[] messageArgs)
        {
            Console.WriteLine(IndentString + message, messageArgs);
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

            Console.WriteLine(warning);
        }
    }
}
