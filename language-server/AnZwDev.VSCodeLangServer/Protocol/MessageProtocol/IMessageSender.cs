//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using System.Threading.Tasks;

namespace AnZwDev.VSCodeLangServer.Protocol.MessageProtocol
{
    public interface IMessageSender
    {
        Task SendNotification<TParams>(string method, TParams notificationParams);

        Task<TResult> SendRequest<TParams, TResult>(string method, TParams requestParams, bool waitForResponse);

        Task<TResult> SendRequest<TResult>(string method);
    }
}
