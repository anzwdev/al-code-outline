//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace AnZwDev.VSCodeLangServer.Protocol.MessageProtocol
{
    /// <summary>
    /// Provides context for a received event so that handlers
    /// can write events back to the channel.
    /// </summary>
    public class NotificationContext
    {
        private MessageWriter messageWriter;

        public NotificationContext(MessageWriter messageWriter)
        {
            this.messageWriter = messageWriter;
        }

        public async Task SendNotification<TParams>(string name, TParams eventParams)
        {
            await this.messageWriter.WriteNotification(name, eventParams);
        }
    }
}
