﻿using System.ComponentModel.Composition;
using System.Waf.Applications.Services;

namespace System.Waf.UnitTesting.Mocks
{
    /// <summary>This mock class implements the IMessageService interface.</summary>
    [Export(typeof(IMessageService)), Export]
    public class MockMessageService : IMessageService
    {
        /// <summary>Gets the message type of the last shown message.</summary>
        public MessageType MessageType { get; private set; }

        /// <summary>Gets the owner window of the last shown message.</summary>
        public object? Owner { get; private set; }

        /// <summary>Gets the message content of the last shown message.</summary>
        public string? Message { get; private set; }

        /// <summary>Gets or sets a delegate that is called when ShowMessage is called.</summary>
        public Action<object?, string>? ShowMessageStub { get; set; }

        /// <summary>Gets or sets a delegate that is called when ShowWarning is called.</summary>
        public Action<object?, string>? ShowWarningStub { get; set; }

        /// <summary>Gets or sets a delegate that is called when ShowError is called.</summary>
        public Action<object?, string>? ShowErrorStub { get; set; }

        /// <summary>Gets or sets a delegate that is called when ShowQuestion is called.</summary>
        public Func<object?, string, bool?>? ShowQuestionStub { get; set; }

        /// <summary>Gets or sets a delegate that is called when ShowYesNoQuestion is called.</summary>
        public Func<object?, string, bool>? ShowYesNoQuestionStub { get; set; }

        /// <summary>Gets or sets a delegate that is called when ShowQuestion is called.</summary>
        [Obsolete("Use ShowQuestionStub instead")]
        public Func<string, bool?>? ShowQuestionAction { get; set; }

        /// <summary>Gets or sets a delegate that is called when ShowYesNoQuestion is called.</summary>
        [Obsolete("Use ShowYesNoQuestionStub instead")]
        public Func<string, bool>? ShowYesNoQuestionAction { get; set; }

        /// <summary>Shows the message.</summary>
        /// <param name="owner">The window that owns this Message Window.</param>
        /// <param name="message">The message.</param>
        public void ShowMessage(object? owner, string message)
        {
            MessageType = MessageType.Message;
            Owner = owner;
            Message = message;
            ShowMessageStub?.Invoke(owner, message);
        }

        /// <summary>Shows the message as warning.</summary>
        /// <param name="owner">The window that owns this Message Window.</param>
        /// <param name="message">The message.</param>
        public void ShowWarning(object? owner, string message)
        {
            MessageType = MessageType.Warning;
            Owner = owner;
            Message = message;
            ShowWarningStub?.Invoke(owner, message);
        }

        /// <summary>Shows the message as error.</summary>
        /// <param name="owner">The window that owns this Message Window.</param>
        /// <param name="message">The message.</param>
        public void ShowError(object? owner, string message)
        {
            MessageType = MessageType.Error;
            Owner = owner;
            Message = message;
            ShowErrorStub?.Invoke(owner, message);
        }

        /// <summary>Shows the specified question.</summary>
        /// <param name="owner">The window that owns this Message Window.</param>
        /// <param name="message">The question.</param>
        /// <returns><c>true</c> for yes, <c>false</c> for no and <c>null</c> for cancel.</returns>
        public bool? ShowQuestion(object? owner, string message)
        {
            Owner = owner;
            if (ShowQuestionStub != null) return ShowQuestionStub(owner, message);
#pragma warning disable CS0618 // Type or member is obsolete
            if (ShowQuestionAction != null) return ShowQuestionAction(message);
#pragma warning restore CS0618 // Type or member is obsolete
            return null;
        }

        /// <summary>Shows the specified yes/no question.</summary>
        /// <param name="owner">The window that owns this Message Window.</param>
        /// <param name="message">The question.</param>
        /// <returns><c>true</c> for yes and <c>false</c> for no.</returns>
        public bool ShowYesNoQuestion(object? owner, string message)
        {
            Owner = owner;
#pragma warning disable CS0618 // Type or member is obsolete
            return ShowYesNoQuestionStub?.Invoke(owner, message) ?? ShowYesNoQuestionAction?.Invoke(message) ?? false;
#pragma warning restore CS0618 // Type or member is obsolete
        }

        /// <summary>Clears the last remembered message.</summary>
        public void Clear()
        {
            MessageType = MessageType.None;
            Owner = null;
            Message = null;
        }
    }
}
