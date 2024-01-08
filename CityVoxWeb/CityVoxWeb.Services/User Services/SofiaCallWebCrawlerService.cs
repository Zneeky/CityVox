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
                string mappedType = MapReportTypeSelectorV1(reportDto.TypeValue);

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
                //await page.SelectAsync("#CATEGORY_ID", mappedType);
                await page.SelectAsync("#CATEGORY_SCRIPT_ID", mappedType);


                // Selector for the parent div
                string parentDivSelector = "#CurrentQuestion";

                // Check if the parent div exists
                bool isParentDivPresent = await page.WaitForSelectorAsync(parentDivSelector, new WaitForSelectorOptions { Timeout = 5000 }).ContinueWith(task => task.IsCompletedSuccessfully);

                if (isParentDivPresent)
                {
                    // The parent div exists, so we attempt to click the button
                    while (true)
                    {
                        try
                        {
                            // Selector for the button with specific data-bind attribute and class
                            string buttonSelector = "button[data-bind*='selectQuestion'][class='btn btn-default']";

                            // Wait for the button to be available for clicking
                            await page.WaitForSelectorAsync(buttonSelector);

                            // Click the button
                            await page.ClickAsync(buttonSelector);

                            // Wait for some condition or delay to ensure the button can reappear
                            await page.WaitForTimeoutAsync(500); // Example delay
                        }
                        catch (PuppeteerException)
                        {
                            // If the WaitForSelectorAsync times out, the button is not present, and we break the loop
                            break;
                        }
                    }
                    // Continue with the next step after the button has been clicked as needed
                }


                // USER's INFORMATION FIELD
                await page.TypeAsync("#SUBMITTER_NAME", $"{user.FirstName} {user.LastName}");
                await page.TypeAsync("#SUBMITTER_PHONE", "0889765432");
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
                //Here could be added logic that would check the how much money are left in the account and if they are less than 0.2$ to throw an exception of insufficient funds
                await recaptchaPlugin.SolveCaptchaAsync(page);

                //Delay to see the submitted result
                await Task.Delay(TimeSpan.FromMinutes(1));

                //This is intended to stop the execution until we are ready with actual report cases, that are valid and can be directed to SofiaCall
                await page.ClickAsync("#submit-button-selector");

                // Optionally, handle the response or confirmation
            }
            catch (Exception ex)
            {
                throw new Exception("Error while forwarding report to Call Sofia", ex);
            }
        }


        private static string MapReportTypeSelectorV1(int typeValue)
        {
            // Implement the mapping logic here
            //Littering = 0,
            //Graffiti = 1,
            //RoadIssues = 2,
            //StreetlightIssues = 3,
            //ParkingViolation = 4,
            //PublicFacilities = 5,
            //TreeHazards = 6,
            //TrafficConcerns = 7,
            //Wildlife = 8,
            //PublicTransport = 9,
            //Sidewalks = 10,
            //Other = 11,
            return typeValue switch
            {
                0 => "10196",
                1 => "300048",
                2 => "30056",
                3 => "280068",
                4 => "50154",
                5 => "90198",
                6 => "60195",
                7 => "40081",
                8 => "220187",
                9 => "380260",
                10 => "30035",
                // ... other cases
                _ => "11",
            };
        }

        private static string MapReportTypeSelectorV2(int typeValue)
        {
            // Implement the mapping logic here
            //Littering = 0,
            //Graffiti = 1,
            //RoadIssues = 2,
            //StreetlightIssues = 3,
            //ParkingViolation = 4,
            //PublicFacilities = 5,
            //TreeHazards = 6,
            //TrafficConcerns = 7,
            //Wildlife = 8,
            //PublicTransport = 9,
            //Sidewalks = 10,
            //Other = 11,
            return typeValue switch
            {
                0 => "1",
                1 => "11",
                2 => "3",
                3 => "28",
                4 => "5",
                5 => "40",
                6 => "6",
                7 => "4",
                8 => "22",
                9 => "38",
                10 => "3",
                // ... other cases
                _ => "11",
            };
        }

    }
}
