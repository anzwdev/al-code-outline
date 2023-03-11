using AnZwDev.VSCodeLangServer.Protocol.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.VSCodeLangServer.Protocol.MessageProtocol
{

    public abstract class RequestHandler<TParameters, TResult> : AbstractMessageHandler
    {

        public RequestHandler(LanguageServerHost languageServerHost, string name) : base(languageServerHost, name)
        {
        }

        public override async Task HandleRawMessage(Message requestMessage, MessageWriter messageWriter)
        {
            var requestContext = new RequestContext<TResult>(requestMessage, messageWriter);
            TParameters typedParams = default(TParameters);
            if (requestMessage.Contents != null)
            {
                // TODO: Catch parse errors!
                typedParams = requestMessage.Contents.ToObject<TParameters>();
            }

            TResult result = await HandleMessage(typedParams, requestContext);

            if (result != null)
                await requestContext.SendResult(result);
        }

#pragma warning disable 1998
        protected virtual async Task<TResult> HandleMessage(TParameters parameters, RequestContext<TResult> context)
        {
            return default(TResult);
        }
#pragma warning restore 1998

    }


}
