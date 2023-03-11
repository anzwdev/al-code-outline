//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using AnZwDev.VSCodeLangServer.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AnZwDev.VSCodeLangServer.Protocol.MessageProtocol
{
    public class MessageDispatcher : IMessageDispatcher
    {
        #region Fields

        private ILogger logger;

        /*
        private Dictionary<string, Func<Message, MessageWriter, Task>> requestHandlers =
            new Dictionary<string, Func<Message, MessageWriter, Task>>();

        private Dictionary<string, Func<Message, MessageWriter, Task>> eventHandlers =
            new Dictionary<string, Func<Message, MessageWriter, Task>>();
        */

        private Dictionary<string, AbstractMessageHandler> _requestHandlers;
        private Dictionary<string, AbstractMessageHandler> _notificationHandlers;

        #endregion

        #region Constructors

        public MessageDispatcher(ILogger logger)
        {
            this.logger = logger;
            this._requestHandlers = new Dictionary<string, AbstractMessageHandler>();
            this._notificationHandlers = new Dictionary<string, AbstractMessageHandler>();
        }

        #endregion

        #region Public Methods

        public void RegisterRequestHandler(AbstractMessageHandler handler)
        {
            this.RegisterMessageHandler(this._requestHandlers, handler, true);
        }

        public void RegisterNotificationHandler(AbstractMessageHandler handler)
        {
            this.RegisterMessageHandler(this._notificationHandlers, handler, true);
        }

        protected bool RegisterMessageHandler(Dictionary<string, AbstractMessageHandler> handlersCollection, AbstractMessageHandler handler, bool replaceExistingHandler)
        {
            if (handlersCollection.ContainsKey(handler.Name))
            {
                if (!replaceExistingHandler)
                    return false;
                handlersCollection[handler.Name] = handler;
            }
            else
                handlersCollection.Add(handler.Name, handler);

            return true;
        }

        /*
        public void SetRequestHandler<TParams, TResult, TError, TRegistrationOptions>(
            RequestType<TParams, TResult, TError, TRegistrationOptions> requestType,
            Func<TParams, RequestContext<TResult>, Task> requestHandler)
        {
            bool overrideExisting = true;

            if (overrideExisting)
            {
                // Remove the existing handler so a new one can be set
                this.requestHandlers.Remove(requestType.Method);
            }

            this.requestHandlers.Add(
                requestType.Method,
                (requestMessage, messageWriter) =>
                {
                    var requestContext =
                        new RequestContext<TResult>(
                            requestMessage,
                            messageWriter);

                    TParams typedParams = default(TParams);
                    if (requestMessage.Contents != null)
                    {
                        // TODO: Catch parse errors!
                        typedParams = requestMessage.Contents.ToObject<TParams>();
                    }

                    return requestHandler(typedParams, requestContext);
                });
        }

        public void SetRequestHandler<TResult, TError, TRegistrationOptions>(
            RequestType0<TResult, TError, TRegistrationOptions> requestType0,
            Func<RequestContext<TResult>, Task> requestHandler)
        {
            this.SetRequestHandler(
                RequestType<Object, TResult, TError, TRegistrationOptions>.ConvertToRequestType(requestType0),
                (param1, requestContext) =>
                {
                    return requestHandler(requestContext);
                });
        }

        public void SetEventHandler<TParams, TRegistrationOptions>(
            NotificationType<TParams, TRegistrationOptions> eventType,
            Func<TParams, EventContext, Task> eventHandler)
        {
            bool overrideExisting = true;

            if (overrideExisting)
            {
                // Remove the existing handler so a new one can be set
                this.eventHandlers.Remove(eventType.Method);
            }

            this.eventHandlers.Add(
                eventType.Method,
                (eventMessage, messageWriter) =>
                {
                    var eventContext = new EventContext(messageWriter);

                    TParams typedParams = default(TParams);
                    if (eventMessage.Contents != null)
                    {
                        // TODO: Catch parse errors!
                        typedParams = eventMessage.Contents.ToObject<TParams>();
                    }

                    return eventHandler(typedParams, eventContext);
                });
        }
        */


        #endregion

        #region Private Methods

        public async Task DispatchMessage(Message messageToDispatch, MessageWriter messageWriter)
        {

            try
            {

                Task handlerToAwait = null;
                AbstractMessageHandler handler = null;

                switch (messageToDispatch.MessageType)
                {
                    case MessageType.Request:
                        if (this._requestHandlers.TryGetValue(messageToDispatch.Method, out handler))
                            handlerToAwait = handler.HandleRawMessage(messageToDispatch, messageWriter);
                        else
                            // TODO: Message not supported error
                            this.logger.Write(LogLevel.Warning, $"MessageDispatcher: No handler registered for Request type '{messageToDispatch.Method}'");
                        break;
                    case MessageType.Event:
                        if (this._notificationHandlers.TryGetValue(messageToDispatch.Method, out handler))
                            handlerToAwait = handler.HandleRawMessage(messageToDispatch, messageWriter);
                        else
                            // TODO: Message not supported error
                            this.logger.Write(LogLevel.Warning, $"MessageDispatcher: No handler registered for Event type '{messageToDispatch.Method}'");
                        break;
                    default:
                        // TODO: Return message not supported
                        this.logger.Write(LogLevel.Warning, $"MessageDispatcher received unknown message type of method '{messageToDispatch.Method}'");
                        break;
                }

                if (handlerToAwait != null)
                    await handlerToAwait;
            }
            catch (TaskCanceledException)
            {
                // Some tasks may be cancelled due to legitimate
                // timeouts so don't let those exceptions go higher.
            }
            catch (AggregateException e)
            {
                if ((e.InnerExceptions != null) && (e.InnerExceptions.Count > 0) && (!(e.InnerExceptions[0] is TaskCanceledException)))
                {
                    Exception innerException = e.InnerExceptions[0];

                    // Cancelled tasks aren't a problem, so rethrow
                    // anything that isn't a TaskCanceledException
                    this.logger.Write(LogLevel.Error, "Error: " + innerException.Message + "\n" + innerException.StackTrace);
                }
            }
            catch (Exception e)
            {
                this.logger.Write(LogLevel.Error, "Error: " + e.Message + "\n" + e.StackTrace);
            }
        }

        #endregion
    }
}
