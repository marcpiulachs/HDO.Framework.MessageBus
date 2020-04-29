using System;

namespace HDO.Framework.MessageBus
{
    /// <summary>
    /// The interface every class that wants to recieve events has to implement.
    /// </summary>
    /// <typeparam name="TMessage">The message class to handle</typeparam>
    public interface ISubscriber<in TMessage>
    {
        /// <summary>
        /// When implemented, should contain the logic for handling the requested <see cref="TMessage"/>.
        /// </summary>
        void HandleMessage(TMessage message);
    }
}