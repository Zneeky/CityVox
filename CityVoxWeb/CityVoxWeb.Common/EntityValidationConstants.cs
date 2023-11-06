using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityVoxWeb.Common
{
    public static class EntityValidationConstants
    {

        public static class ApplicationUserValidations
        {
            public const int UsernameMinLength = 2;
            public const int UsernameMaxLength = 30;

            public const int FirstNameMinLength = 2;
            public const int FirstNameMaxLength = 20;

            public const int LastNameMinLength = 2;
            public const int LastNameMaxLength = 20;

            public const int EmailMinLength = 8;
            public const int EmailMaxLength = 110;

            public const int PasswordMinLength = 6;
            public const int PasswordMaxLength = 200;

            public const int ProfilePictureUrlMaxLength = 2048;

            public const string PasswordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{6,}$";
            public const string EmailRegex = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        }
        public static class IssuesValidations
        {
            public const int TitleMinLength = 1;
            public const int TitleMaxLength = 40;

            public const int DescriptionMinLength = 10;
            public const int DescriptionMaxLength = 500;

            public const int ImageUrlMaxLength = 2048;

            public const double LatitudeMinRange = -90;
            public const double LatitudeMaxRange = 90;

            public const double LongitudeMinRange = -180;
            public const double LongitudeMaxRange = 180;

            public const int AddressMaxLength = 1000;
        }

        public static class PostValidations
        {
            public const int TextMinLength = 5;
            public const int TextMaxLength = 600;

            public const int ImageUrlsMaxLength = 10000;

        }

        public static class CommentValidations
        {
            public const int TextMinLength = 5;
            public const int TextMaxLength = 200;

        }

        public static class MunicipalityValidations
        {
            public const int MunicipalityNameMinLength = 3;
            public const int MunicipalityNameMaxLength = 20;

            public const int OpenStreetMapCodeMaxLength = 10;
        }

        public static class RegionValidations
        {
            public const int RegionNameMinLength = 3;
            public const int RegionNameMaxLength = 30;

            public const int OpenStreetMapCodeMaxLength = 10;
        }

        public static class MunicipalityRepresentativeValidations
        {
            public const int PositionNameMinLength = 5;
            public const int PositionNameMaxLength = 20;

            public const int DepartmentNameMinLength = 5;
            public const int DepartmentNameMaxLength = 40;

            public const int OfficePhoneNumMaxLength = 10;
            public const int OfficeEmailMaxLength = 320;

            public const int BioMaxLength = 500;
        }

        public static class NotificationValidations
        {
            public const int ContentMinLength = 5;
            public const int ContentMaxLength = 50;
        }
    }
}
