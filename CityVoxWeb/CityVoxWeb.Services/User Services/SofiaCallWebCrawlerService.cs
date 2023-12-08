using CityVoxWeb.DTOs.Issues.Reports;
using PuppeteerSharp;
using static CityVoxWeb.Common.AddressParser;
using CityVoxWeb.DTOs.Users;
using CityVoxWeb.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using _2CaptchaAPI;
using PuppeteerExtraSharp.Plugins.Recaptcha.Provider.AntiCaptcha;
using PuppeteerExtraSharp.Plugins.Recaptcha;
using PuppeteerExtraSharp.Plugins.Recaptcha.Provider._2Captcha;
using PuppeteerExtraSharp;
using PuppeteerExtraSharp.Plugins.ExtraStealth;
using PuppeteerExtraSharp.Plugins.AnonymizeUa;

namespace CityVoxWeb.Services.User_Services
{
    public class SofiaCallWebCrawlerService
    {
        private readonly IUsersService _usersService;
        private readonly IConfiguration _configuration;

        public SofiaCallWebCrawlerService(IUsersService usersService, IConfiguration configuration)
        {
            _usersService = usersService;
            _configuration = configuration;
        }

        public async Task ForwardReportToCallSofia(ExportReportDto reportDto)
        {
            try
            {

                var user = await _usersService.GetByUsernameAsync(reportDto.CreatorUsername);
                //var browserFetcher = new BrowserFetcher();
                //var revisionInfo = browserFetcher.RevisionInfo(BrowserFetcher.DefaultChromiumRevision);
                //await browserFetcher.DownloadAsync(revisionInfo.Revision);

                var recaptchaPlugin = new RecaptchaPlugin(new TwoCaptcha(_configuration["reCaptchaSolver:ApiKey"]));
                await using var browser = await new PuppeteerExtra().Use(new StealthPlugin()).Use(new AnonymizeUaPlugin()).Use(recaptchaPlugin).LaunchAsync(new LaunchOptions
                {
                    Headless = false, // Set to false if you want to see the browser
                    Args = new[]
                    {
                            "--disable-web-security",
                            "--disable-features=IsolateOrigins,site-per-process",
                    },
                    SlowMo = 10,
                });
                await using var page = await browser.NewPageAsync();
                await page.GoToAsync("https://call.sofia.bg/bg/Signal/Create#");

                //Click on the button that opens the map
                await page.ClickAsync("a[href='#']");


                // Add marker to the map
                await page.EvaluateFunctionAsync(@"(lat, lng) => {
                    // Confirm the map object is available
                    if (window.map) {
                        const position = new google.maps.LatLng(lat, lng);
                        const marker = new google.maps.Marker({
                        position: position,
                        map: window.map
                    });

                    // Optionally set the map's center to the new marker's position
                    window.map.setCenter(position);

                    } else {
                    throw new Error('Map object not found');
                    }
                    }", reportDto.Latitude, reportDto.Longitude);


                // Wait for the zoom-in button to be available
                await page.WaitForSelectorAsync("button[title='Увеличаване на мащаба']");

                // Define how many times you want to click the zoom-in button
                int zoomClicks = 10; // for example, adjust as needed

                // Click the zoom-in button the desired number of times
                for (int i = 0; i < zoomClicks; i++)
                {
                    await page.ClickAsync("button[title='Увеличаване на мащаба']");
                    // Wait a bit for the zoom animation to complete before the next click
                    await page.WaitForTimeoutAsync(30); // Adjust the timeout as necessary
                }


                await page.ClickAsync("#map-canvas");

                // Map the ReportType to the website's form options
                string mappedType = MapReportType(reportDto.TypeValue);

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

                //reCaptchV2 solver plugin -- costs around 2$ per 1000 captchas
                await recaptchaPlugin.SolveCaptchaAsync(page);
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
