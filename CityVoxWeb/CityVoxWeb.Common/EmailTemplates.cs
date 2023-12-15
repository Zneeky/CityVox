using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityVoxWeb.Common
{
    public static class EmailTemplates
    {
        public const string EmailConfirm = "﻿<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Email Confirmation</title>\r\n    <style>\r\n        body {\r\n            font-family: Arial, sans-serif;\r\n            background-color: #f4f4f4;\r\n            color: #333;\r\n            line-height: 1.6;\r\n        }\r\n\r\n        .container {\r\n            max-width: 600px;\r\n            margin: 20px auto;\r\n            padding: 20px;\r\n            background: #fff;\r\n            border: 1px solid #ddd;\r\n            border-radius: 5px;\r\n        }\r\n\r\n        h1 {\r\n            color: #32a852;\r\n        }\r\n\r\n        a.button {\r\n            display: inline-block;\r\n            padding: 10px 20px;\r\n            margin: 20px 0;\r\n            background-color: #32a852;\r\n            color: #fff;\r\n            text-decoration: none;\r\n            border-radius: 5px;\r\n        }\r\n    </style>\r\n</head>\r\n<body>\r\n    <div class=\"container\">\r\n        <h1>Email Confirmation</h1>\r\n        <p> Hello {{UserName}}</p>\r\n        <p>Thanks for signing up! Please confirm your email address by clicking on the button-link below.</p>\r\n        <a href=\"{{Link}}\" class=\"button\">Confirm Email</a>\r\n        <p>If you did not sign up for this account, you can ignore this email.</p>\r\n        <p>Best regards,<br>CityVox</p>\r\n    </div>\r\n</body>\r\n</html>";
        public const string EmailTest = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <meta charset=\"utf-8\" />\r\n    <title></title>\r\n</head>\r\n<body>\r\n    <p>\r\n        Hello {{UserName}}, <br />\r\n        This email is coming from CityVox web app\r\n    </p>\r\n</body>\r\n</html>";
    }
}
