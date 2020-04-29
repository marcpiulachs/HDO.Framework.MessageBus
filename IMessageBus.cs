using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDO.Framework.MessageBus
{
    /// <summary>
    /// Message Bus Interface that provides functionality for objects to subscribe to, unsubscribe to and publish message
    /// </summary>
    public interface IMessageBus
    {
        /// <summary>Subscribe to a Message of type TMessage</summary>
        /// <param name="handler">The handler.</param>
        /// <typeparam name="TMessage">The Type of the message to subscribe</typeparam>
        void Subscribe<TMessage>(ISubscriber<TMessage> handler) 
            where TMessage : IMessage;

        /// <summary>Unsubscribe the target from all messages it previously subscribed to.</summary>
        /// <typeparam name="TMessage">The type of the target that we need to unregister</typeparam>
        /// <param name="target">The target object.</param>
        void UnSubscribe<TMessage>(ISubscriber<TMessage> target)
            where TMessage : IMessage;

        /// <summary>Publish a message</summary>
        /// <param name="message">The message.</param>
        /// <typeparam name="TMessage">The type of message to publish</typeparam>
        void Publish<TMessage>(TMessage message) 
            where TMessage : IMessage;
    }
}
