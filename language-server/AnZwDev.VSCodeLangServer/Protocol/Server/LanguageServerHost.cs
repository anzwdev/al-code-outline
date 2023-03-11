using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol.Channel;
using AnZwDev.VSCodeLangServer.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.VSCodeLangServer.Protocol.Server
{
    public class LanguageServerHost
    {

        protected static long MaxLogFileSize = 5000000; // 5MB

        public MessageDispatcher Dispatcher { get; protected set; }
        public ChannelBase Channel { get; protected set; }
        public ProtocolEndpoint Endpoint { get; protected set; }
        public ILogger Logger { get; protected set; }
        public string LogFilePath { get; protected set; }

        public LanguageServerHost()
        {
        }

        public virtual void Initialize()
        {
            //initialize logger
            this.Logger = CreateLogger();
            //initialize protocol dispatcher and endpoint
            this.Dispatcher = new MessageDispatcher(this.Logger);
            this.Channel = new StdioServerChannel(this.Logger);
            this.Endpoint = new ProtocolEndpoint(this.Channel, this.Dispatcher, this.Logger);
            //initialize message handlers
            InitializeMessageHandlers();
        }

        protected virtual ILogger CreateLogger()
        {
            string logFileFolderPath = System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location);
            this.LogFilePath = System.IO.Path.Combine(logFileFolderPath, "log.txt");

            try
            {
                if (System.IO.File.Exists(this.LogFilePath))
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(this.LogFilePath);
                    if (fileInfo.Length > MaxLogFileSize)
                        System.IO.File.Delete(this.LogFilePath);
                }
            }
            catch (Exception)
            {
            }

            Logging.Builder builder = Logging.CreateLogger();
            builder.AddLogFile(this.LogFilePath, LogLevel.Diagnostic);
            return builder.Build();
        }

        protected virtual void InitializeMessageHandlers()
        {
        }

        public void Run()
        {
            this.Channel.Start(MessageProtocolType.LanguageServer);
            this.Endpoint.Start();
            this.Endpoint.WaitForExit();
        }

        public void Stop()
        {
            this.Endpoint.Stop();
        }

    }
}
