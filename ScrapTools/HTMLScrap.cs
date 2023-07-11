using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scrapi.ScrapTools
{
    internal class HTMLScrap
    {
        #region Helpers
        private static HtmlNode CheckIfNullHtmlNode(HtmlNode htmlNode)
        {
            if (htmlNode != null)
            {
                if (htmlNode != null)
                {
                    return htmlNode;
                }
                else
                {
                    //return "DEFAULT-0";
                    return new HtmlNode(HtmlNodeType.Text,new HtmlDocument(),0);
                }
            }
            else
            {
                //return "DEFAULT-0";
                return new HtmlNode(HtmlNodeType.Text, new HtmlDocument(), 0);
            }
        }
        private static string? CheckIfNullHtmlNodeValue(HtmlNode htmlNode)
        {
            if (htmlNode != null)
            {
                if (!string.IsNullOrEmpty(htmlNode.InnerText))
                {
                    return htmlNode.InnerText.Trim();
                }
                else
                {
                    //return "DEFAULT-0";
                    return null;
                }
            }
            else
            {
                //return "DEFAULT-0";
                return null;
            }
        }
        private static string? CheckIfNullAttributeValue(HtmlAttribute htmlAttribute)
        {
            if (htmlAttribute != null)
            {
                if (!string.IsNullOrEmpty(htmlAttribute.Value))
                {
                    return htmlAttribute.Value.Trim();
                }
                else
                {
                    //return "DEFAULT-0";
                    return null;
                }
            }
            else
            {
                //return "DEFAULT-0";
                return null;
            }
        }
        #endregion

        #region Selenium
        public static int GetCountPage(string baseUrl, string tagName, string containsGetAttribute)
        {
            List<string> urlList = new List<string>();
            IWebDriver driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(baseUrl);

            ((IJavaScriptExecutor)driver).ExecuteScript(string.Format("window.scrollTo({0}, {1})", 9999, 999999));

            Thread.Sleep(5000);

            ICollection<IWebElement> links = driver.FindElements(By.TagName(tagName));
            foreach (IWebElement link in links)
            {
                if (link.GetAttribute("outerHTML").Contains(containsGetAttribute))
                    urlList.Add(link.Text);
            }

            driver.Close();
            driver.Quit();

            var number = 1;
            if (urlList != null)
            {
                if (urlList.Any())
                {
                    int.TryParse(urlList.Last(), out number);
                }
            }

            return number;
        }
        public static List<string> GetAllLinks(string url, string scherchHtmlCode,string tagName,string getAttribute)
        {
            List<string> urlList = new List<string>();
            IWebDriver driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(url);

            ((IJavaScriptExecutor)driver).ExecuteScript(string.Format("window.scrollTo({0}, {1})", 9999, 999999));

            Thread.Sleep(5000);

            ICollection<IWebElement> links = driver.FindElements(By.TagName(tagName));
            foreach (IWebElement link in links)
            {
                var obj = link.GetAttribute(getAttribute);
                if (obj != null)
                {
                    if (obj.Contains(scherchHtmlCode))
                        urlList.Add(link.GetAttribute(getAttribute).ToString());
                }
            }
            driver.Close();
            driver.Quit();
            return urlList;
        }
        #endregion

        #region HtmlAgilityPack
        public static HtmlNode? FindSelectorFromCode(string baseUrl, string scherchHtmlCode, string helperCodeLocation)
        {
            var htmlWebUrlConnection = new HtmlWeb();
            var document = htmlWebUrlConnection.Load(baseUrl);
            var schearchHtmlCodeList = document.QuerySelectorAll(scherchHtmlCode);

            if (schearchHtmlCodeList != null)
            {
                List<string> scherchHtmlCodeListCollection = new List<string>();

                for (int z = 0; z < schearchHtmlCodeList.Count; z++)
                {
                    scherchHtmlCodeListCollection.Add(schearchHtmlCodeList[z].OuterHtml);
                }

                var findVales = scherchHtmlCodeListCollection.Where(o => o.Contains(helperCodeLocation)).FirstOrDefault();

                if (findVales != null)
                {
                    var htmlDocUrlConnection = new HtmlDocument();
                    htmlDocUrlConnection.LoadHtml(findVales);
                    var htmlBodyUrlConnection = htmlDocUrlConnection.DocumentNode.QuerySelector(scherchHtmlCode);
                    return CheckIfNullHtmlNode(htmlBodyUrlConnection);
                }
            }
            return CheckIfNullHtmlNode(null);
        }
        public static string? FindValueSelectorFromCode(string baseUrl, string scherchHtmlCode, string helperCodeLocation)
        {
            var htmlWebUrlConnection = new HtmlWeb();
            var document = htmlWebUrlConnection.Load(baseUrl);
            var schearchHtmlCodeList = document.QuerySelectorAll(scherchHtmlCode);

            if (schearchHtmlCodeList != null)
            {
                List<string> scherchHtmlCodeListCollection = new List<string>();

                for (int z = 0; z < schearchHtmlCodeList.Count; z++)
                {
                    scherchHtmlCodeListCollection.Add(schearchHtmlCodeList[z].OuterHtml);
                }

                var findVales = scherchHtmlCodeListCollection.Where(o => o.Contains(helperCodeLocation)).FirstOrDefault();

                if (findVales != null)
                {
                    var htmlDocUrlConnection = new HtmlDocument();
                    htmlDocUrlConnection.LoadHtml(findVales);
                    var htmlBodyUrlConnection = htmlDocUrlConnection.DocumentNode.QuerySelector(scherchHtmlCode);
                    return CheckIfNullHtmlNodeValue(htmlBodyUrlConnection);
                }
            }
            return CheckIfNullHtmlNodeValue(null);
        }
        public static string? FindValueAttributesFromCode(string baseUrl, string scherchHtmlCode, string helperCodeLocation, string scherchSelector, string scherchAttribute)
        {
            var htmlWebUrlConnection = new HtmlWeb();
            var document = htmlWebUrlConnection.Load(baseUrl);
            var schearchHtmlCodeList = document.QuerySelectorAll(scherchHtmlCode);

            if (schearchHtmlCodeList != null)
            {
                List<string> scherchHtmlCodeListCollection = new List<string>();

                for (int z = 0; z < schearchHtmlCodeList.Count; z++)
                {
                    scherchHtmlCodeListCollection.Add(schearchHtmlCodeList[z].OuterHtml);
                }

                var findVales = scherchHtmlCodeListCollection.Where(o => o.Contains(helperCodeLocation)).FirstOrDefault();

                if (findVales != null)
                {
                    var htmlDocUrlConnection = new HtmlDocument();
                    htmlDocUrlConnection.LoadHtml(findVales);
                    var htmlBodyUrlConnection = htmlDocUrlConnection.DocumentNode.QuerySelector(scherchHtmlCode);

                    if (htmlBodyUrlConnection.QuerySelector(scherchSelector) != null)
                    {
                        if (htmlBodyUrlConnection.QuerySelector(scherchSelector).Attributes[scherchAttribute] != null)
                        {
                            return CheckIfNullAttributeValue(htmlBodyUrlConnection.QuerySelector(scherchSelector).Attributes[scherchAttribute]);
                        }
                    }
                }
            }
            return CheckIfNullAttributeValue(null);
        }
        public static List<HtmlNode> FindValueSelectorFromCodeList(string baseUrl, string scherchHtmlCode, string helperCodeLocation)
        {
            var htmlWebUrlConnection = new HtmlWeb();
            var document = htmlWebUrlConnection.Load(baseUrl);
            var schearchHtmlCodeList = document.QuerySelectorAll(scherchHtmlCode);
            List<HtmlNode> htmlNodeList = new List<HtmlNode>();

            if (schearchHtmlCodeList != null)
            {
                List<string> scherchHtmlCodeListCollection = new List<string>();

                for (int z = 0; z < schearchHtmlCodeList.Count; z++)
                {
                    scherchHtmlCodeListCollection.Add(schearchHtmlCodeList[z].OuterHtml);
                }

                foreach (var item in scherchHtmlCodeListCollection.Where(o => o.Contains(helperCodeLocation)))
                {
                    if (item != null)
                    {
                        var htmlDocUrlConnection = new HtmlDocument();
                        htmlDocUrlConnection.LoadHtml(item);
                        htmlNodeList.Add(htmlDocUrlConnection.DocumentNode.QuerySelector(scherchHtmlCode));
                     
                    }
                }
            }
            return htmlNodeList;
        }
        #endregion
    }
}
