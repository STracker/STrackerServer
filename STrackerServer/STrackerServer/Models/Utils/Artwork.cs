// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Artwork.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace STrackerServer.Models.Utils
{
    /// <summary>
    /// The artwork. This class stores an image file.
    /// That image will be used to show banners, posters or photos.
    /// </summary>
    public class Artwork
    {
        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        public byte[] Image { get; set; }

        /// <summary>
        /// Gets or sets the image mime type.
        /// </summary>
        public string ImageMimeType { get; set; }
    }
}