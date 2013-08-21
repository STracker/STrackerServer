// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateTvShow.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Create a television show view model
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.TvShow
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The create television show.
    /// </summary>
    public class CreateTvShow
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Required]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the poster.
        /// </summary>
        [Required]
        public string Poster { get; set; }

        /// <summary>
        /// Gets or sets the first aired.
        /// </summary>
        [Required]
        public string FirstAired { get; set; }

        /// <summary>
        /// Gets or sets the air day.
        /// </summary>
        [Required]
        public string AirDay { get; set; }

        /// <summary>
        /// Gets or sets the air time.
        /// </summary>
        [Required]
        public string AirTime { get; set; }
    }
}