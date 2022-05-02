using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace RegionTests
{
    public class Tests
    {
        private IWebDriver driver;

        private readonly By _search_form_loc = By.XPath("//input[@placeholder='Найти город']");
        private readonly By _to_in_cities_loc = By.XPath("//li/a/span/mark");
        private readonly By _city_not_found_loc = By.CssSelector(".city-not-found");
        private const string _city_not_found_text = "Город не найден, возможно Вы имели ввиду:";

        private readonly By _city_select_loc = By.XPath("//div[contains(@class,'city-select')]/div/p");

        [SetUp]
        public void Setup()
        {
            //driver = new TWebDriver();
            driver = new OpenQA.Selenium.Chrome.ChromeDriver();
            driver.Navigate().GoToUrl("https://www.dns-shop.ru");
            driver.FindElement(By.CssSelector(".pull-right")).Click();
            Thread.Sleep(3000);
        }

        [Test]
        public void Search_form()
        {
            driver.FindElement(By.XPath("//div[text()='Ваш город']"));

            var search_form = driver.FindElement(_search_form_loc);

            string input_text = "То";
            search_form.SendKeys(input_text);
            var input_text_in_cities = driver.FindElement(_to_in_cities_loc).Text;
            Assert.AreEqual(input_text, input_text_in_cities, "Expected another text");

            search_form.Clear();
            search_form.SendKeys("Рандом");
            var actual_text = driver.FindElement(_city_not_found_loc).Text;
            Assert.AreEqual(_city_not_found_text, actual_text, "Expected another text");
        }

        [Test]
        public void City_bubbles()
        {
            var cities = new List<string>() { "Москва", "Санкт-Петербург", "Новосибирск",
                "Екатеринбург", "Нижний Новгород", "Казань", "Самара", "Владивосток" };

            foreach (var city in cities)
            {
                var actual_city = driver.FindElement(By.XPath($"//a[text()='{city}']")).Text;
                Assert.AreEqual(city, actual_city, "Expected another text");
            }

            Random rnd = new Random();
            string random_city = cities[rnd.Next(cities.Count)];
            driver.FindElement(By.XPath($"//a[text()='{random_city}']")).Click();
            Thread.Sleep(1000);
            string actual_selected = driver.FindElement(_city_select_loc).GetDomProperty("innerText");
            Assert.AreEqual(random_city, actual_selected, "Expected another text");
        }

        [Test]
        public void List_of_cities()
        {
            driver.FindElement(By.XPath("//span[text()='Дальневосточный']")).Click();
            driver.FindElement(By.XPath("//span[text()='Амурская область']")).Click();
            driver.FindElement(By.XPath("//span[text()='Белогорск']"));

            driver.FindElement(By.XPath("//span[text()='Приволжский']")).Click();
            driver.FindElement(By.XPath("//span[text()='Башкортостан']")).Click();
            driver.FindElement(By.XPath("//span[text()='Агиртамак']"));

            driver.FindElement(By.XPath("//span[text()='Северо-Западный']")).Click();
            driver.FindElement(By.XPath("//span[text()='Город Санкт-Петербург']")).Click();
            driver.FindElement(By.XPath("//span[text()='Санкт-Петербург']"));

            driver.FindElement(By.XPath("//span[text()='Северо-Кавказский']")).Click();
            driver.FindElement(By.XPath("//span[text()='Ингушетия']")).Click();
            driver.FindElement(By.XPath("//span[text()='Магас']"));

            driver.FindElement(By.XPath("//span[text()='Сибирский']")).Click();
            driver.FindElement(By.XPath("//span[text()='Алтай']")).Click();
            driver.FindElement(By.XPath("//span[text()='Онгудай']"));

            driver.FindElement(By.XPath("//span[text()='Уральский']")).Click();
            driver.FindElement(By.XPath("//span[text()='Тюменская область']")).Click();
            driver.FindElement(By.XPath("//span[text()='Тюмень']"));

            driver.FindElement(By.XPath("//span[text()='Центральный']")).Click();
            driver.FindElement(By.XPath("//span[text()='Город Москва']")).Click();
            driver.FindElement(By.XPath("//span[text()='Москва']"));

            driver.FindElement(By.XPath("//span[text()='Южный']")).Click();
            driver.FindElement(By.XPath("//span[text()='Краснодарский край']")).Click();
            string agoy = "Агой";
            driver.FindElement(By.XPath($"//span[text()='{agoy}']")).Click();
            Thread.Sleep(1000);
            string actual_selected = driver.FindElement(_city_select_loc).GetDomProperty("innerText");
            Assert.AreEqual(agoy, actual_selected, "Expected another text");
        }

        [TearDown]
        public void Quitting()
        {
            Console.WriteLine("Selenium webdriver quit");
            driver.Quit();
        }
    }
}
