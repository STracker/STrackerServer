// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageConverterDummy.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the ImageConverterDummy type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Tests.Dummies
{
    using STrackerServer.ImageConverter.Core;

    /// <summary>
    /// The image converter dummy.
    /// </summary>
    public class ImageConverterDummy : IImageConverter
    {
        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="imageUrl">
        /// The image url.
        /// </param>
        /// <param name="defaultImage">
        /// The default image.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string Convert(string imageUrl, string defaultImage)
        {
            return imageUrl;
        }
    }
}
