// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IImageConverter.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines The Image Converter interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.ImageConverter.Core
{
    /// <summary>
    /// The Image Converter interface.
    /// </summary>
    public interface IImageConverter
    {
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
        string Convert(string imageUrl, string defaultImage);
    }
}
