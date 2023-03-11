//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using System.Threading.Tasks;

namespace AnZwDev.VSCodeLangServer.Protocol.MessageProtocol
{
    public interface IMessageDispatcher
    {
        Task DispatchMessage(Message messageToDispatch, MessageWriter messageWriter);
    }
}
