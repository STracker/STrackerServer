// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CloudinaryConverter.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The cloudinary converter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.ImageConverter.Cloudinary
{
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;

    using STrackerServer.ImageConverter.Core;

    /// <summary>
    /// The cloudinary repository.
    /// </summary>
    public class CloudinaryConverter : IImageConverter
    {
        /// <summary>
        /// The provider.
        /// </summary>
        private readonly Cloudinary provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudinaryConverter"/> class.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        public CloudinaryConverter(Cloudinary provider)
        {
            this.provider = provider;
        }

        /// <summary>
        /// Converts the image to a new image.
        /// </summary>
        /// <param name="imageUrl">
        /// The image url.
        /// </param>
        /// <param name="defaultImage">
        /// The default image in case of error
        /// </param>
        /// <returns>
        /// The resulting url.
        /// </returns>
        public string Convert(string imageUrl, string defaultImage)
        {
            if (imageUrl == null)
            {
                return defaultImage;
            }

            var fileDescription = new FileDescription(imageUrl);
            var result = this.provider.Upload(new ImageUploadParams { File = fileDescription, });
            return result.Uri != null ? result.Uri.AbsoluteUri : defaultImage;
        }
    }
}
