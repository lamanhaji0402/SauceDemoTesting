using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

namespace NUnit_Demo
{
    public class BrowserOps
    {
        private IWebDriver webDriver;

        public void InitBrowser()
        {
            webDriver = new ChromeDriver();
            webDriver.Manage().Window.Maximize();
        }

        public string Title => webDriver.Title;

        public void Goto(string url)
        {
            webDriver.Url = url;
        }

        public void Close()
        {
            webDriver.Quit();
        }

        public IWebDriver GetDriver => webDriver;
    }

    [TestFixture]
    public class UnitTest1
    {
        private BrowserOps brow = new BrowserOps();
        private string test_url = "https://www.saucedemo.com/";
        private IWebDriver driver;

        [SetUp]
        public void start_Browser()
        {
            brow.InitBrowser();
        }

        [Test, Order(1)]
        public void TestUsernameIsRequired()
        {
            brow.Goto(test_url);
            driver = brow.GetDriver;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            IWebElement username = wait.Until(d => d.FindElement(By.XPath("//*[@id='user-name']")));
            username.SendKeys("");

            IWebElement password = driver.FindElement(By.XPath("//*[@id='password']"));
            password.SendKeys("kmdo");

            IWebElement loginButton = driver.FindElement(By.XPath("//*[@id='login_button_container']/div/form/div[3]"));
            loginButton.Submit();

            string actualError = GetErrorMessage();
            string expectedError = "Epic sadface: Username is required";
            Assert.AreEqual(expectedError, actualError);
        }
        [Test, Order(2)]
        public void TestPasswordIsRequired()
        {
            brow.Goto(test_url);
            driver = brow.GetDriver;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            IWebElement username = wait.Until(d => d.FindElement(By.XPath("//*[@id='user-name']")));
            username.SendKeys("lkds");

            IWebElement password = driver.FindElement(By.XPath("//*[@id='password']"));
            password.SendKeys("");

            IWebElement loginButton = driver.FindElement(By.XPath("//*[@id='login_button_container']/div/form/div[3]"));
            loginButton.Submit();

            string actualError = GetErrorMessage();
            string expectedError = "Epic sadface: Password is required";
            Assert.AreEqual(expectedError, actualError);
        }
        [Test, Order(3)]
        public void TestSuccessLogin()
        {
            brow.Goto(test_url);
            driver = brow.GetDriver;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            IWebElement username = wait.Until(d => d.FindElement(By.XPath("//*[@id='user-name']")));
            username.SendKeys("standard_user");

            IWebElement password = driver.FindElement(By.XPath("//*[@id='password']"));
            password.SendKeys("secret_sauce");

            IWebElement loginButton = driver.FindElement(By.XPath("//*[@id='login_button_container']/div/form/div[3]"));
            loginButton.Submit();

            IWebElement productLabel = wait.Until(d => d.FindElement(By.XPath("//*[@id='inventory_container']/div/div[1]/div[1]/div")));
            Assert.IsTrue(productLabel.Displayed, "Product label is not displayed after successful login.");
            
        }

        [Test, Order(4)]
        public void TestWrongUsernameOrPassword()
        {
            brow.Goto(test_url);
            driver = brow.GetDriver;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            IWebElement username = wait.Until(d => d.FindElement(By.XPath("//*[@id='user-name']")));
            username.SendKeys("stanr");

            IWebElement password = driver.FindElement(By.XPath("//*[@id='password']"));
            password.SendKeys("secret_sause");

            IWebElement loginButton = driver.FindElement(By.XPath("//*[@id='login_button_container']/div/form/div[3]"));
            loginButton.Submit();

            string actualError = GetErrorMessage();
            string expectedError = "Epic sadface: Username and password do not match any user in this service";
            Assert.AreEqual(expectedError, actualError);
        }

        [Test, Order(5)]
        public void test_ShoppingBagSuccess()
        {
            brow.Goto(test_url);
            driver = brow.GetDriver;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            IWebElement username = wait.Until(d => d.FindElement(By.XPath("//*[@id='user-name']")));
            username.SendKeys("standard_user");

            IWebElement password = driver.FindElement(By.XPath("//*[@id='password']"));
            password.SendKeys("secret_sauce");

            IWebElement loginButton = driver.FindElement(By.XPath("//*[@id='login_button_container']/div/form/div[3]"));
            loginButton.Submit();

            IWebElement selectBackpack = wait.Until(d => d.FindElement(By.XPath("//*[@id='add-to-cart-sauce-labs-backpack']")));
            selectBackpack.Submit();

            IWebElement shoppingCartLink = wait.Until(d => d.FindElement(By.XPath("//*[@id='shopping_cart_container']/a")));
            shoppingCartLink.Click();

            IWebElement checkOutBtn = wait.Until(d => d.FindElement(By.XPath("//*[@id='checkout']")));
            checkOutBtn.Submit();
            IWebElement firstName = wait.Until(d => d.FindElement(By.XPath("//*[@id='first-name']")));
            firstName.SendKeys("Laman");

            IWebElement lastName = wait.Until(d => d.FindElement(By.XPath("//*[@id='last-name']")));
            lastName.SendKeys("Hajiyeva");

            IWebElement zip = wait.Until(d => d.FindElement(By.XPath("//*[@id='postal-code']")));
            zip.SendKeys("12345");

            IWebElement continueBtn = wait.Until(d => d.FindElement(By.XPath("//*[@id='continue']")));
            continueBtn.Submit();

            IWebElement finishBtn = driver.FindElement(By.XPath("//*[@id=\"finish\"]"));
            finishBtn.Submit();

            string checkoutMsg = GetFinishMessage();
            Assert.AreEqual("Your order has been dispatched, and will arrive just as fast as the pony can get there!", checkoutMsg);

        }

        [Test, Order(6)]
        public void TestEmptyShoppingBagCheckoutBtn()
        {
            brow.Goto(test_url);
            driver = brow.GetDriver;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            IWebElement username = wait.Until(d => d.FindElement(By.XPath("//*[@id='user-name']")));
            username.SendKeys("standard_user");

            IWebElement password = driver.FindElement(By.XPath("//*[@id='password']"));
            password.SendKeys("secret_sauce");

            IWebElement loginButton = driver.FindElement(By.XPath("//*[@id='login_button_container']/div/form/div[3]"));
            loginButton.Submit();

            IWebElement shoppingCartLink = wait.Until(d => d.FindElement(By.XPath("//*[@id='shopping_cart_container']/a")));
            shoppingCartLink.Click();

            IWebElement checkOutBtn = wait.Until(d => d.FindElement(By.XPath("//*[@id='checkout']")));
            checkOutBtn.Submit();

            bool isCheckoutPageDisplayed = IsCheckoutPageDisplayed();
            Assert.IsFalse(isCheckoutPageDisplayed, "Checkout page displayed with an empty shopping bag.");
        }
        [Test, Order(7)]
        public void TestEmptyShoppingBagPrice()
        {
            brow.Goto(test_url);
            driver = brow.GetDriver;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            IWebElement username = wait.Until(d => d.FindElement(By.XPath("//*[@id='user-name']")));
            username.SendKeys("standard_user");

            IWebElement password = driver.FindElement(By.XPath("//*[@id='password']"));
            password.SendKeys("secret_sauce");

            IWebElement loginButton = driver.FindElement(By.XPath("//*[@id='login_button_container']/div/form/div[3]"));
            loginButton.Submit();

            IWebElement shoppingCartLink = wait.Until(d => d.FindElement(By.XPath("//*[@id='shopping_cart_container']/a")));
            shoppingCartLink.Click();

            IWebElement checkOutBtn = wait.Until(d => d.FindElement(By.XPath("//*[@id='checkout']")));
            checkOutBtn.Submit();

            IWebElement firstName = wait.Until(d => d.FindElement(By.XPath("//*[@id='first-name']")));
            firstName.SendKeys("Laman");

            IWebElement lastName = wait.Until(d => d.FindElement(By.XPath("//*[@id='last-name']")));
            lastName.SendKeys("Hajiyeva");

            IWebElement zip = wait.Until(d => d.FindElement(By.XPath("//*[@id='postal-code']")));
            zip.SendKeys("aze1141");

            IWebElement continueBtn = wait.Until(d => d.FindElement(By.XPath("//*[@id='continue']")));
            continueBtn.Submit();

            string priceInfo = GetPriceInfo();
            Assert.AreNotEqual("Total: $0.00", priceInfo);
        }

        [Test, Order(8)]
        public void TestEmptyShoppingBagFinishMessage()
        {
            brow.Goto(test_url);
            driver = brow.GetDriver;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            IWebElement username = wait.Until(d => d.FindElement(By.XPath("//*[@id='user-name']")));
            username.SendKeys("standard_user");

            IWebElement password = driver.FindElement(By.XPath("//*[@id='password']"));
            password.SendKeys("secret_sauce");

            IWebElement loginButton = driver.FindElement(By.XPath("//*[@id='login_button_container']/div/form/div[3]"));
            loginButton.Submit();

            IWebElement shoppingCartLink = wait.Until(d => d.FindElement(By.XPath("//*[@id='shopping_cart_container']/a")));
            shoppingCartLink.Click();

            IWebElement checkOutBtn = wait.Until(d => d.FindElement(By.XPath("//*[@id='checkout']")));
            checkOutBtn.Submit();

            IWebElement firstName = wait.Until(d => d.FindElement(By.XPath("//*[@id='first-name']")));
            firstName.SendKeys("Laman");

            IWebElement lastName = wait.Until(d => d.FindElement(By.XPath("//*[@id='last-name']")));
            lastName.SendKeys("Hajiyeva");

            IWebElement zip = wait.Until(d => d.FindElement(By.XPath("//*[@id='postal-code']")));
            zip.SendKeys("aze1141");

            IWebElement continueBtn = wait.Until(d => d.FindElement(By.XPath("//*[@id='continue']")));
            continueBtn.Submit();

            IWebElement finishBtn = wait.Until(d => d.FindElement(By.XPath("//*[@id='finish']")));
            finishBtn.Submit();

            string checkoutMsg = GetFinishMessage();
            Assert.AreNotEqual("Your order has been dispatched, and will arrive just as fast as the pony can get there!", checkoutMsg);
        }
        [Test, Order(9)]
        public void TestEmptyShoppingBagZip()
        {
            brow.Goto(test_url);
            driver = brow.GetDriver;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            IWebElement username = wait.Until(d => d.FindElement(By.XPath("//*[@id='user-name']")));
            username.SendKeys("standard_user");

            IWebElement password = driver.FindElement(By.XPath("//*[@id='password']"));
            password.SendKeys("secret_sauce");

            IWebElement loginButton = driver.FindElement(By.XPath("//*[@id='login_button_container']/div/form/div[3]"));
            loginButton.Submit();

            IWebElement shoppingCartLink = wait.Until(d => d.FindElement(By.XPath("//*[@id='shopping_cart_container']/a")));
            shoppingCartLink.Click();

            IWebElement checkOutBtn = wait.Until(d => d.FindElement(By.XPath("//*[@id='checkout']")));
            checkOutBtn.Submit();

            IWebElement firstName = wait.Until(d => d.FindElement(By.XPath("//*[@id='first-name']")));
            firstName.SendKeys("Laman");

            IWebElement lastName = wait.Until(d => d.FindElement(By.XPath("//*[@id='last-name']")));
            lastName.SendKeys("Hajiyeva");

            IWebElement zip = wait.Until(d => d.FindElement(By.XPath("//*[@id='postal-code']")));
            zip.SendKeys("aze1141");

            IWebElement continueBtn = wait.Until(d => d.FindElement(By.XPath("//*[@id='continue']")));
            continueBtn.Submit();

            IWebElement finishBtn = wait.Until(d => d.FindElement(By.XPath("//*[@id='finish']")));
            finishBtn.Submit();

            string checkoutMsg = GetFinishMessage();
            Assert.AreNotEqual("Your order has been dispatched, and will arrive just as fast as the pony can get there!", checkoutMsg);
        }
        private string GetErrorMessage()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            return wait.Until(d => d.FindElement(By.XPath("//*[@id='login_button_container']/div/form/div[3]"))).Text;
        }

        private bool IsCheckoutPageDisplayed()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(d => d.FindElement(By.XPath("//*[@id='checkout_info_container']/div/form/div[1]")));
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private string GetPriceInfo()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            return wait.Until(d => d.FindElement(By.XPath("//*[@id='checkout_summary_container']/div/div[2]/div[8]"))).Text;
        }
        private string GetFinishMessage()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement finishMessageElement = wait.Until(d => d.FindElement(By.XPath("//*[@id='checkout_complete_container']/div")));
            return finishMessageElement.Text;
        }

        [TearDown]
        public void close_Browser()
        {
            brow.Close();
        }
    }
}