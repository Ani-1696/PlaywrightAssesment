using System.Threading.Tasks;
using Microsoft.Playwright;
using NUnit.Framework;
//using Microsoft.Playwright.Assertions; // Import only Expectations





namespace PlaywrightTests
{
    public class BrowserTests
    {
        string item1;
        string item2;       
        [Test]
        public async Task E2ESauceDemoUserLoginAndCheckout() 
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false, // Make the browser window visible
                SlowMo = 50,
                
                Args = new[] { "--start-maximized" }
            }) ;

            var context=await browser.NewContextAsync();
            var page = await browser.NewPageAsync();

            await page.SetViewportSizeAsync(1920,1080);
            await page.GotoAsync("https://www.saucedemo.com");
            // await Task.Delay(5000);

            //Entering username
            await page.GetByPlaceholder("Username").FillAsync("standard_user");
            await Task.Delay(2000);
            //Entering password
            await page.GetByPlaceholder("Password").FillAsync("secret_sauce");
            await Task.Delay(1500);
            //Perform Login opreation
            await page.ClickAsync("[data-test='login-button']");
            //Added item no 1
            item1 = "Sauce Labs Backpack";
            await page.Locator("[data-test=\"add-to-cart-sauce-labs-backpack\"]").ClickAsync();
            //Added item no 2
            item2 = "Sauce Labs Bike Light";
            await page.Locator("[data-test=\"add-to-cart-sauce-labs-bike-light\"]").ClickAsync();
            //Assert for checking cart contain 2 items 
            await page.WaitForSelectorAsync("[data-test=\"shopping-cart-badge\"]");
            await Assertions.Expect(page.Locator("[data-test=\"shopping-cart-badge\"]")).ToContainTextAsync("2");
            await page.Locator("[data-test=\"shopping-cart-link\"]").ClickAsync();

            await page.WaitForSelectorAsync($"[data-test=\"inventory-item-name\"]");
            //await Assertions.Expect(page.Locator($"[data-test=\"inventory-item-name\"]")).ToContainTextAsync(item1);
           // Assertions.Expect(page.Locator($"[data-test=\"inventory-item-name\"]")).ToContainTextAsync(item2);
           
            //Check out and log out from website
            await page.Locator("[data-test=\"checkout\"]").ClickAsync();
            await page.Locator("[data-test=\"continue\"]").ClickAsync();
            //Assert without entering data and check it will redirect or not
           // await Assertions.Expect(page.Locator("[data-test=\"continue\"]")).ToBeDisabledAsync();

            await page.Locator("[data-test=\"firstName\"]").FillAsync("Aniket");
            await page.Locator("[data-test=\"lastName\"]").FillAsync("C");
            await page.Locator("[data-test=\"postalCode\"]").FillAsync("12345");
            //Check again after entering data
            await Assertions.Expect(page.Locator("[data-test=\"continue\"]")).ToBeEnabledAsync();

            await page.Locator("[data-test=\"continue\"]").ClickAsync();
            await page.Locator("[data-test=\"finish\"]").ClickAsync();
            await page.Locator("[data-test=\"back-to-products\"]").ClickAsync();
            await page.GetByRole(AriaRole.Button, new() { Name = "Open Menu" }).ClickAsync();
            await page.Locator("[data-test=\"logout-sidebar-link\"]").ClickAsync();

            await Task.Delay(2000);

            await browser.CloseAsync(); // Close the browser window


        }

       
    }
}