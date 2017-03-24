﻿using System;

namespace Melanchall.DryMidi
{
    public sealed class PortPrefixMessage : MetaMessage
    {
        #region Constructor

        public PortPrefixMessage()
        {
        }

        public PortPrefixMessage(byte port)
            : this()
        {
            Port = port;
        }

        #endregion

        #region Properties

        public byte Port { get; set; }

        #endregion

        #region Overrides

        internal override void ReadContent(MidiReader reader, ReadingSettings settings, int size = -1)
        {
            Port = reader.ReadByte();
        }

        internal override void WriteContent(MidiWriter writer, WritingSettings settings)
        {
            writer.WriteByte(Port);
        }

        internal override int GetContentSize()
        {
            return 1;
        }

        protected override Message CloneMessage()
        {
            return new PortPrefixMessage(Port);
        }

        public override string ToString()
        {
            return $"Port Prefix (port = {Port})";
        }

        #endregion
    }
}