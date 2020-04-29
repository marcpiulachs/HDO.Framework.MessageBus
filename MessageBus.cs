using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace HDO.Framework.MessageBus
{
    /// <summary>
    /// Message Bus concrete implementation
    /// </summary>
    public sealed class MessageBus : IMessageBus
    {
        /// <summary>
        /// Backing field of all types of messages and the actions of subscribers
        /// </summary>
        private readonly Dictionary<Type, IList> handlers = new Dictionary<Type, IList>();

        /// <summary>
        /// The synchronisation object we use to ensure mutual exclusion
        /// </summary>
        private readonly object syncObject = new object();

        #region Singleton

        static MessageBus _messageBus = null;

        public MessageBus()
        {

        }

        public static MessageBus Instance
        {
            get
            {
                if (_messageBus == null)
                    _messageBus = new MessageBus();

                return _messageBus;
            }
        }
        #endregion

        /// <summary>
        /// Subscribe to a Message of type TMessage
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <typeparam name="TMessage">The Type of the message to subscribe</typeparam>
        public void Subscribe<TMessage>(ISubscriber<TMessage> handler) where TMessage : IMessage
        {
            lock (this.syncObject)
            {
                if (!this.handlers.ContainsKey(typeof(TMessage)))
                {
                    this.handlers.Add(typeof(TMessage), new List<ISubscriber<TMessage>> { handler });
                }
                else
                {
                    this.handlers[typeof(TMessage)].Add(handler);
                }
            }
        }

        /// <summary>
        /// Unsubscribe target from all its subscribed messages
        /// </summary>
        /// <param name="target">The target param.</param>
        /// <typeparam name="TMessage">the target object</typeparam>
        public void UnSubscribe<TMessage>(ISubscriber<TMessage> target)
            where TMessage : IMessage
        {
            // Let's get our copy of the keys so that we don't get error 'Collection was modified'
            lock (this.syncObject)
            {
                var keyType = typeof(TMessage);

                if (this.handlers.ContainsKey(keyType) &&
                    this.handlers[keyType] != null &&
                    this.handlers[keyType].Count > 0 &&
                    this.handlers[keyType].Contains(target))
                {
                    this.handlers[keyType].Remove(target);
                }
            }
        }

        /// <summary>
        /// Publish the message on the message bus
        /// </summary>
        /// <param name="message">The message.</param>
        public void Publish<TMessage>() where TMessage : IMessage, new()
        {
            Publish<TMessage>(new TMessage());
        }

        /// <summary>
        /// Publish the message on the message bus
        /// </summary>
        /// <param name="message">The message.</param>
        /// <typeparam name="TMessage">Type of message to publish</typeparam>
        public void Publish<TMessage>(TMessage message) where TMessage : IMessage
        {
            IList handlersList = new List<ISubscriber<TMessage>>();

            // Get a copy of the handlers that are currently registered
            lock (this.syncObject)
            {
                if (this.handlers.ContainsKey(typeof(TMessage)))
                {
                    handlersList = this.handlers[typeof(TMessage)];
                }
            }

            // Call each handler in the list
            // NOTE: Outside of the lock is important
            foreach (var handler in handlersList)
            {
                try
                {
                    // Cast the message
                    var subscriber = handler as ISubscriber<TMessage>;

                    // process the message
                    subscriber.HandleMessage(message);
                }
                catch (Exception e)
                {
                    throw new MessageBusException(e.Message, e);
                }
            }
        }
    }
}