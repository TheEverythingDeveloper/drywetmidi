﻿using Melanchall.DryWetMidi.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Melanchall.DryWetMidi.Smf.Interaction
{
    /// <summary>
    /// Provides a fluent API to build an instance of the <see cref="Interaction.Pattern"/>.
    /// </summary>
    public sealed class PatternBuilder
    {
        #region Fields

        private readonly List<IPatternAction> _actions = new List<IPatternAction>();

        private readonly Dictionary<object, int> _anchorCounters = new Dictionary<object, int>();
        private int _globalAnchorsCounter = 0;

        private SevenBitNumber _defaultVelocity = Interaction.Note.DefaultVelocity;
        private ILength _defaultNoteLength = (MusicalLength)MusicalFraction.Quarter;
        private ILength _defaultStep = (MusicalLength)MusicalFraction.Quarter;
        private OctaveDefinition _defaultOctave = OctaveDefinition.Get(4);

        #endregion

        #region Methods

        #region Note

        /// <summary>
        /// Adds a note by the specified note name using default velocity, length and octave.
        /// </summary>
        /// <param name="noteName">The name of a note.</param>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <remarks>
        /// To set default octave use <see cref="DefaultOctave(int)"/> method. By default the octave number is 4.
        /// To set default note length use <see cref="DefaultNoteLength(ILength)"/> method. By default the length
        /// is 1/4. To set default velocity use <see cref="DefaultVelocity(SevenBitNumber)"/> method. By default the
        /// velocity is 100.
        /// </remarks>
        /// <exception cref="InvalidEnumArgumentException"><paramref name="noteName"/> specified an invalid value.</exception>
        public PatternBuilder Note(NoteName noteName)
        {
            return Note(noteName, _defaultVelocity, _defaultNoteLength);
        }

        /// <summary>
        /// Adds a note by the specified note name using specified length and default velocity and octave.
        /// </summary>
        /// <param name="noteName">The name of a note.</param>
        /// <param name="length">The length of a note.</param>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <remarks>
        /// To set default octave use <see cref="DefaultOctave(int)"/> method. By default the octave number is 4.
        /// To set default velocity use <see cref="DefaultVelocity(SevenBitNumber)"/> method. By default the
        /// velocity is 100.
        /// </remarks>
        /// <exception cref="InvalidEnumArgumentException"><paramref name="noteName"/> specified an invalid value.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="length"/> is null.</exception>
        public PatternBuilder Note(NoteName noteName, ILength length)
        {
            return Note(noteName, _defaultVelocity, length);
        }

        /// <summary>
        /// Adds a note by the specified note name using specified velocity and default length and octave.
        /// </summary>
        /// <param name="noteName">The name of a note.</param>
        /// <param name="velocity">The velocity of a note.</param>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <remarks>
        /// To set default octave use <see cref="DefaultOctave(int)"/> method. By default the octave number is 4.
        /// To set default note length use <see cref="DefaultNoteLength(ILength)"/> method. By default the length
        /// is 1/4.
        /// </remarks>
        /// <exception cref="InvalidEnumArgumentException"><paramref name="noteName"/> specified an invalid value.</exception>
        public PatternBuilder Note(NoteName noteName, SevenBitNumber velocity)
        {
            return Note(noteName, velocity, _defaultNoteLength);
        }

        /// <summary>
        /// Adds a note by the specified note name using specified velocity and length, and default octave.
        /// </summary>
        /// <param name="noteName">The name of a note.</param>
        /// <param name="velocity">The velocity of a note.</param>
        /// <param name="length">The length of a note.</param>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <remarks>
        /// To set default octave use <see cref="DefaultOctave(int)"/> method. By default the octave number is 4.
        /// </remarks>
        /// <exception cref="InvalidEnumArgumentException"><paramref name="noteName"/> specified an invalid value.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="length"/> is null.</exception>
        public PatternBuilder Note(NoteName noteName, SevenBitNumber velocity, ILength length)
        {
            ThrowIfArgument.IsInvalidEnumValue(nameof(noteName), noteName);

            return Note(_defaultOctave.GetNoteDefinition(noteName), velocity, length);
        }

        /// <summary>
        /// Adds a note by the specified definition using default length and velocity.
        /// </summary>
        /// <param name="noteDefinition">The definition of a note.</param>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <remarks>
        /// To set default note length use <see cref="DefaultNoteLength(ILength)"/> method. By default the length
        /// is 1/4. To set default velocity use <see cref="DefaultVelocity(SevenBitNumber)"/> method. By default the
        /// velocity is 100.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="noteDefinition"/> is null.</exception>
        public PatternBuilder Note(NoteDefinition noteDefinition)
        {
            return Note(noteDefinition, _defaultVelocity, _defaultNoteLength);
        }

        /// <summary>
        /// Adds a note by the specified definition using specified length and default velocity.
        /// </summary>
        /// <param name="noteDefinition">The definition of a note.</param>
        /// <param name="length">The length of a note.</param>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <remarks>
        /// To set default velocity use <see cref="DefaultVelocity(SevenBitNumber)"/> method. By default the
        /// velocity is 100.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="noteDefinition"/> is null. -or-
        /// <paramref name="length"/> is null.</exception>
        public PatternBuilder Note(NoteDefinition noteDefinition, ILength length)
        {
            return Note(noteDefinition, _defaultVelocity, length);
        }

        /// <summary>
        /// Adds a note by the specified definition using specified velocity and default length.
        /// </summary>
        /// <param name="noteDefinition">The definition of a note.</param>
        /// <param name="velocity">The velocity of a note.</param>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <remarks>
        /// To set default note length use <see cref="DefaultNoteLength(ILength)"/> method. By default the length
        /// is 1/4.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="noteDefinition"/> is null.</exception>
        public PatternBuilder Note(NoteDefinition noteDefinition, SevenBitNumber velocity)
        {
            return Note(noteDefinition, velocity, _defaultNoteLength);
        }

        /// <summary>
        /// Adds a note by the specified definition using specified velocity and length.
        /// </summary>
        /// <param name="noteDefinition">The definition of a note.</param>
        /// <param name="velocity">The velocity of a note.</param>
        /// <param name="length">The length of a note.</param>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="noteDefinition"/> is null. -or-
        /// <paramref name="length"/> is null.</exception>
        public PatternBuilder Note(NoteDefinition noteDefinition, SevenBitNumber velocity, ILength length)
        {
            ThrowIfArgument.IsNull(nameof(noteDefinition), noteDefinition);
            ThrowIfArgument.IsNull(nameof(length), length);

            return AddAction(new AddNoteAction(noteDefinition, velocity, length));
        }

        #endregion

        #region Chord

        /// <summary>
        /// Adds a chord by the specified notes names using default velocity, length and octave.
        /// </summary>
        /// <param name="noteNames">Names of notes that represent a chord.</param>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <remarks>
        /// To set default octave use <see cref="DefaultOctave(int)"/> method. By default the octave number is 4.
        /// To set default note length use <see cref="DefaultNoteLength(ILength)"/> method. By default the length
        /// is 1/4. To set default velocity use <see cref="DefaultVelocity(SevenBitNumber)"/> method. By default the
        /// velocity is 100.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="noteNames"/> is null.</exception>
        public PatternBuilder Chord(IEnumerable<NoteName> noteNames)
        {
            return Chord(noteNames, _defaultVelocity, _defaultNoteLength);
        }

        /// <summary>
        /// Adds a chord by the specified notes names using specified length and default velocity, and default octave.
        /// </summary>
        /// <param name="noteNames">Names of notes that represent a chord.</param>
        /// <param name="length">The length of a chord.</param>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <remarks>
        /// To set default octave use <see cref="DefaultOctave(int)"/> method. By default the octave number is 4.
        /// To set default velocity use <see cref="DefaultVelocity(SevenBitNumber)"/> method. By default the
        /// velocity is 100.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="noteNames"/> is null. -or-
        /// <paramref name="length"/> is null.</exception>
        public PatternBuilder Chord(IEnumerable<NoteName> noteNames, ILength length)
        {
            return Chord(noteNames, _defaultVelocity, length);
        }

        /// <summary>
        /// Adds a chord by the specified notes names using specified velocity and default length, and default octave.
        /// </summary>
        /// <param name="noteNames">Names of notes that represent a chord.</param>
        /// <param name="velocity">The velocity of a chord.</param>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <remarks>
        /// To set default octave use <see cref="DefaultOctave(int)"/> method. By default the octave number is 4.
        /// To set default note length use <see cref="DefaultNoteLength(ILength)"/> method. By default the length
        /// is 1/4.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="noteNames"/> is null.</exception>
        public PatternBuilder Chord(IEnumerable<NoteName> noteNames, SevenBitNumber velocity)
        {
            return Chord(noteNames, velocity, _defaultNoteLength);
        }

        /// <summary>
        /// Adds a chord by the specified notes names using specified velocity and length, and default octave.
        /// </summary>
        /// <param name="noteNames">Names of notes that represent a chord.</param>
        /// <param name="velocity">The velocity of a chord.</param>
        /// <param name="length">The length of a chord.</param>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <remarks>
        /// To set default octave use <see cref="DefaultOctave(int)"/> method. By default the octave number is 4.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="noteNames"/> is null. -or-
        /// <paramref name="length"/> is null.</exception>
        public PatternBuilder Chord(IEnumerable<NoteName> noteNames, SevenBitNumber velocity, ILength length)
        {
            ThrowIfArgument.IsNull(nameof(noteNames), noteNames);
            ThrowIfArgument.IsNull(nameof(length), length);

            return Chord(noteNames.Select(n => _defaultOctave.GetNoteDefinition(n)), velocity, length);
        }

        /// <summary>
        /// Adds a chord by the specified notes definitions using default velocity and length.
        /// </summary>
        /// <param name="noteDefinitions">Definitions of notes that represent a chord.</param>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <remarks>
        /// To set default note length use <see cref="DefaultNoteLength(ILength)"/> method. By default the length
        /// is 1/4. To set default velocity use <see cref="DefaultVelocity(SevenBitNumber)"/> method. By default the
        /// velocity is 100.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="noteDefinitions"/> is null.</exception>
        public PatternBuilder Chord(IEnumerable<NoteDefinition> noteDefinitions)
        {
            return Chord(noteDefinitions, _defaultVelocity, _defaultNoteLength);
        }

        /// <summary>
        /// Adds a chord by the specified notes definitions using specified length and default velocity.
        /// </summary>
        /// <param name="noteDefinitions">Definitions of notes that represent a chord.</param>
        /// <param name="length">The length of a chord.</param>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <remarks>
        /// To set default velocity use <see cref="DefaultVelocity(SevenBitNumber)"/> method. By default the
        /// velocity is 100.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="noteDefinitions"/> is null. -or-
        /// <paramref name="length"/> is null.</exception>
        public PatternBuilder Chord(IEnumerable<NoteDefinition> noteDefinitions, ILength length)
        {
            return Chord(noteDefinitions, _defaultVelocity, length);
        }

        /// <summary>
        /// Adds a chord by the specified notes definitions using specified velocity and default length.
        /// </summary>
        /// <param name="noteDefinitions">Definitions of notes that represent a chord.</param>
        /// <param name="velocity">The velocity of a chord.</param>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <remarks>
        /// To set default note length use <see cref="DefaultNoteLength(ILength)"/> method. By default the length
        /// is 1/4.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="noteDefinitions"/> is null.</exception>
        public PatternBuilder Chord(IEnumerable<NoteDefinition> noteDefinitions, SevenBitNumber velocity)
        {
            return Chord(noteDefinitions, velocity, _defaultNoteLength);
        }

        /// <summary>
        /// Adds a chord by the specified notes definitions using specified velocity and length.
        /// </summary>
        /// <param name="noteDefinitions">Definitions of notes that represent a chord.</param>
        /// <param name="velocity">The velocity of a chord.</param>
        /// <param name="length">The length of a chord.</param>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="noteDefinitions"/> is null. -or-
        /// <paramref name="length"/> is null.</exception>
        public PatternBuilder Chord(IEnumerable<NoteDefinition> noteDefinitions, SevenBitNumber velocity, ILength length)
        {
            ThrowIfArgument.IsNull(nameof(noteDefinitions), noteDefinitions);
            ThrowIfArgument.IsNull(nameof(length), length);

            return AddAction(new AddChordAction(noteDefinitions, velocity, length));
        }

        #endregion

        #region Pattern

        /// <summary>
        /// Adds a pattern.
        /// </summary>
        /// <param name="pattern">Pattern to add.</param>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="pattern"/> is null.</exception>
        public PatternBuilder Pattern(Pattern pattern)
        {
            ThrowIfArgument.IsNull(nameof(pattern), pattern);

            return AddAction(new AddPatternAction(pattern));
        }

        #endregion

        #region Anchor

        /// <summary>
        /// Places the specified anchor at the current time.
        /// </summary>
        /// <param name="anchor">Anchor to place.</param>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="anchor"/> is null.</exception>
        public PatternBuilder Anchor(object anchor)
        {
            ThrowIfArgument.IsNull(nameof(anchor), anchor);

            UpdateAnchorsCounters(anchor);

            return AddAction(new AddAnchorAction(anchor));
        }

        /// <summary>
        /// Places an anchor at the current time.
        /// </summary>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        public PatternBuilder Anchor()
        {
            UpdateAnchorsCounters(null);

            return AddAction(new AddAnchorAction());
        }

        /// <summary>
        /// Moves to the first specified anchor.
        /// </summary>
        /// <param name="anchor">Anchor to move to.</param>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="anchor"/> is null.</exception>
        /// <exception cref="ArgumentException">There are no anchors with the <paramref name="anchor"/> key.</exception>
        public PatternBuilder MoveToFirstAnchor(object anchor)
        {
            ThrowIfArgument.IsNull(nameof(anchor), anchor);

            var counter = GetAnchorCounter(anchor);
            if (counter < 1)
                throw new ArgumentException($"There are no anchors with the '{anchor}' key.", nameof(anchor));

            return AddAction(new MoveToAnchorAction(anchor, AnchorPosition.First));
        }

        /// <summary>
        /// Move to the first anchor.
        /// </summary>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <exception cref="InvalidOperationException">There are no anchors.</exception>
        public PatternBuilder MoveToFirstAnchor()
        {
            var counter = GetAnchorCounter(null);
            if (counter < 1)
                throw new InvalidOperationException("There are no anchors.");

            return AddAction(new MoveToAnchorAction(AnchorPosition.First));
        }

        /// <summary>
        /// Moves to the last specified anchor.
        /// </summary>
        /// <param name="anchor">Anchor to move to.</param>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="anchor"/> is null.</exception>
        /// <exception cref="ArgumentException">There are no anchors with the <paramref name="anchor"/> key.</exception>
        public PatternBuilder MoveToLastAnchor(object anchor)
        {
            ThrowIfArgument.IsNull(nameof(anchor), anchor);

            var counter = GetAnchorCounter(anchor);
            if (counter < 1)
                throw new ArgumentException($"There are no anchors with the '{anchor}' key.", nameof(anchor));

            return AddAction(new MoveToAnchorAction(anchor, AnchorPosition.Last));
        }

        /// <summary>
        /// Moves to the last anchor.
        /// </summary>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <exception cref="InvalidOperationException">The are no anchors.</exception>
        public PatternBuilder MoveToLastAnchor()
        {
            var counter = GetAnchorCounter(null);
            if (counter < 1)
                throw new InvalidOperationException("There are no anchors.");

            return AddAction(new MoveToAnchorAction(AnchorPosition.Last));
        }

        /// <summary>
        /// Moves to the nth specified anchor.
        /// </summary>
        /// <param name="anchor">Anchor to move to.</param>
        /// <param name="index">Index of an anchor to move to.</param>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="anchor"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is out of range.</exception>
        public PatternBuilder MoveToNthAnchor(object anchor, int index)
        {
            ThrowIfArgument.IsNull(nameof(anchor), anchor);

            var counter = GetAnchorCounter(anchor);
            ThrowIfArgument.IsOutOfRange(nameof(index),
                                         index,
                                         0,
                                         counter - 1,
                                         "Index is out of range.");

            return AddAction(new MoveToAnchorAction(anchor, AnchorPosition.Nth, index));
        }

        /// <summary>
        /// Moves to the nth anchor.
        /// </summary>
        /// <param name="index">Index of an anchor to move to.</param>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is out of range.</exception>
        public PatternBuilder MoveToNthAnchor(int index)
        {
            var counter = GetAnchorCounter(null);
            ThrowIfArgument.IsOutOfRange(nameof(index),
                                         index,
                                         0,
                                         counter - 1,
                                         "Index is out of range.");

            return AddAction(new MoveToAnchorAction(AnchorPosition.Nth, index));
        }

        #endregion

        #region Move

        /// <summary>
        /// Moves the current time by the specified step forward.
        /// </summary>
        /// <param name="step">Step to move by.</param>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="step"/> is null.</exception>
        public PatternBuilder StepForward(ILength step)
        {
            ThrowIfArgument.IsNull(nameof(step), step);

            return AddAction(new StepForwardAction(step));
        }

        /// <summary>
        /// Moves the current time by the default step forward.
        /// </summary>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <remarks>
        /// To set default step use <see cref="DefaultStep(ILength)"/> method. By default the step is 1/4.
        /// </remarks>
        public PatternBuilder StepForward()
        {
            return AddAction(new StepForwardAction(_defaultStep));
        }

        /// <summary>
        /// Moves the current time by the specified step back.
        /// </summary>
        /// <param name="step">Step to move by.</param>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="step"/> is null.</exception>
        public PatternBuilder StepBack(ILength step)
        {
            ThrowIfArgument.IsNull(nameof(step), step);

            return AddAction(new StepBackAction(step));
        }

        /// <summary>
        /// Moves the current time by the default step back.
        /// </summary>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <remarks>
        /// To set default step use <see cref="DefaultStep(ILength)"/> method. By default the step is 1/4.
        /// </remarks>
        public PatternBuilder StepBack()
        {
            return AddAction(new StepBackAction(_defaultStep));
        }

        /// <summary>
        /// Moves the current time to the specified one.
        /// </summary>
        /// <param name="time">Time to move to.</param>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="time"/> is null.</exception>
        public PatternBuilder MoveToTime(ITime time)
        {
            ThrowIfArgument.IsNull(nameof(time), time);

            return AddAction(new MoveToTimeAction(time));
        }

        /// <summary>
        /// Moves the current time to the previous one.
        /// </summary>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <remarks>
        /// On every action current time is stored in the time history. To return to the last saved time
        /// you can call the <see cref="MoveToPreviousTime"/>.
        /// </remarks>
        public PatternBuilder MoveToPreviousTime()
        {
            return AddAction(new MoveToTimeAction());
        }

        #endregion

        #region Repeat

        /// <summary>
        /// Repeats the specified number of previous actions.
        /// </summary>
        /// <param name="actionsCount">Number of previous actions to repeat.</param>
        /// <param name="repetitionsCount">Count of repetitions.</param>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <remarks>
        /// Note that <see cref="DefaultNoteLength(ILength)"/>, <see cref="DefaultOctave(int)"/>,
        /// <see cref="DefaultStep(ILength)"/> and <see cref="DefaultVelocity(SevenBitNumber)"/> are not
        /// actions and will not be repeated since default values applies immidiately on next actions.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="actionsCount"/> is
        /// negative. -or- <paramref name="actionsCount"/> is greater than count of existing actions. -or-
        /// <paramref name="repetitionsCount"/> is negative.</exception>
        public PatternBuilder Repeat(int actionsCount, int repetitionsCount)
        {
            ThrowIfArgument.IsNegative(nameof(actionsCount), actionsCount, "Actions count is negative.");
            ThrowIfArgument.IsGreaterThan(nameof(actionsCount),
                                          actionsCount,
                                          _actions.Count,
                                          "Actions count is greater than existing actions count.");
            ThrowIfArgument.IsNegative(nameof(repetitionsCount), repetitionsCount, "Repetitions count is negative.");

            return RepeatActions(actionsCount, repetitionsCount);
        }

        /// <summary>
        /// Repeats the previous action the specified number of times.
        /// </summary>
        /// <param name="repetitionsCount">Count of repetitions.</param>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <remarks>
        /// Note that <see cref="DefaultNoteLength(ILength)"/>, <see cref="DefaultOctave(int)"/>,
        /// <see cref="DefaultStep(ILength)"/> and <see cref="DefaultVelocity(SevenBitNumber)"/> are not
        /// actions and will not be repeated since default values applies immidiately on next actions.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="repetitionsCount"/> is negative.</exception>
        /// <exception cref="InvalidOperationException">There are no actions to repeat.</exception>
        public PatternBuilder Repeat(int repetitionsCount)
        {
            ThrowIfArgument.IsNegative(nameof(repetitionsCount), repetitionsCount, "Repetitions count is negative.");

            if (!_actions.Any())
                throw new InvalidOperationException("There is no action to repeat.");

            return RepeatActions(1, repetitionsCount);
        }

        /// <summary>
        /// Repeats the previous action one time.
        /// </summary>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <remarks>
        /// Note that <see cref="DefaultNoteLength(ILength)"/>, <see cref="DefaultOctave(int)"/>,
        /// <see cref="DefaultStep(ILength)"/> and <see cref="DefaultVelocity(SevenBitNumber)"/> are not
        /// actions and will not be repeated since default values applies immidiately on next actions.
        /// </remarks>
        /// <exception cref="InvalidOperationException">There are no actions to repeat.</exception>
        public PatternBuilder Repeat()
        {
            if (!_actions.Any())
                throw new InvalidOperationException("There is no action to repeat.");

            return RepeatActions(1, 1);
        }

        #endregion

        #region Default

        /// <summary>
        /// Sets default velocity that will be used by next actions of the builder that
        /// add notes.
        /// </summary>
        /// <param name="velocity">New default velocity of a note.</param>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <remarks>
        /// Setting default velocity is not an action and thus will not be stored in pattern.
        /// </remarks>
        public PatternBuilder DefaultVelocity(SevenBitNumber velocity)
        {
            _defaultVelocity = velocity;
            return this;
        }

        /// <summary>
        /// Sets default note length that will be used by next actions of the builder that
        /// add notes.
        /// </summary>
        /// <param name="length">New default note length.</param>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <remarks>
        /// Setting default note length is not an action and thus will not be stored in pattern.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="length"/> is null.</exception>
        public PatternBuilder DefaultNoteLength(ILength length)
        {
            ThrowIfArgument.IsNull(nameof(length), length);

            _defaultNoteLength = length;
            return this;
        }

        /// <summary>
        /// Sets default step for step back and step forward actions of the builder.
        /// </summary>
        /// <param name="step">New default step.</param>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <remarks>
        /// Setting default step is not an action and thus will not be stored in pattern.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="step"/> is null.</exception>
        public PatternBuilder DefaultStep(ILength step)
        {
            ThrowIfArgument.IsNull(nameof(step), step);

            _defaultStep = step;
            return this;
        }

        /// <summary>
        /// Sets default note octave that will be used by next actions of the builder that
        /// add notes.
        /// </summary>
        /// <param name="octave">New default octave.</param>
        /// <returns>The current <see cref="PatternBuilder"/>.</returns>
        /// <remarks>
        /// Setting default octave is not an action and thus will not be stored in pattern.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="octave"/> is out of valid range.</exception>
        public PatternBuilder DefaultOctave(int octave)
        {
            _defaultOctave = OctaveDefinition.Get(octave);
            return this;
        }

        #endregion

        /// <summary>
        /// Build an instance of the <see cref="Interaction.Pattern"/> holding all actions
        /// defined via builder.
        /// </summary>
        /// <returns>An instance of the <see cref="Interaction.Pattern"/> that holds all actions
        /// defined by the current <see cref="PatternBuilder"/>.</returns>
        public Pattern Build()
        {
            return new Pattern(_actions.ToList());
        }

        private PatternBuilder AddAction(IPatternAction patternEvent)
        {
            _actions.Add(patternEvent);
            return this;
        }

        private int GetAnchorCounter(object anchor)
        {
            if (anchor == null)
                return _globalAnchorsCounter;

            int counter;
            if (!_anchorCounters.TryGetValue(anchor, out counter))
                throw new ArgumentException($"Anchor {anchor} doesn't exist.", nameof(anchor));

            return counter;
        }

        private void UpdateAnchorsCounters(object anchor)
        {
            _globalAnchorsCounter++;

            if (anchor == null)
                return;

            if (!_anchorCounters.ContainsKey(anchor))
                _anchorCounters.Add(anchor, 0);

            _anchorCounters[anchor]++;
        }

        private PatternBuilder RepeatActions(int actionsCount, int repetitionsCount)
        {
            var actionsToRepeat = _actions.Skip(_actions.Count - actionsCount).ToList();
            var newActions = Enumerable.Range(0, repetitionsCount).SelectMany(i => actionsToRepeat);

            foreach (var action in newActions)
            {
                var addAnchorAction = action as AddAnchorAction;
                if (addAnchorAction != null)
                    UpdateAnchorsCounters(addAnchorAction.Anchor);

                _actions.Add(action);
            }

            return this;
        }

        #endregion
    }
}