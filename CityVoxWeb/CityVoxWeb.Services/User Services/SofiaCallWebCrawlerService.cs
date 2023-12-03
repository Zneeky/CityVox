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

namespace CityVoxWeb.Services.User_Services
{
    public class SofiaCallWebCrawlerService
    {
        private readonly IMapper _mapper;

        public SofiaCallWebCrawlerService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public  async Task ForwardReportToCallSofia(ExportReportDto reportDto)
        {
            try
            {
                var browserFetcher = new BrowserFetcher();
                var revisionInfo = browserFetcher.RevisionInfo(BrowserFetcher.DefaultChromiumRevision);
                await browserFetcher.DownloadAsync(revisionInfo.Revision);

                await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    Headless = false, // Set to false if you want to see the browser
                    Devtools = true,
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

                // Fill the form fields
                await page.TypeAsync("#NAME", reportDto.Title);

                // Wait for the iframe to load
                // Wait for the iframe to load
                // await page.WaitForSelectorAsync("#DESCRIPTION_ifr"); // Wait for the iframe by its ID

                var iframeElementHandle = await page.QuerySelectorAsync("#DESCRIPTION_ifr");
                var frame = await iframeElementHandle.ContentFrameAsync();
                await frame.WaitForSelectorAsync("body#tinymce");
                await frame.TypeAsync("body#tinymce", reportDto.Description);

                string typeValue = MapReportType(reportDto.TypeValue);
                await page.SelectAsync("#CATEGORY_ID", typeValue);

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
