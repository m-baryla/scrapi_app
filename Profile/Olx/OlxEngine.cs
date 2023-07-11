using HtmlAgilityPack.CssSelectors.NetCore;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using scrapi.Base.Model;
using scrapi.DtoBase;
using scrapi.ScrapTools;

namespace scrapi.Profile.Olx
{
    internal static class OlxEngine
    {
        private static List<string> GetUrl(string baseUrl)
        {
            List<string> urls = new List<string>();

            var lastPageNumber = HTMLScrap.GetCountPage(baseUrl, "a", "pagination-link");

            for (int i = 1; i <= lastPageNumber; i++)
            {
                urls.Add(baseUrl + "&page=" + i);
            }
            return urls;
        }
        public static void Scrap(string? scrapItem, string baseUrl, string make, string model, IDataBaseAcces dtoBase)
        {
            List<string> subPageBaseUrlList = new List<string>();
            List<string> offerIdList = new List<string>();
            ModelScrap modelScrap = new ModelScrap();
            var urlList = GetUrl(baseUrl);

            foreach (var url in urlList)
            {
                subPageBaseUrlList = HTMLScrap.GetAllLinks(url, "/oferta/","a","href").Where(s => s.Contains("olx")).ToList();

                foreach (var subPageBaseUrl in subPageBaseUrlList)
                {
                    var offerIdStr = HTMLScrap.FindSelectorFromCode(subPageBaseUrl, "span", "class=\"css-12hdxwj er34gjf0\"");

                    var offerId = Regex.Match(offerIdStr.QuerySelector("span").InnerText, @"\b\d+\b");

                    var olxData = REST.GET<RootobjectOLX>("https://m.olx.pl/api/v1/offers/", offerId.Value);

                    modelScrap.ScrapItem = scrapItem;
                    modelScrap.OfferName = olxData.Result.data.title;
                    modelScrap.OfferId = string.Concat(scrapItem,"_",offerId.Value);
                    modelScrap.Make = make;
                    modelScrap.Model = model;
                    modelScrap.PageBaseUrl = url;
                    modelScrap.SubPageBaseUrl = subPageBaseUrl;
                    modelScrap.Price = olxData.Result.data._params.Where(o => o.key == "price").Select(o => o.value.label).FirstOrDefault();
                    modelScrap.Location = string.Concat(olxData.Result.data.location.city.name,", ",olxData.Result.data.location.region.name);
                    modelScrap.AddedDate = olxData.Result.data.created_time.ToShortDateString();
                    //modelScrap.AdditionalInfo1 = olxData.Result.data._params.Where(o => o.key == "enginesize").Select(o => o.value.label).FirstOrDefault(); 
                    //modelScrap.AdditionalInfo2 = olxData.Result.data._params.Where(o => o.key == "year").Select(o => o.value.label).FirstOrDefault();
                    //modelScrap.AdditionalInfo3 = olxData.Result.data._params.Where(o => o.key == "milage").Select(o => o.value.label).FirstOrDefault();
                    //modelScrap.AdditionalInfo4 = null;
                    //modelScrap.AdditionalInfo5 = null;
                    modelScrap.AdditionalInfo1 = null; 
                    modelScrap.AdditionalInfo2 = null;
                    modelScrap.AdditionalInfo3 = null;
                    modelScrap.AdditionalInfo4 = null;
                    modelScrap.AdditionalInfo5 = null;
                    modelScrap.AdditionalInfo6 = null;
                    modelScrap.AdditionalInfo7 = null;
                    modelScrap.AdditionalInfo8 = null;
                    modelScrap.AdditionalInfo9 = null;
                    modelScrap.AdditionalInfo10 = olxData.Result.data.photos.FirstOrDefault().link.Replace("{width}x{height}", string.Concat(olxData.Result.data.photos.FirstOrDefault().width, "x", olxData.Result.data.photos.FirstOrDefault().height));
                    dtoBase.AddNewScrap(modelScrap);
                }
            }

        }
    }
    #region OLX DATA MODEL

    public class RootobjectOLX
    {
        public Data data { get; set; }
        public Links links { get; set; }
    }
    //[JsonObject(MemberSerialization.OptIn)]
    public class Data
    {
        public int id { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public DateTime last_refresh_time { get; set; }
        public DateTime created_time { get; set; }
        public DateTime valid_to_time { get; set; }
        public object pushup_time { get; set; }
        public string description { get; set; }
        public Promotion promotion { get; set; }
        [JsonProperty("params")]
        public Param[] _params { get; set; }
        public string[] key_params { get; set; }
        public bool business { get; set; }
        public User user { get; set; }
        public string status { get; set; }
        public Contact contact { get; set; }
        public Map map { get; set; }
        public Location location { get; set; }
        public Photo[] photos { get; set; }
        public object partner { get; set; }
        public Category category { get; set; }
        public Delivery delivery { get; set; }
        public Safedeal safedeal { get; set; }
        public Shop shop { get; set; }
        public string offer_type { get; set; }
        public bool protect_phone { get; set; }
    }

    public class Promotion
    {
        public bool highlighted { get; set; }
        public bool urgent { get; set; }
        public bool top_ad { get; set; }
        public object[] options { get; set; }
        public bool b2c_ad_page { get; set; }
        public bool premium_ad_page { get; set; }
    }

    public class User
    {
        public int id { get; set; }
        public DateTime created { get; set; }
        public bool other_ads_enabled { get; set; }
        public string name { get; set; }
        public object logo { get; set; }
        public object logo_ad_page { get; set; }
        public object social_network_account_type { get; set; }
        public object photo { get; set; }
        public string banner_mobile { get; set; }
        public string banner_desktop { get; set; }
        public string company_name { get; set; }
        public string about { get; set; }
        public bool b2c_business_page { get; set; }
        public bool is_online { get; set; }
        public DateTime last_seen { get; set; }
        public object seller_type { get; set; }
        public string uuid { get; set; }
    }

    public class Contact
    {
        public string name { get; set; }
        public bool phone { get; set; }
        public bool chat { get; set; }
        public bool negotiation { get; set; }
        public bool courier { get; set; }
    }

    public class Map
    {
        public int zoom { get; set; }
        public float lat { get; set; }
        public float lon { get; set; }
        public int radius { get; set; }
        public bool show_detailed { get; set; }
    }

    public class Location
    {
        public City city { get; set; }
        public Region region { get; set; }
    }

    public class City
    {
        public int id { get; set; }
        public string name { get; set; }
        public string normalized_name { get; set; }
    }

    public class Region
    {
        public int id { get; set; }
        public string name { get; set; }
        public string normalized_name { get; set; }
    }

    public class Category
    {
        public int id { get; set; }
        public string type { get; set; }
    }

    public class Delivery
    {
        public Rock rock { get; set; }
    }

    public class Rock
    {
        public object offer_id { get; set; }
        public bool active { get; set; }
        public string mode { get; set; }
    }

    public class Safedeal
    {
        public int weight { get; set; }
        public int weight_grams { get; set; }
        public string status { get; set; }
        public bool safedeal_blocked { get; set; }
        public object[] allowed_quantity { get; set; }
    }

    public class Shop
    {
        public object subdomain { get; set; }
    }

    public class Param
    {
        public string key { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public Value value { get; set; }
    }

    public class Value
    {
        public int value { get; set; }
        public string type { get; set; }
        public bool arranged { get; set; }
        public bool budget { get; set; }
        public string currency { get; set; }
        public bool negotiable { get; set; }
        public object converted_value { get; set; }
        public object previous_value { get; set; }
        public object converted_previous_value { get; set; }
        public object converted_currency { get; set; }
        public string label { get; set; }
        public string key { get; set; }
    }

    public class Photo
    {
        public long id { get; set; }
        public string filename { get; set; }
        public int rotation { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string link { get; set; }
    }

    public class Links
    {
        public Self self { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }


    #endregion
}

