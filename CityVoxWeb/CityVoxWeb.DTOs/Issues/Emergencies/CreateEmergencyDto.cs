using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static CityVoxWeb.Common.EntityValidationConstants.IssuesValidations;


namespace CityVoxWeb.DTOs.Issues.Emergencies
{
    public class CreateEmergencyDto
    {
        [Required]
        public string CreatorId { get; set; } = null!;

        [Required]
        [MinLength(TitleMinLength)]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MinLength(DescriptionMinLength)]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [MaxLength(ImageUrlMaxLength)]
        public string? ImageUrl { get; set; }

        [Range(LatitudeMinRange, LatitudeMaxRange)]
        public double Latitude { get; set; }

        [Range(LongitudeMinRange, LongitudeMaxRange)]
        public double Longitude { get; set; }

        public string? Address { get; set; }

        public string MunicipalityId { get; set; } = null!;

        public int Type { get; set; }
    }
}
