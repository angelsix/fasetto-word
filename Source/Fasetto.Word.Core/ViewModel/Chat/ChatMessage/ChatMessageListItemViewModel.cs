using System;

namespace Fasetto.Word.Core
{
    /// <summary>
    /// A view model for each chat message thread item in a chat thread
    /// </summary>
    public class ChatMessageListItemViewModel : BaseViewModel
    {
        /// <summary>
        /// The display name of the sender of the message
        /// </summary>
        public string SenderName { get; set; }

        /// <summary>
        /// The latest message from this chat
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The initials to show for the profile picture background
        /// </summary>
        public string Initials { get; set; }

        /// <summary>
        /// The RGB values (in hex) for the background color of the profile picture
        /// For example FF00FF for Red and Blue mixed
        /// </summary>
        public string ProfilePictureRGB { get; set; } 

        /// <summary>
        /// True if this item is currently selected
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// True if this message was sent by the signed in user
        /// </summary>
        public bool SentByMe { get; set; }

        /// <summary>
        /// The time the message was read, or <see cref="DateTimeOffset.MinValue"/> if not read
        /// </summary>
        public DateTimeOffset MessageReadTime { get; set; }

        /// <summary>
        /// True if this message has been read
        /// </summary>
        public bool MessageRead => MessageReadTime > DateTimeOffset.MinValue;

        /// <summary>
        /// The time the message was sent
        /// </summary>
        public DateTimeOffset MessageSentTime { get; set; }

        /// <summary>
        /// A flag indicating if this item was added since the first main list of items was created
        /// Used as a flag for animating in
        /// </summary>
        public bool NewItem { get; set; }

        /// <summary>
        /// The attachment to the message, if it is of an image type
        /// </summary>
        public ChatMessageListItemImageAttachmentViewModel ImageAttachment { get; set; }

        /// <summary>
        /// A flag indicating if we have any message text or not
        /// </summary>
        public bool HasMessage => Message != null;

        /// <summary>
        /// A flag indicating if we have an image attached to this message
        /// </summary>
        public bool HasImageAttachment => ImageAttachment != null;
    }
}
