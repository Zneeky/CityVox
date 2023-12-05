using AutoMapper;
using CityVoxWeb.DTOs.Issues.Reports;
using PuppeteerSharp;
using System;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CityVoxWeb.Common.AddressParser;
using CityVoxWeb.DTOs.Users;
using CityVoxWeb.Services.Interfaces;

namespace CityVoxWeb.Services.User_Services
{
    public class SofiaCallWebCrawlerService
    {
        private readonly IMapper _mapper;
        private readonly IUsersService _usersService;

        public SofiaCallWebCrawlerService(IMapper mapper, IUsersService usersService)
        {
            _mapper = mapper;
            _usersService = usersService;
        }

        public  async Task ForwardReportToCallSofia(ExportReportDto reportDto)
        {
            try
            {
                var user = await _usersService.GetByUsernameAsync(reportDto.CreatorUsername);
                var browserFetcher = new BrowserFetcher();
                var revisionInfo = browserFetcher.RevisionInfo(BrowserFetcher.DefaultChromiumRevision);
                await browserFetcher.DownloadAsync(revisionInfo.Revision);

                await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    Headless = false, // Set to false if you want to see the browser
                    Args = new[]
                    {
                            "--disable-web-security",
                            "--disable-features=IsolateOrigins,site-per-process",
                    },
                });
                await using var page = await browser.NewPageAsync();
                await page.GoToAsync("https://call.sofia.bg/bg/Signal/Create#");

                // Map the ReportType to the website's form options
                string mappedType = MapReportType(reportDto.TypeValue);

                // LOCATION INFORMATION FIELDS
                string[] addressInfo = ParseAddress(reportDto.Address);
                await page.TypeAsync("#CITY", addressInfo[3]);
                await page.TypeAsync("#NEIGHBOURHOOD", addressInfo[2]);
                await page.TypeAsync("#STREET", addressInfo[0]);
                await page.TypeAsync("#STREET_NO", addressInfo[1]);

                // ISSUE INFORMATION FIELDS
                //Target the title field
                await page.TypeAsync("#NAME", reportDto.Title);

                // Wait for the iframe to load
                var iframeDescriptionElementHandle = await page.QuerySelectorAsync("#DESCRIPTION_ifr");
                var frameDescription = await iframeDescriptionElementHandle.ContentFrameAsync();

                //Target the description field since it is in an iframe object
                await frameDescription.WaitForSelectorAsync("body#tinymce");
                await frameDescription.TypeAsync("body#tinymce", reportDto.Description);

                //select the category based on the report type
                await page.SelectAsync("#CATEGORY_ID", mappedType);

                // USER's INFORMATION FIELD
                await page.TypeAsync("#SUBMITTER_NAME", $"{user.FirstName} {user.LastName}");
                await page.TypeAsync("#SUBMITTER_PHONE", "0882331910");
                await page.TypeAsync("#SUBMITTER_EMAIL", user.Email);

                //SUBMIT REPORT
                // First, ensure that the checkbox is present
                await page.WaitForSelectorAsync("#userConsent");
                // Then, click on the checkbox to check it
                await page.ClickAsync("#userConsent");

                // Wait for the reCAPTCHA iframe to load
                await page.WaitForSelectorAsync("iframe[title='reCAPTCHA']");

                // Get the iframe element handle
                var iframeCaptchaElementHandle = await page.QuerySelectorAsync("iframe[title='reCAPTCHA']");

                // Switch to the iframe's content frame
                var frameCaptcha = await iframeCaptchaElementHandle.ContentFrameAsync();

                // Wait for the checkbox element within the iframe and click it
                await frameCaptcha.WaitForSelectorAsync(".recaptcha-checkbox-checkmark");
                await frameCaptcha.ClickAsync(".recaptcha-checkbox-checkmark");

                await page.ClickAsync("#submit-button-selector");

                // Optionally, handle the response or confirmation
            }
            catch (Exception ex)
            {
               throw new Exception("Error while forwarding report to Call Sofia", ex);
            }
        }

        private static string MapReportType(int typeValue)
        {
            // Implement the mapping logic here
            // Example:
            switch (typeValue)
            {
                case 0: return "1"; // Replace with actual value on the website
                case 1: return "11";  // Replace with actual value on the website
                // ... other cases
                default: return "11";   // Replace with actual value on the website
            }
        }


    }
}
