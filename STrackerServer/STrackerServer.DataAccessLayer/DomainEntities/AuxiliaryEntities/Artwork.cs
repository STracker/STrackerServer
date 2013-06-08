﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Artwork.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of artwork object. Its a wrapper for one image, the image its expressed in byte[].
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities
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

        /// <summary>
        /// Gets or sets the image url.
        /// </summary>
        public string ImageUrl { get; set; }
    }
}