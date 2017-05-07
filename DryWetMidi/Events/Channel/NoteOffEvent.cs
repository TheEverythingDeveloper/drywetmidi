﻿namespace Melanchall.DryWetMidi
{
    public sealed class NoteOffEvent : ChannelEvent
    {
        #region Constants

        private const int ParametersCount = 2;
        private const int NoteNumberParameterIndex = 0;
        private const int VelocityParameterIndex = 1;

        #endregion

        #region Constructor

        public NoteOffEvent()
            : base(ParametersCount)
        {
        }

        public NoteOffEvent(SevenBitNumber noteNumber, SevenBitNumber velocity)
            : this()
        {
            NoteNumber = noteNumber;
            Velocity = velocity;
        }

        #endregion

        #region Properties

        public SevenBitNumber NoteNumber
        {
            get { return _parameters[NoteNumberParameterIndex]; }
            set { _parameters[NoteNumberParameterIndex] = value; }
        }

        public SevenBitNumber Velocity
        {
            get { return _parameters[VelocityParameterIndex]; }
            set { _parameters[VelocityParameterIndex] = value; }
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return $"Note Off (channel = {Channel}, note number = {NoteNumber}, velocity = {Velocity})";
        }

        #endregion
    }
}