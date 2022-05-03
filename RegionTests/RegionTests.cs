using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;

namespace RegionTests
{
    public class Tests
    {
        private IWebDriver driver;

        private readonly By _search_form_loc = By.XPath("//input[@placeholder='Найти город']");
        private readonly By _to_in_cities_loc = By.XPath("//li/a/span/mark");
        private readonly By _city_not_found_loc = By.CssSelector(".city-not-found");
        private const string _city_not_found_text = "Город не найден, возможно Вы имели ввиду:";

        private readonly By _city_select_loc = By.XPath("//div[contains(@class, 'header-top')]/div[contains(@class,'city-select')]/div/p");

        string[] _big_cities = { "Москва", "Санкт-Петербург", "Новосибирск", "Екатеринбург",
            "Нижний Новгород", "Казань", "Самара", "Владивосток" };

        private readonly By _region_loc = By.XPath("//ul[contains(@class, 'regions')]/li[contains(@style, 'display: block;')][2]");
        private readonly By _city_loc = By.XPath("//ul[contains(@class, 'cities')]/li[contains(@style, 'display: block;')][2]");
        string[] _regions = { "Дальневосточный", "Приволжский", "Северо-Западный", "Северо-Кавказский",
            "Сибирский", "Уральский", "Центральный", "Южный" };

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.dns-shop.ru");
            driver.FindElement(By.CssSelector(".pull-right")).Click();
            Thread.Sleep(3000);
        }

        [Test]
        public void Header_text()
        {
            driver.FindElement(By.XPath("//div[text()='Ваш город']"));
        }

        [Test]
        public void Search_form_prefix_of_city()
        {
            var search_form = driver.FindElement(_search_form_loc);
            string input_text = "То";
            search_form.SendKeys(input_text);
            var input_text_in_cities = driver.FindElement(_to_in_cities_loc).Text;
            Assert.AreEqual(input_text, input_text_in_cities, $"Expected {input_text}, but it is {input_text_in_cities}");
        }

        [Test]
        public void Search_form_random_word()
        {
            var search_form = driver.FindElement(_search_form_loc);
            search_form.SendKeys("Рандом");
            var actual_text = driver.FindElement(_city_not_found_loc).Text;
            Assert.AreEqual(_city_not_found_text, actual_text, $"Expected {_city_not_found_text}, but it is {actual_text}");
        }

        [Test]
        public void Search_form_city()
        {
            var search_form = driver.FindElement(_search_form_loc);
            string input_text = "Тольятти";
            search_form.SendKeys(input_text);
            driver.FindElement(By.XPath($"//mark[text()='{input_text}']")).Click();
            Thread.Sleep(1000);
            string actual_selected = driver.FindElement(_city_select_loc).GetDomProperty("innerText");
            Assert.AreEqual(input_text, actual_selected, $"Expected {input_text}, but it is {actual_selected}");
        }

        [Test]
        public void City_bubbles_check_items()
        {
            foreach (var city in _big_cities)
            {
                var actual_city = driver.FindElement(By.XPath($"//a[text()='{city}']")).Text;
                Assert.AreEqual(city, actual_city, $"Expected {city}, but it is {actual_city}");
            }
        }

        [Test]
        public void City_bubbles_click_random_bubble()
        {
            Random rnd = new Random();
            string random_city = _big_cities[rnd.Next(_big_cities.Length)];
            driver.FindElement(By.XPath($"//a[text()='{random_city}']")).Click();
            Thread.Sleep(1000);
            string actual_selected = driver.FindElement(_city_select_loc).GetDomProperty("innerText");
            Assert.AreEqual(random_city, actual_selected, $"Expected {random_city}, but it is {actual_selected}");
        }

        [Test]
        public void List_of_cities()
        {
            foreach (var region in _regions)
            {
                driver.FindElement(By.XPath($"//span[text()='{region}']")).Click();
                driver.FindElement(_region_loc).Click();
                var city = driver.FindElement(_city_loc);
                string city_name = city.Text;
                city.Click();

                var actual_city = driver.FindElement(_city_select_loc);
                string actual_city_text = actual_city.GetDomProperty("innerText");
                actual_city.Click();
                
                Assert.AreEqual(city_name, actual_city_text, $"Expected {city_name}, but it is {actual_city_text}");
            }
        }

        [TearDown]
        public void Quitting()
        {
            driver.Quit();
        }
    }
}
