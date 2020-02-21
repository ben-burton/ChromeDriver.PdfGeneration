using System;
using System.Collections.Generic;
using System.IO;
using OpenQA.Selenium.Chrome;

namespace ChromeDriver.PdfGeneration
{
    public class Program
    {
        public static void Main()
        {
            var webpageFile = $"{Environment.CurrentDirectory}\\HTMLPage1.html";

            var html = File.ReadAllText(webpageFile);

            File.WriteAllText(webpageFile, html.Replace("[Name]", "Sir/Madam"));

            var chromeOptions = new ChromeOptions();

            chromeOptions.AddArgument("--headless");
            chromeOptions.AddArgument("--disable-gpu");
            chromeOptions.AddExcludedArgument("enable-automation");
            chromeOptions.AddAdditionalCapability("useAutomationExtension", false);

            var driver = new OpenQA.Selenium.Chrome.ChromeDriver(chromeOptions);

            driver.Navigate().GoToUrl($"file:///{webpageFile}");

            var result = (Dictionary<string, object>)driver.ExecuteChromeCommandWithResult("Page.printToPDF", new Dictionary<string, object>
            {
                { "displayHeaderFooter", false },
                { "transferMode", "ReturnAsBase64" }
            });

            File.WriteAllBytes($"{Environment.CurrentDirectory}\\example_letter.pdf", Convert.FromBase64String((string)result["data"]));

            driver.Quit();
            driver.Dispose();
        }
    }
}
