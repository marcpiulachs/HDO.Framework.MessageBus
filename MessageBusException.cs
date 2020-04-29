using System;

namespace HDO.Framework.MessageBus
{
    /// <summary>
    /// An <see cref="Exception" /> generated from the message bus.
    /// </summary>
    public class MessageBusException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBusException"/> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public MessageBusException(string message) : base (message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBusException"/> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference (Nothing in
        ///     Visual Basic) if no inner exception is specified.
        /// </param>
        public MessageBusException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}