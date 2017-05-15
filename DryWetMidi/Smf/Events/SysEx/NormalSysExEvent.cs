﻿using System;
using System.Linq;

namespace Melanchall.DryWetMidi.Smf
{
    public sealed class NormalSysExEvent : SysExEvent
    {
        #region Constants

        private const byte EndOfEventByte = 0xF7;

        #endregion

        #region Constructor

        public NormalSysExEvent()
        {
        }

        public NormalSysExEvent(byte[] data)
        {
            Data = data;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether the specified event is equal to the current one.
        /// </summary>
        /// <param name="normalSysExEvent">The event to compare with the current one.</param>
        /// <returns>true if the specified event is equal to the current one; otherwise, false.</returns>
        public bool Equals(NormalSysExEvent normalSysExEvent)
        {
            return Equals(normalSysExEvent, true);
        }

        /// <summary>
        /// Determines whether the specified event is equal to the current one.
        /// </summary>
        /// <param name="normalSysExEvent">The event to compare with the current one.</param>
        /// <param name="respectDeltaTime">If true the <see cref="MidiEvent.DeltaTime"/> will be taken into an account
        /// while comparing events; if false - delta-times will be ignored.</param>
        /// <returns>true if the specified event is equal to the current one; otherwise, false.</returns>
        public bool Equals(NormalSysExEvent normalSysExEvent, bool respectDeltaTime)
        {
            if (ReferenceEquals(null, normalSysExEvent))
                return false;

            if (ReferenceEquals(this, normalSysExEvent))
                return true;

            return base.Equals(normalSysExEvent, respectDeltaTime);
        }

        #endregion

        #region Overrides

        internal override void Read(MidiReader reader, ReadingSettings settings, int size)
        {
            if (size < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(size),
                    size,
                    "Non-negative size have to be specified in order to read Normal SysEx event.");

            var data = reader.ReadBytes(size);

            Completed = data.Last() == EndOfEventByte;

            if (Data == null)
            {
                Data = data;
                return;
            }

            //

            var currentData = Data;
            var currentDataLength = currentData.Length;

            Array.Resize(ref currentData, currentData.Length + data.Length);
            Array.Copy(data, 0, currentData, currentDataLength, data.Length);
        }

        public override string ToString()
        {
            return "Normal SysEx";
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as NormalSysExEvent);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}