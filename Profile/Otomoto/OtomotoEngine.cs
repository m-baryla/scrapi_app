using HtmlAgilityPack;
using System.Text.RegularExpressions;
using scrapi.Base.Model;
using scrapi.DtoBase;
using scrapi.ScrapTools;

namespace scrapi.Profile.Otomoto
{
    internal static class OtomotoEngine
    {
        public static List<string> GetUrl(string baseUrl)
        {
            List<string> urls = new List<string>();

            var lastPageNumber = HTMLScrap.GetCountPage(baseUrl, "li", "pagination-list-item");

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
            ModelScrap modelScrap;
            var urlList = GetUrl(baseUrl);

            foreach (var url in urlList)
            {
                subPageBaseUrlList = HTMLScrap.GetAllLinks(url, "otomoto.pl/osobowe/oferta/", "a", "href");

                foreach (var subPageBaseUrl in subPageBaseUrlList.Where(s => !s.Contains("otomotopay.pl") && !s.Contains("finansowanie")).ToList().Distinct())
                {
                    List<HtmlNode?> additionalInfo = HTMLScrap.FindValueSelectorFromCodeList(subPageBaseUrl, "ul", "class=\"offer-params__list\"");

                    string? additionalInfo1 = null;
                    string? additionalInfo2 = null;

                    if (additionalInfo.Any())
                    {
                        additionalInfo1 = additionalInfo[0].InnerText;
                        if (!string.IsNullOrEmpty(additionalInfo1))
                        {
                            additionalInfo1 = additionalInfo1.Trim().Replace("\n", " ");
                            additionalInfo1 = string.Join(" ", additionalInfo1.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                        }
                        additionalInfo2 = additionalInfo[1].InnerText;
                        if (!string.IsNullOrEmpty(additionalInfo2))
                        {
                            additionalInfo2 = additionalInfo2.Trim().Replace("\n", " ");
                            additionalInfo2 = string.Join(" ", additionalInfo2.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                        }
                    }
                   
                    modelScrap = new ModelScrap();
                    modelScrap.ScrapItem = scrapItem;

                    var tempOfferId = HTMLScrap.FindValueSelectorFromCode(subPageBaseUrl, "span", "<span class=\"offer-meta__value\" id=\"ad_id\"");
                    if (tempOfferId != null)
                    {
                        modelScrap.OfferId = string.Concat(scrapItem, "_", Regex.Match(input: tempOfferId, pattern: @"\b\d+\b"));
                    }

                    modelScrap.OfferName = HTMLScrap.FindValueSelectorFromCode(subPageBaseUrl, "span", "<span class=\"offer-title big-text fake-title \"");
                    modelScrap.Make = make;
                    modelScrap.Model = model;
                    modelScrap.PageBaseUrl = url;
                    modelScrap.SubPageBaseUrl = subPageBaseUrl;

                    var tempPrice = HTMLScrap.FindValueSelectorFromCode(subPageBaseUrl, "span", "<span class=\"offer-price__number\"");
                    if (tempPrice != null)
                    {
                        modelScrap.Price = Regex.Replace(input: tempPrice, pattern: @"\s+", replacement: "");
                    }

                    modelScrap.Location = HTMLScrap.FindValueSelectorFromCode(subPageBaseUrl, "a", "<a class=\"seller-card__links__link__cta\" href=\"#map\" title=\"Pokaż mapę\">");
                    modelScrap.AddedDate = HTMLScrap.FindValueSelectorFromCode(subPageBaseUrl, "span", "<span class=\"offer-meta__value\">");
                    modelScrap.AdditionalInfo1 = additionalInfo1;
                    modelScrap.AdditionalInfo2 = additionalInfo2;
                    modelScrap.AdditionalInfo3 = null;
                    modelScrap.AdditionalInfo4 = null;
                    modelScrap.AdditionalInfo5 = null;
                    modelScrap.AdditionalInfo6 = null;
                    modelScrap.AdditionalInfo7 = null;
                    modelScrap.AdditionalInfo8 = null;
                    modelScrap.AdditionalInfo9 = null;
                    modelScrap.AdditionalInfo10 = HTMLScrap.FindValueAttributesFromCode(subPageBaseUrl, "li", "<li class=\"offer-photos-thumbs__item\">", "img", "src");
                    dtoBase.AddNewScrap(modelScrap);
                }
            }
        }

    }
}
