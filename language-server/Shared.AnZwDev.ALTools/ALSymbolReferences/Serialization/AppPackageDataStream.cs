using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AnZwDev.ALTools.ALSymbolReferences.Serialization
{
    public class AppPackageDataStream : Stream
    {
        public static int HeaderLength = 40;
        protected Stream SourceStream { get; set; }

        public AppPackageDataStream(Stream sourceStream)
        {
            this.SourceStream = sourceStream;
            this.SourceStream.Seek(HeaderLength, SeekOrigin.Begin);
        }

        public override bool CanRead => this.SourceStream.CanRead;
        public override bool CanSeek => this.SourceStream.CanSeek;
        public override bool CanWrite => this.SourceStream.CanWrite;
        public override long Length => this.SourceStream.Length - HeaderLength;

        public override long Position 
        { 
            get
            {
                return this.SourceStream.Position - HeaderLength;
            }
            set
            {
                this.SourceStream.Position = value + HeaderLength;
            }
        }

        public override void Flush()
        {
            this.SourceStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return this.SourceStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    this.SourceStream.Seek(HeaderLength + offset, origin);
                    break;
                case SeekOrigin.Current:
                    this.SourceStream.Seek(offset, origin);
                    break;
                case SeekOrigin.End:
                    this.SourceStream.Seek(offset, origin);
                    break;
            }
            return this.Position;
        }

        public override void SetLength(long value)
        {
            this.SourceStream.SetLength(HeaderLength + value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this.SourceStream.Write(buffer, offset, count);
        }

    }
}
