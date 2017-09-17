using System;
using System.Threading.Tasks;

namespace Fasetto.Word.Core
{
    /// <summary>
    /// A view model for each chat message thread item's attachment
    /// (in this case an image) in a chat thread
    /// </summary>
    public class ChatMessageListItemImageAttachmentViewModel : BaseViewModel
    {
        #region Private Members

        /// <summary>
        /// The thumbnail URL of this attachment
        /// </summary>
        private string mThumbnailUrl;

        #endregion

        /// <summary>
        /// The title of this image file
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The original file name of the attachment
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The file size in bytes of this attachment
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// The thumbnail URL of this attachment
        /// </summary>
        public string ThumbnailUrl
        {
            get => mThumbnailUrl;
            set
            {
                // If value hasn't changed, return
                if (value == mThumbnailUrl)
                    return;

                // Update value
                mThumbnailUrl = value;

                // TODO: Download image from website
                //       Save file to local storage/cache
                //       Set LocalFilePath value
                //
                //       For now, just set the file path directly
                Task.Delay(2000).ContinueWith(t => LocalFilePath = "/Images/Samples/rusty.jpg");
            }
        }

        /// <summary>
        /// The local file path on this machine to the downloaded thumbnail
        /// </summary>
        public string LocalFilePath { get; set; }

        /// <summary>
        /// Indicates if an image has loaded
        /// </summary>
        public bool ImageLoaded => LocalFilePath != null;
    }
}
