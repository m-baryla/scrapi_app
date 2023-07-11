using Newtonsoft.Json;
using scrapi.Base.Model;
using scrapi.DtoBase;
using scrapi.ScrapTools;

namespace scrapig.Profile.Otodom
{
    internal static class OtodomEngine
    {
        private static List<string> GetUrl(string baseUrl)
        {
            List<string> urls = new List<string>();

            var lastPageNumber = HTMLScrap.GetCountPage(baseUrl, "button", "pagination.go-to-page");

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
                subPageBaseUrlList = HTMLScrap.GetAllLinks(url, "otodom.pl/pl/oferta/", "a", "href");

                foreach (var subPageBaseUrl in subPageBaseUrlList)
                {
                    var otodomScript = HTMLScrap.FindValueSelectorFromCode(subPageBaseUrl, "script", "id=\"__NEXT_DATA__\"");
                    var otodomData = JsonConvert.DeserializeObject<RootobjectOTODOM>(otodomScript);

                    modelScrap.ScrapItem = scrapItem;
                    modelScrap.OfferName = otodomData.props.pageProps.ad.title;
                    modelScrap.OfferId = string.Concat(scrapItem, "_",otodomData.props.pageProps.ad.id.ToString());
                    modelScrap.Make = make;
                    modelScrap.Model = model;
                    modelScrap.PageBaseUrl = url;
                    modelScrap.SubPageBaseUrl = subPageBaseUrl;
                    modelScrap.Price = otodomData.props.pageProps.ad.characteristics.Where(o=>o.key == "price").Select(o=>o.value).FirstOrDefault();
                    modelScrap.Location = string.Concat(otodomData.props.pageProps.ad.location.address.province.name,",", otodomData.props.pageProps.ad.location.address.city.name);
                    modelScrap.AddedDate = otodomData.props.pageProps.ad.createdAt.ToShortDateString();
                    modelScrap.AdditionalInfo1 = otodomData.props.pageProps.ad.characteristics.Where(o => o.key == "m").Select(o => o.value).FirstOrDefault();
                    modelScrap.AdditionalInfo2 = otodomData.props.pageProps.ad.characteristics.Where(o => o.key == "rooms_num").Select(o => o.value).FirstOrDefault(); 
                    modelScrap.AdditionalInfo3 = otodomData.props.pageProps.ad.owner.phones.FirstOrDefault();
                    modelScrap.AdditionalInfo4 = null;
                    modelScrap.AdditionalInfo5 = null;
                    modelScrap.AdditionalInfo6 = null;
                    modelScrap.AdditionalInfo7 = null;
                    modelScrap.AdditionalInfo8 = null;
                    modelScrap.AdditionalInfo9 = null;
                    modelScrap.AdditionalInfo10 = otodomData.props.pageProps.ad.images.Select(o => o.large).FirstOrDefault();
                    dtoBase.AddNewScrap(modelScrap);
                }
            }
        }

    }
    #region OTODOM DATA MODEL
    public class RootobjectOTODOM
    {
        public Props props { get; set; }
        public string page { get; set; }
        public Query query { get; set; }
        public string buildId { get; set; }
        public string assetPrefix { get; set; }
        public Runtimeconfig runtimeConfig { get; set; }
        public bool isFallback { get; set; }
        public int[] dynamicIds { get; set; }
        public bool gssp { get; set; }
        public bool customServer { get; set; }
        public bool appGip { get; set; }
        public object[] scriptLoader { get; set; }
    }

    public class Props
    {
        public Pageprops pageProps { get; set; }
        public bool __N_SSP { get; set; }
    }

    public class Pageprops
    {
        public string lang { get; set; }
        public string _sentryTraceData { get; set; }
        public string _sentryBaggage { get; set; }
        public Experiments experiments { get; set; }
        public string id { get; set; }
        public Ad ad { get; set; }
        public string relativeUrl { get; set; }
        public Adtrackingdata adTrackingData { get; set; }
        public object referer { get; set; }
        public Translations translations { get; set; }
        public object currentUser { get; set; }
        public Featureflags featureFlags { get; set; }
        public string laquesisResult { get; set; }
        public string userSessionId { get; set; }
        public object unconfirmedUserAuthToken { get; set; }
    }

    public class Experiments
    {
        public object EUADS2905 { get; set; }
        public object SFS313 { get; set; }
        public object SFS314 { get; set; }
        public object REMT569 { get; set; }
        public string SMR1687 { get; set; }
        public string SFS348 { get; set; }
        public object SEE1643 { get; set; }
        public object SEE1075 { get; set; }
        public string SMR2003 { get; set; }
    }

    public class Ad
    {
        public int id { get; set; }
        public string market { get; set; }
        public object[] services { get; set; }
        public string publicId { get; set; }
        public string slug { get; set; }
        public string advertiserType { get; set; }
        public string advertType { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime modifiedAt { get; set; }
        public string description { get; set; }
        public int developmentId { get; set; }
        public string developmentTitle { get; set; }
        public string developmentUrl { get; set; }
        public bool exclusiveOffer { get; set; }
        public string externalId { get; set; }
        public object[] features { get; set; }
        public object[] featuresByCategory { get; set; }
        public object[] featuresWithoutCategory { get; set; }
        public Openday openDay { get; set; }
        public string referenceId { get; set; }
        public Target target { get; set; }
        public string title { get; set; }
        public Topinformation[] topInformation { get; set; }
        public Additionalinformation[] additionalInformation { get; set; }
        public string url { get; set; }
        public string status { get; set; }
        public Adcategory adCategory { get; set; }
        public Category category { get; set; }
        public Characteristic[] characteristics { get; set; }
        public Image[] images { get; set; }
        public Links links { get; set; }
        public Location location { get; set; }
        public object property { get; set; }
        public Owner owner { get; set; }
        public Agency agency { get; set; }
        public Seo seo { get; set; }
        public Breadcrumb[] breadcrumbs { get; set; }
        public Useradvert[] userAdverts { get; set; }
        public Paginatedunits paginatedUnits { get; set; }
        public object specialOffer { get; set; }
        public string __typename { get; set; }
        public Contactdetails contactDetails { get; set; }
    }

    public class Openday
    {
        public object date { get; set; }
        public object timeFrom { get; set; }
        public object timeTo { get; set; }
        public string __typename { get; set; }
    }

    public class Target
    {
        public string Area { get; set; }
        public object[] AreaRange { get; set; }
        public string Build_year { get; set; }
        public string Building_floors_num { get; set; }
        public string City { get; set; }
        public string City_id { get; set; }
        public string Country { get; set; }
        public string[] Floor_no { get; set; }
        public string[] Heating { get; set; }
        public string Id { get; set; }
        public string ObidoAdvert { get; set; }
        public string OfferType { get; set; }
        public string Photo { get; set; }
        public int Price { get; set; }
        public string[] PriceRange { get; set; }
        public string ProperType { get; set; }
        public string Province { get; set; }
        public string RegularUser { get; set; }
        public string[] Rooms_num { get; set; }
        public string Subregion { get; set; }
        public string Title { get; set; }
        public string[] Windows_type { get; set; }
        public string categoryId { get; set; }
        public string env { get; set; }
        public string hidePrice { get; set; }
        public string seller_id { get; set; }
        public string user_type { get; set; }
    }

    public class Adcategory
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string __typename { get; set; }
    }

    public class Category
    {
        public int id { get; set; }
        public Name[] name { get; set; }
        public string __typename { get; set; }
    }

    public class Name
    {
        public string locale { get; set; }
        public string value { get; set; }
        public string __typename { get; set; }
    }

    public class Links
    {
        public string localPlanUrl { get; set; }
        public string videoUrl { get; set; }
        public string view3dUrl { get; set; }
        public string walkaroundUrl { get; set; }
        public string __typename { get; set; }
    }

    public class Location
    {
        public object id { get; set; }
        public Coordinates coordinates { get; set; }
        public Mapdetails mapDetails { get; set; }
        public Address address { get; set; }
        public string __typename { get; set; }
    }

    public class Coordinates
    {
        public float latitude { get; set; }
        public float longitude { get; set; }
        public string __typename { get; set; }
    }

    public class Mapdetails
    {
        public int radius { get; set; }
        public int zoom { get; set; }
        public string __typename { get; set; }
    }

    public class Address
    {
        public object street { get; set; }
        public object subdistrict { get; set; }
        public District district { get; set; }
        public City city { get; set; }
        public object municipality { get; set; }
        public County county { get; set; }
        public Province province { get; set; }
        public object postalCode { get; set; }
        public string __typename { get; set; }
    }

    public class District
    {
        public string id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string __typename { get; set; }
    }

    public class City
    {
        public string id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string __typename { get; set; }
    }

    public class County
    {
        public string id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string __typename { get; set; }
    }

    public class Province
    {
        public string id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string __typename { get; set; }
    }

    public class Owner
    {
        public int id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string[] phones { get; set; }
        public string imageUrl { get; set; }
        public object[] contacts { get; set; }
        public string __typename { get; set; }
    }

    public class Agency
    {
        public int id { get; set; }
        public string name { get; set; }
        public string licenseNumber { get; set; }
        public string type { get; set; }
        public string[] phones { get; set; }
        public string address { get; set; }
        public string imageUrl { get; set; }
        public string url { get; set; }
        public string leaderYear { get; set; }
        public bool brandingVisible { get; set; }
        public string[] enabledFeatures { get; set; }
        public string __typename { get; set; }
    }

    public class Seo
    {
        public string title { get; set; }
        public string description { get; set; }
        public string __typename { get; set; }
    }

    public class Paginatedunits
    {
        public object items { get; set; }
        public object isPriceHidden { get; set; }
        public object pagination { get; set; }
        public object facets { get; set; }
        public string __typename { get; set; }
    }

    public class Contactdetails
    {
        public string name { get; set; }
        public string type { get; set; }
        public string[] phones { get; set; }
        public string imageUrl { get; set; }
    }

    public class Topinformation
    {
        public string label { get; set; }
        public string[] values { get; set; }
        public string unit { get; set; }
        public string __typename { get; set; }
    }

    public class Additionalinformation
    {
        public string label { get; set; }
        public string[] values { get; set; }
        public string unit { get; set; }
        public string __typename { get; set; }
    }

    public class Characteristic
    {
        public string key { get; set; }
        public string value { get; set; }
        public string label { get; set; }
        public string localizedValue { get; set; }
        public string currency { get; set; }
        public string suffix { get; set; }
        public string __typename { get; set; }
    }

    public class Image
    {
        public string thumbnail { get; set; }
        public string small { get; set; }
        public string medium { get; set; }
        public string large { get; set; }
        public string __typename { get; set; }
    }

    public class Breadcrumb
    {
        public string label { get; set; }
        public string locative { get; set; }
        public string url { get; set; }
        public string __typename { get; set; }
    }

    public class Useradvert
    {
        public string url { get; set; }
        public string image { get; set; }
        public string roomsNum { get; set; }
        public int pricePerM { get; set; }
        public Price price { get; set; }
        public string netArea { get; set; }
        public string title { get; set; }
        public string __typename { get; set; }
    }

    public class Price
    {
        public string value { get; set; }
        public string unit { get; set; }
        public string __typename { get; set; }
    }

    public class Adtrackingdata
    {
        public string touch_point_page { get; set; }
        public float lat { get; set; }
        public float _long { get; set; }
        public int ad_photo { get; set; }
        public string price_currency { get; set; }
        public int cat_l1_id { get; set; }
        public string cat_l1_name { get; set; }
        public object special_offer_type { get; set; }
        public string ad_id { get; set; }
        public int ad_price { get; set; }
        public string business { get; set; }
        public string city_id { get; set; }
        public string city_name { get; set; }
        public object market { get; set; }
        public string poster_type { get; set; }
        public object surface { get; set; }
        public string region_id { get; set; }
        public string region_name { get; set; }
        public string subregion_id { get; set; }
        public string seller_id { get; set; }
        public string obido_advert { get; set; }
    }

    public class Translations
    {
        public string frontendadcontentdescription { get; set; }
        public string frontendadcontactpersonbannertitle { get; set; }
        public string frontendadfeaturesadditionalinfo { get; set; }
        public string frontendadpagemaptitle { get; set; }
        public string frontendlanguageswitcherplshort { get; set; }
        public string frontendnavbarmenuads { get; set; }
        public string frontendnavbarmenuadsapartmentsforsale { get; set; }
        public string frontendnavbarmenuadsapartmentsforrent { get; set; }
        public string frontendnavbarmenuadshousesforsale { get; set; }
        public string frontendnavbarmenuadshousesforrent { get; set; }
        public string frontendnavbarmenuadscommercialsforrent { get; set; }
        public string frontendnavbarmenuadsplotsforsale { get; set; }
        public string frontendnavbarmenuprimarymarket { get; set; }
        public string frontendnavbarmenuprimarymarkettypenewflats { get; set; }
        public string frontendnavbarmenuprimarymarkettypenewhouses { get; set; }
        public string frontendnewnavbarmenuprimarymarkettopdevelopers { get; set; }
        public string frontendnavbarmenucompanies { get; set; }
        public string frontendnavbarmenucompaniesagencies { get; set; }
        public string frontendnavbarmenucompaniesdevelopers { get; set; }
        public string frontendnavbarmenuarticles { get; set; }
        public string frontendnavbarmenudata { get; set; }
        public string frontendnavbarmenucredits { get; set; }
        public string frontendnavusermenucompleteregistration { get; set; }
        public string frontendnavusermenuadverts { get; set; }
        public string frontendnavusermenuanswers { get; set; }
        public string frontendnavusermenuwallet { get; set; }
        public string frontendnavusermenusettings { get; set; }
        public string frontendnavusermenusummary { get; set; }
        public string frontendnavusermenufavouriteadverts { get; set; }
        public string frontendnavusermenufavouritesearches { get; set; }
        public string frontendfooterlinksgeneralaboutus { get; set; }
        public string frontendfooterlinksgeneralhelpcenter { get; set; }
        public string frontendfooterlinksgeneralcontactclient { get; set; }
        public string frontendfooterlinksgeneralcontactsales { get; set; }
        public string frontendfooterlinksgeneralprices { get; set; }
        public string frontendfooterlinksgeneraltermsofuse { get; set; }
        public string frontendfooterlinksgeneralprivacypolicy { get; set; }
        public string frontendfooterlinksgeneralpresscenter { get; set; }
        public string frontendfooterlinksgeneralblog { get; set; }
        public string frontendfooterlinksservicestoolsforagencies { get; set; }
        public string frontendfooterlinksservicessellwithotodom { get; set; }
        public string frontendfooterlinksservicesfindanagent { get; set; }
        public string frontendfooterlinksservicesfindadeveloper { get; set; }
        public string frontendfooterlinkssitemapcategorymap { get; set; }
        public string frontendfooterlinkssitemapcitiesmap { get; set; }
        public string frontendfooterlinkssitemaparticlesandtips { get; set; }
        public string frontendfooterlinkssitemapcompaniesbase { get; set; }
        public string frontendfooterlinkssitemapcareers { get; set; }
        public string frontendfooterlinksappsolxpl { get; set; }
        public string frontendfooterlinksappsotomotopl { get; set; }
        public string frontendfooterlinksappsfixly { get; set; }
        public string frontendfooterlinksappsobidopl { get; set; }
        public string frontendadpageadagencybrandingshowallshort { get; set; }
        public string frontendadpageadagencybrandingshowall { get; set; }
        public string frontendbreadcrumbsgobacktolist { get; set; }
        public string frontendadcontactformbottomtitle { get; set; }
        public string frontendpostingformformdropdownfieldvalueanteroom { get; set; }
        public string frontendpostingformformdropdownfieldvalueattractivepaymentschedule { get; set; }
        public string frontendpostingformformdropdownfieldvaluebasement { get; set; }
        public string frontendpostingformformdropdownfieldvaluebathroom { get; set; }
        public string frontendpostingformformdropdownfieldvaluebedroom { get; set; }
        public string frontendpostingformformdropdownfieldvaluebelongingclosedareaparking { get; set; }
        public string frontendpostingformformdropdownfieldvaluebelonginggroundparking { get; set; }
        public string frontendpostingformformdropdownfieldvaluebuildingmaterialbreezeblock { get; set; }
        public string frontendpostingformformdropdownfieldvaluebuildingmaterialbrick { get; set; }
        public string frontendpostingformformdropdownfieldvaluebuildingmaterialcellularconcrete { get; set; }
        public string frontendpostingformformdropdownfieldvaluebuildingmaterialconcrete { get; set; }
        public string frontendpostingformformdropdownfieldvaluebuildingmaterialconcreteplate { get; set; }
        public string frontendpostingformformdropdownfieldvaluebuildingmaterialhydroton { get; set; }
        public string frontendpostingformformdropdownfieldvaluebuildingmaterialother { get; set; }
        public string frontendpostingformformdropdownfieldvaluebuildingmaterialreinforcedconcrete { get; set; }
        public string frontendpostingformformdropdownfieldvaluebuildingmaterialsilikat { get; set; }
        public string frontendpostingformformdropdownfieldvaluebuildingmaterialwood { get; set; }
        public string frontendpostingformformdropdownfieldvaluebuildingownershipcooperativeownership { get; set; }
        public string frontendpostingformformdropdownfieldvaluebuildingownershipcooperativeownershipwithalandandmortgageregiste { get; set; }
        public string frontendpostingformformdropdownfieldvaluebuildingownershipfullownership { get; set; }
        public string frontendpostingformformdropdownfieldvaluebuildingownershipshare { get; set; }
        public string frontendpostingformformdropdownfieldvaluebuildingtypeapartment { get; set; }
        public string frontendpostingformformdropdownfieldvaluebuildingtypeblock { get; set; }
        public string frontendpostingformformdropdownfieldvaluebuildingtypehouse { get; set; }
        public string frontendpostingformformdropdownfieldvaluebuildingtypeinfill { get; set; }
        public string frontendpostingformformdropdownfieldvaluebuildingtypeloft { get; set; }
        public string frontendpostingformformdropdownfieldvaluebuildingtyperibbon { get; set; }
        public string frontendpostingformformdropdownfieldvaluebuildingtypetenement { get; set; }
        public string frontendpostingformformdropdownfieldvaluecommercialproperties { get; set; }
        public string frontendpostingformformdropdownfieldvalueconstructionstatusnewreadytouse { get; set; }
        public string frontendpostingformformdropdownfieldvalueconstructionstatusreadytouse { get; set; }
        public string frontendpostingformformdropdownfieldvalueconstructionstatusrenovated { get; set; }
        public string frontendpostingformformdropdownfieldvalueconstructionstatustocompletion { get; set; }
        public string frontendpostingformformdropdownfieldvalueconstructionstatustorenovation { get; set; }
        public string frontendpostingformformdropdownfieldvalueflats { get; set; }
        public string frontendpostingformformdropdownfieldvaluefloor2 { get; set; }
        public string frontendpostingformformdropdownfieldvaluefloor4 { get; set; }
        public string frontendpostingformformdropdownfieldvaluefloor5 { get; set; }
        public string frontendpostingformformdropdownfieldvaluefloor6 { get; set; }
        public string frontendpostingformformdropdownfieldvaluefloor7 { get; set; }
        public string frontendpostingformformdropdownfieldvaluefloor8 { get; set; }
        public string frontendpostingformformdropdownfieldvaluefloorhigher10 { get; set; }
        public string frontendpostingformformdropdownfieldvaluefloornocellar { get; set; }
        public string frontendpostingformformdropdownfieldvaluefloornofloor1 { get; set; }
        public string frontendpostingformformdropdownfieldvaluefloornofloor10 { get; set; }
        public string frontendpostingformformdropdownfieldvaluefloornofloor2 { get; set; }
        public string frontendpostingformformdropdownfieldvaluefloornofloor3 { get; set; }
        public string frontendpostingformformdropdownfieldvaluefloornofloor4 { get; set; }
        public string frontendpostingformformdropdownfieldvaluefloornofloor5 { get; set; }
        public string frontendpostingformformdropdownfieldvaluefloornofloor6 { get; set; }
        public string frontendpostingformformdropdownfieldvaluefloornofloor7 { get; set; }
        public string frontendpostingformformdropdownfieldvaluefloornofloor8 { get; set; }
        public string frontendpostingformformdropdownfieldvaluefloornofloor9 { get; set; }
        public string frontendpostingformformdropdownfieldvaluefloornofloorhigher10 { get; set; }
        public string frontendpostingformformdropdownfieldvaluefloornogarret { get; set; }
        public string frontendpostingformformdropdownfieldvaluefloornogroundfloor { get; set; }
        public string frontendpostingformformdropdownfieldvaluefreegroundparking { get; set; }
        public string frontendpostingformformdropdownfieldvaluefreeparking { get; set; }
        public string frontendpostingformformdropdownfieldvaluegarage { get; set; }
        public string frontendpostingformformdropdownfieldvaluehall { get; set; }
        public string frontendpostingformformdropdownfieldvalueheatingboilerroom { get; set; }
        public string frontendpostingformformdropdownfieldvalueheatingelectrical { get; set; }
        public string frontendpostingformformdropdownfieldvalueheatinggas { get; set; }
        public string frontendpostingformformdropdownfieldvalueheatingother { get; set; }
        public string frontendpostingformformdropdownfieldvalueheatingtiledstove { get; set; }
        public string frontendpostingformformdropdownfieldvalueheatingurban { get; set; }
        public string frontendpostingformformdropdownfieldvaluehouses { get; set; }
        public string frontendpostingformformdropdownfieldvalueinbuilding { get; set; }
        public string frontendpostingformformdropdownfieldvalueintegralgarage { get; set; }
        public string frontendpostingformformdropdownfieldvaluekitchen { get; set; }
        public string frontendpostingformformdropdownfieldvaluekitchentypeopen { get; set; }
        public string frontendpostingformformdropdownfieldvaluekitchentypeseparate { get; set; }
        public string frontendpostingformformdropdownfieldvaluekitchentypewithwindow { get; set; }
        public string frontendpostingformformdropdownfieldvaluelastminuteoffer { get; set; }
        public string frontendpostingformformdropdownfieldvaluelivingroom { get; set; }
        public string frontendpostingformformdropdownfieldvaluelivingroomwithopenkitchen { get; set; }
        public string frontendpostingformformdropdownfieldvaluenone { get; set; }
        public string frontendpostingformformdropdownfieldvaluenotstarted { get; set; }
        public string frontendpostingformformdropdownfieldvalueparkingspace { get; set; }
        public string frontendpostingformformdropdownfieldvalueparkingspacedetachedgarage { get; set; }
        public string frontendpostingformformdropdownfieldvalueparkingspaceenclosedareaprivateparking { get; set; }
        public string frontendpostingformformdropdownfieldvalueparkingspacegarageinthebuilding { get; set; }
        public string frontendpostingformformdropdownfieldvalueparkingspacenoparking { get; set; }
        public string frontendpostingformformdropdownfieldvalueparkingspacestreetparking { get; set; }
        public string frontendpostingformformdropdownfieldvalueparkingspacestreetprivateparking { get; set; }
        public string frontendpostingformformdropdownfieldvalueparkingspaceundergroundparking { get; set; }
        public string frontendpostingformformdropdownfieldvaluepresaleoffer { get; set; }
        public string frontendpostingformformdropdownfieldvaluepricediscount { get; set; }
        public string frontendpostingformformdropdownfieldvaluepropertytypeapartment { get; set; }
        public string frontendpostingformformdropdownfieldvaluepropertytypeduplex { get; set; }
        public string frontendpostingformformdropdownfieldvaluepropertytypepenthouse { get; set; }
        public string frontendpostingformformdropdownfieldvaluepropertytypestudio { get; set; }
        public string frontendpostingformformdropdownfieldvaluepropertytypetriplex { get; set; }
        public string frontendpostingformformdropdownfieldvaluepropertytypewithmezzanine { get; set; }
        public string frontendpostingformformdropdownfieldvalueready { get; set; }
        public string frontendpostingformformdropdownfieldvaluerestroom { get; set; }
        public string frontendpostingformformdropdownfieldvalueroom { get; set; }
        public string frontendpostingformformdropdownfieldvaluestorageroom { get; set; }
        public string frontendpostingformformdropdownfieldvalueusedrenovated { get; set; }
        public string frontendpostingformformdropdownfieldvaluewindowsorientationeast { get; set; }
        public string frontendpostingformformdropdownfieldvaluewindowsorientationnorth { get; set; }
        public string frontendpostingformformdropdownfieldvaluewindowsorientationsouth { get; set; }
        public string frontendpostingformformdropdownfieldvaluewindowsorientationwest { get; set; }
        public string frontendpostingformformdropdownfieldvaluewindowstypealuminium { get; set; }
        public string frontendpostingformformdropdownfieldvaluewindowstypeplastic { get; set; }
        public string frontendpostingformformdropdownfieldvaluewindowstypethermopaneglass { get; set; }
        public string frontendpostingformformdropdownfieldvaluewindowstypewooden { get; set; }
        public string frontendadheaderadditionalcostsdescription { get; set; }
        public string frontendadheaderexclusiveoffertagsecondary { get; set; }
        public string frontendadheaderdevelopername { get; set; }
        public string frontendadheaderdeveloper { get; set; }
        public string frontendadheadersuggestprice { get; set; }
        public string frontendadheaderpropertyinvestmentpart { get; set; }
        public string frontendadheaderinvestmentunitarearange { get; set; }
        public string frontendadheaderareato { get; set; }
        public string frontendadheaderpricepermeter { get; set; }
        public string frontendadheadernegotiablebadge { get; set; }
        public string frontendadheaderpriceincludes { get; set; }
        public string frontendadheadmetadataseotitlesellwithusprefix { get; set; }
        public string frontendprojectpageinvestmentunitstitle { get; set; }
        public string frontendadinvestmentunitstitle { get; set; }
        public string frontendadreleaderbannercomponentlabel { get; set; }
        public string frontendadreleaderbannericon { get; set; }
        public string frontendadreleaderbannertitle { get; set; }
        public string frontendadreleaderbannerlabel { get; set; }
        public string frontendadmetaofferid { get; set; }
        public string frontendadmetaexternalid { get; set; }
        public string frontendadmetadatepublished { get; set; }
        public string frontendadmetadatemodified { get; set; }
        public string frontendadoverviewdefaultoverview { get; set; }
        public string frontendadoverviewinvestmentoverview { get; set; }
        public string frontendadremoteservicemovie { get; set; }
        public string frontendadremoteservicevirtualwalk { get; set; }
        public string frontendadremoteservicelocalplan { get; set; }
        public string frontendadremoteservicetitle { get; set; }
        public string frontendadremoteserviceopenday { get; set; }
        public string frontendadremoteservicedescription { get; set; }
        public string frontendadremoteservicesee { get; set; }
        public string frontendaduseradsmoreadsfrom { get; set; }
        public string frontendaduseradsthisauthor { get; set; }
        public string frontendadmultimediasectiongallery { get; set; }
        public string frontendadmultimediasectionvideo { get; set; }
        public string frontendadmultimediasectionvirtualwalkaround { get; set; }
        public string frontendadmultimediasectionvirtualrenovation { get; set; }
        public string frontendadmultimediasectionurbanplan { get; set; }
        public string frontendadmultimediasectionapartmentplan { get; set; }
        public string frontendglobaldatetoday { get; set; }
        public string frontendglobaldatetomorrow { get; set; }
        public string frontendadremoteserviceopendayshow { get; set; }
        public string frontendsubscribetoastlimitreached { get; set; }
        public string frontendtoasterror { get; set; }
        public string frontendsubscribetoastsuccess { get; set; }
        public string frontendsubscribetoastsuccessremove { get; set; }
        public string frontendlaquesisSurveydecline { get; set; }
        public string frontendlaquesisSurveyaccept { get; set; }
        public string frontendlaquesisSurveynext { get; set; }
        public string frontendlaquesisSurveyfinish { get; set; }
        public string frontendlaquesisSurveyradioGroupHint { get; set; }
        public string frontendlaquesisSurveycheckBoxGroupHint { get; set; }
        public string frontendlaquesisSurveymultiLineInputHint { get; set; }
        public string frontendlaquesisSurveysingleInputHint { get; set; }
        public string frontendadobserveadmodaltitle { get; set; }
        public string frontendadsimilarofferssubscriptiontoastsuccess { get; set; }
        public string frontendsearchitemadded { get; set; }
        public string frontendsearchitemaskforprice { get; set; }
        public string frontendsearchitemcommercialpropertyforrent { get; set; }
        public string frontendsearchitemcommercialpropertyforsell { get; set; }
        public string frontendsearchitemcontactmobile { get; set; }
        public string frontendsearchitemdevelopmentinvestment { get; set; }
        public string frontendsearchitemestateagency { get; set; }
        public string frontendsearchitemflatforrent { get; set; }
        public string frontendsearchitemflatforsell { get; set; }
        public string frontendsearchitemfloorplan { get; set; }
        public string frontendsearchitemfrom { get; set; }
        public string frontendsearchitemgarageforrent { get; set; }
        public string frontendsearchitemgarageforsell { get; set; }
        public string frontendsearchitemhallforrent { get; set; }
        public string frontendsearchitemhallforsell { get; set; }
        public string frontendsearchitemhouseforrent { get; set; }
        public string frontendsearchitemhouseforsell { get; set; }
        public string frontendsearchitemignoretogglebuttonlabel { get; set; }
        public string frontendsearchiteminvestment { get; set; }
        public string frontendsearchiteminvestmentStatein_building { get; set; }
        public string frontendsearchiteminvestmentStatenot_started { get; set; }
        public string frontendsearchiteminvestmentStateready { get; set; }
        public string frontendsearchitemmetaapartments { get; set; }
        public string frontendsearchitemmetadatequarter { get; set; }
        public string frontendsearchitemmetafinishdate { get; set; }
        public string frontendsearchitemmetafrom { get; set; }
        public string frontendsearchitemmetaplot { get; set; }
        public string frontendsearchitemmetarooms { get; set; }
        public string frontendsearchitemmetasquaremeters { get; set; }
        public string frontendsearchitemmetato { get; set; }
        public string frontendsearchitemofficeforrent { get; set; }
        public string frontendsearchitemofficeforsell { get; set; }
        public string frontendsearchitemopenday { get; set; }
        public string frontendsearchitemopeninnewtab { get; set; }
        public string frontendsearchitemotodomonly { get; set; }
        public string frontendsearchitempeoplePerRoomfour { get; set; }
        public string frontendsearchitempeoplePerRoomone { get; set; }
        public string frontendsearchitempeoplePerRoomthree { get; set; }
        public string frontendsearchitempeoplePerRoomtwo { get; set; }
        public string frontendsearchitempermonth { get; set; }
        public string frontendsearchitempermonthfull { get; set; }
        public string frontendsearchitemprivateoffer { get; set; }
        public string frontendsearchitemrefreshed { get; set; }
        public string frontendsearchitemrefreshedtooltipbumpdefinition { get; set; }
        public string frontendsearchitemrefreshedtooltiptext { get; set; }
        public string frontendsearchitemremoteservices { get; set; }
        public string frontendsearchitemrent { get; set; }
        public string frontendsearchitemrentfind { get; set; }
        public string frontendsearchitemroomforrent { get; set; }
        public string frontendsearchitemroomforsell { get; set; }
        public string frontendsearchitemrooms { get; set; }
        public string frontendsearchitemroomspluralgenitive { get; set; }
        public string frontendsearchitemroomspluralnominative { get; set; }
        public string frontendsearchitemroomssingularnominative { get; set; }
        public string frontendsearchitemseead { get; set; }
        public string frontendsearchitemstudioflatforrent { get; set; }
        public string frontendsearchitemstudioflatforsell { get; set; }
        public string frontendsearchitemterrainforrent { get; set; }
        public string frontendsearchitemterrainforsell { get; set; }
        public string frontendsearchitemto { get; set; }
        public string frontendsearchitemvideo { get; set; }
        public string frontendsearchitemview3d { get; set; }
        public string frontendsearchitemvirtualwalk { get; set; }
        public string frontendadstickyheadercontact { get; set; }
        public string frontendadcontactformadtypeprivate { get; set; }
        public string frontendadcontactformadtypebusiness { get; set; }
        public string frontendadcontactformadtypedeveloper { get; set; }
        public string frontendadcontactformadtypeconsultant { get; set; }
        public string frontendadcontactformadtypemanager { get; set; }
        public string frontendadcontactformadtypepromoter { get; set; }
        public string frontendadcontactpersonbannersendmessage { get; set; }
        public string frontendadavmmodulemaintitle { get; set; }
        public string frontendadavmmodulemainsubtitleb { get; set; }
        public string frontendadavmmoduleprice { get; set; }
        public string frontendadavmmodulelisttitle { get; set; }
        public string frontendadavmmodulelistpropertydescription { get; set; }
        public string frontendadavmmodulelistlocation { get; set; }
        public string frontendadavmmodulelistenvironment { get; set; }
        public string frontendadavmmodulelistinfrastructure { get; set; }
        public string frontendadavmmodulelistneighborhoodprices { get; set; }
        public string frontendadsidebarstickycontactformlabel { get; set; }
        public string frontendadtableinformationlabelaccess_types { get; set; }
        public string frontendadtableinformationlabeladditional_areas { get; set; }
        public string frontendadtableinformationlabeladditional_cost { get; set; }
        public string frontendadtableinformationlabeladvertiser_type { get; set; }
        public string frontendadtableinformationlabelarea { get; set; }
        public string frontendadtableinformationlabelarea_range { get; set; }
        public string frontendadtableinformationlabelbuilding_material { get; set; }
        public string frontendadtableinformationlabelbuilding_ownership { get; set; }
        public string frontendadtableinformationlabelbuilding_type { get; set; }
        public string frontendadtableinformationlabelbuilding_year { get; set; }
        public string frontendadtableinformationlabelbuild_year { get; set; }
        public string frontendadtableinformationlabelcar { get; set; }
        public string frontendadtableinformationlabelceiling_height_from { get; set; }
        public string frontendadtableinformationlabelceiling_height_range { get; set; }
        public string frontendadtableinformationlabelceiling_height_to { get; set; }
        public string frontendadtableinformationlabelcondition { get; set; }
        public string frontendadtableinformationlabelconstruction_material { get; set; }
        public string frontendadtableinformationlabelconstruction_status { get; set; }
        public string frontendadtableinformationlabelconstruction_year { get; set; }
        public string frontendadtableinformationlabeldeposit { get; set; }
        public string frontendadtableinformationlabeldimensions { get; set; }
        public string frontendadtableinformationlabelequipment_types { get; set; }
        public string frontendadtableinformationlabelextras_types { get; set; }
        public string frontendadtableinformationlabelextra_spaces { get; set; }
        public string frontendadtableinformationlabelfence { get; set; }
        public string frontendadtableinformationlabelfence_types { get; set; }
        public string frontendadtableinformationlabelfloor { get; set; }
        public string frontendadtableinformationlabelflooring { get; set; }
        public string frontendadtableinformationlabelfloors_num { get; set; }
        public string frontendadtableinformationlabelfree_from { get; set; }
        public string frontendadtableinformationlabelgarret_type { get; set; }
        public string frontendadtableinformationlabelgross_area { get; set; }
        public string frontendadtableinformationlabelheating { get; set; }
        public string frontendadtableinformationlabelheating_types { get; set; }
        public string frontendadtableinformationlabelheight { get; set; }
        public string frontendadtableinformationlabelkitchen { get; set; }
        public string frontendadtableinformationlabellift { get; set; }
        public string frontendadtableinformationlabellighting { get; set; }
        public string frontendadtableinformationlabellocalization { get; set; }
        public string frontendadtableinformationlabellocation { get; set; }
        public string frontendadtableinformationlabelmarket { get; set; }
        public string frontendadtableinformationlabelmedia_types { get; set; }
        public string frontendadtableinformationlabelnon_smokers_only { get; set; }
        public string frontendadtableinformationlabelnumber_of_adverts { get; set; }
        public string frontendadtableinformationlabelnumber_of_floors_project { get; set; }
        public string frontendadtableinformationlabelnumber_of_properties { get; set; }
        public string frontendadtableinformationlabelnumber_of_units_in_project { get; set; }
        public string frontendadtableinformationlabeloffered_estates_type { get; set; }
        public string frontendadtableinformationlabeloffered_estates_types_project { get; set; }
        public string frontendadtableinformationlabeloffice_space { get; set; }
        public string frontendadtableinformationlabeloutdoor { get; set; }
        public string frontendadtableinformationlabelparking { get; set; }
        public string frontendadtableinformationlabelpremises_location { get; set; }
        public string frontendadtableinformationlabelproject_amenities { get; set; }
        public string frontendadtableinformationlabelproject_begin_date { get; set; }
        public string frontendadtableinformationlabelproject_finish_date { get; set; }
        public string frontendadtableinformationlabelproperty_type { get; set; }
        public string frontendadtableinformationlabelramp { get; set; }
        public string frontendadtableinformationlabelrecreational { get; set; }
        public string frontendadtableinformationlabelremote_services { get; set; }
        public string frontendadtableinformationlabelrent { get; set; }
        public string frontendadtableinformationlabelrent_to_students { get; set; }
        public string frontendadtableinformationlabelroofing { get; set; }
        public string frontendadtableinformationlabelroof_type { get; set; }
        public string frontendadtableinformationlabelroomsize { get; set; }
        public string frontendadtableinformationlabelrooms_num { get; set; }
        public string frontendadtableinformationlabelroom_usable_area { get; set; }
        public string frontendadtableinformationlabelsecurity { get; set; }
        public string frontendadtableinformationlabelsecurity_types { get; set; }
        public string frontendadtableinformationlabelsocial_facilities { get; set; }
        public string frontendadtableinformationlabelstate { get; set; }
        public string frontendadtableinformationlabelstructure { get; set; }
        public string frontendadtableinformationlabelterrain_area { get; set; }
        public string frontendadtableinformationlabelterrain_type { get; set; }
        public string frontendadtableinformationlabeluse_types { get; set; }
        public string frontendadtableinformationlabelvicinity_types { get; set; }
        public string frontendadtableinformationlabelwindows { get; set; }
        public string frontendadtableinformationlabelwindows_type { get; set; }
        public string frontendadtableinformationroomshall { get; set; }
        public string frontendadtableinformationroomskitchen { get; set; }
        public string frontendadtableinformationvaluen { get; set; }
        public string frontendadtableinformationvaluey { get; set; }
        public string frontendadtableinformationvalueabovetenth { get; set; }
        public string frontendadtableinformationvalueaccess_typesasphalt { get; set; }
        public string frontendadtableinformationvalueaccess_typesdirt { get; set; }
        public string frontendadtableinformationvalueaccess_typeshard_surfaced { get; set; }
        public string frontendadtableinformationvalueaccess_typessoft_surfaced { get; set; }
        public string frontendadtableinformationvalueadvertiser_typeagency { get; set; }
        public string frontendadtableinformationvalueadvertiser_typedeveloper { get; set; }
        public string frontendadtableinformationvalueadvertiser_typeprivate { get; set; }
        public string frontendadtableinformationvalueapartment_room_count10ormore { get; set; }
        public string frontendadtableinformationvaluebalcony { get; set; }
        public string frontendadtableinformationvalueboilerroom { get; set; }
        public string frontendadtableinformationvaluebuilding_material117brick { get; set; }
        public string frontendadtableinformationvaluebuilding_material117wood { get; set; }
        public string frontendadtableinformationvaluebuilding_material69brick { get; set; }
        public string frontendadtableinformationvaluebuilding_material69wood { get; set; }
        public string frontendadtableinformationvaluebuilding_materialbreezeblock { get; set; }
        public string frontendadtableinformationvaluebuilding_materialbrick { get; set; }
        public string frontendadtableinformationvaluebuilding_materialcellular_concrete { get; set; }
        public string frontendadtableinformationvaluebuilding_materialconcrete { get; set; }
        public string frontendadtableinformationvaluebuilding_materialconcrete_plate { get; set; }
        public string frontendadtableinformationvaluebuilding_materialhydroton { get; set; }
        public string frontendadtableinformationvaluebuilding_materialother { get; set; }
        public string frontendadtableinformationvaluebuilding_materialreinforced_concrete { get; set; }
        public string frontendadtableinformationvaluebuilding_materialsilikat { get; set; }
        public string frontendadtableinformationvaluebuilding_materialwood { get; set; }
        public string frontendadtableinformationvaluebuilding_ownershipco_operative_ownership { get; set; }
        public string frontendadtableinformationvaluebuilding_ownershipco_operative_ownership_with_a_land_and_mortgage_registe { get; set; }
        public string frontendadtableinformationvaluebuilding_ownershipfull_ownership { get; set; }
        public string frontendadtableinformationvaluebuilding_ownershipshare { get; set; }
        public string frontendadtableinformationvaluebuilding_type145block { get; set; }
        public string frontendadtableinformationvaluebuilding_type15block { get; set; }
        public string frontendadtableinformationvaluebuilding_type155block { get; set; }
        public string frontendadtableinformationvaluebuilding_typeapartment { get; set; }
        public string frontendadtableinformationvaluebuilding_typedetached { get; set; }
        public string frontendadtableinformationvaluebuilding_typefarm { get; set; }
        public string frontendadtableinformationvaluebuilding_typehistorical_building { get; set; }
        public string frontendadtableinformationvaluebuilding_typehouse { get; set; }
        public string frontendadtableinformationvaluebuilding_typeinfill { get; set; }
        public string frontendadtableinformationvaluebuilding_typeloft { get; set; }
        public string frontendadtableinformationvaluebuilding_typeoffice_building { get; set; }
        public string frontendadtableinformationvaluebuilding_typeprivate_house { get; set; }
        public string frontendadtableinformationvaluebuilding_typeresidence { get; set; }
        public string frontendadtableinformationvaluebuilding_typeribbon { get; set; }
        public string frontendadtableinformationvaluebuilding_typesemi_detached { get; set; }
        public string frontendadtableinformationvaluebuilding_typeseparate { get; set; }
        public string frontendadtableinformationvaluebuilding_typeshopping_centre { get; set; }
        public string frontendadtableinformationvaluebuilding_typetenement { get; set; }
        public string frontendadtableinformationvaluebuilding_typetenement_house { get; set; }
        public string frontendadtableinformationvaluecellar { get; set; }
        public string frontendadtableinformationvaluecooperativeownership { get; set; }
        public string frontendadtableinformationvaluecooperativeownershipwithalandandmortgageregister { get; set; }
        public string frontendadtableinformationvalueconditionforrenewal { get; set; }
        public string frontendadtableinformationvalueconditionnew { get; set; }
        public string frontendadtableinformationvalueconstruction_status107ready_to_use { get; set; }
        public string frontendadtableinformationvalueconstruction_status113ready_to_use { get; set; }
        public string frontendadtableinformationvalueconstruction_status121ready_to_use { get; set; }
        public string frontendadtableinformationvalueconstruction_status141ready_to_use { get; set; }
        public string frontendadtableinformationvalueconstruction_status67ready_to_use { get; set; }
        public string frontendadtableinformationvalueconstruction_statusto_completion { get; set; }
        public string frontendadtableinformationvalueconstruction_statusto_renovation { get; set; }
        public string frontendadtableinformationvalueconstruction_statusunfinished_close { get; set; }
        public string frontendadtableinformationvalueconstruction_statusunfinished_open { get; set; }
        public string frontendadtableinformationvaluedetachedgarage { get; set; }
        public string frontendadtableinformationvalueesignature { get; set; }
        public string frontendadtableinformationvalueeighth { get; set; }
        public string frontendadtableinformationvalueelectrical { get; set; }
        public string frontendadtableinformationvalueenclosedareaprivateparking { get; set; }
        public string frontendadtableinformationvalueequipment_typesbathtub { get; set; }
        public string frontendadtableinformationvalueequipment_typesdishwasher { get; set; }
        public string frontendadtableinformationvalueequipment_typesfridge { get; set; }
        public string frontendadtableinformationvalueequipment_typesfurniture { get; set; }
        public string frontendadtableinformationvalueequipment_typesoven { get; set; }
        public string frontendadtableinformationvalueequipment_typesstove { get; set; }
        public string frontendadtableinformationvalueequipment_typestv { get; set; }
        public string frontendadtableinformationvalueequipment_typeswashing_machine { get; set; }
        public string frontendadtableinformationvalueextras_types179garage { get; set; }
        public string frontendadtableinformationvalueextras_types181garage { get; set; }
        public string frontendadtableinformationvalueextras_types219garage { get; set; }
        public string frontendadtableinformationvalueextras_types85garage { get; set; }
        public string frontendadtableinformationvalueextras_types87garage { get; set; }
        public string frontendadtableinformationvalueextras_typesair_conditioning { get; set; }
        public string frontendadtableinformationvalueextras_typesasphalt_access { get; set; }
        public string frontendadtableinformationvalueextras_typesattic { get; set; }
        public string frontendadtableinformationvalueextras_typesbalcony { get; set; }
        public string frontendadtableinformationvalueextras_typesbasement { get; set; }
        public string frontendadtableinformationvalueextras_typescellar { get; set; }
        public string frontendadtableinformationvalueextras_typesclosed_area { get; set; }
        public string frontendadtableinformationvalueextras_typeselevator { get; set; }
        public string frontendadtableinformationvalueextras_typesfurniture { get; set; }
        public string frontendadtableinformationvalueextras_typesgarage { get; set; }
        public string frontendadtableinformationvalueextras_typesgarden { get; set; }
        public string frontendadtableinformationvalueextras_typesheating { get; set; }
        public string frontendadtableinformationvalueextras_typeslift { get; set; }
        public string frontendadtableinformationvalueextras_typesnon_smokers_only { get; set; }
        public string frontendadtableinformationvalueextras_typesparking { get; set; }
        public string frontendadtableinformationvalueextras_typespool { get; set; }
        public string frontendadtableinformationvalueextras_typessecurity { get; set; }
        public string frontendadtableinformationvalueextras_typesseparate_kitchen { get; set; }
        public string frontendadtableinformationvalueextras_typesshop_window { get; set; }
        public string frontendadtableinformationvalueextras_typesterrace { get; set; }
        public string frontendadtableinformationvalueextras_typesterraces { get; set; }
        public string frontendadtableinformationvalueextras_typestwo_storey { get; set; }
        public string frontendadtableinformationvalueextras_typesusable_room { get; set; }
        public string frontendadtableinformationvalueextra_spacesbalcony { get; set; }
        public string frontendadtableinformationvalueextra_spacesbasement { get; set; }
        public string frontendadtableinformationvalueextra_spacesbicycle_room { get; set; }
        public string frontendadtableinformationvalueextra_spacesground_parking_space { get; set; }
        public string frontendadtableinformationvalueextra_spacesstorage_room { get; set; }
        public string frontendadtableinformationvalueextra_spacesterrace { get; set; }
        public string frontendadtableinformationvalueextra_spacesunderground_parking_space { get; set; }
        public string frontendadtableinformationvaluefencen { get; set; }
        public string frontendadtableinformationvaluefencey { get; set; }
        public string frontendadtableinformationvaluefence_types177brick { get; set; }
        public string frontendadtableinformationvaluefence_typesbrick { get; set; }
        public string frontendadtableinformationvaluefence_typesconcrete { get; set; }
        public string frontendadtableinformationvaluefence_typeshedge { get; set; }
        public string frontendadtableinformationvaluefence_typesmetal { get; set; }
        public string frontendadtableinformationvaluefence_typesother { get; set; }
        public string frontendadtableinformationvaluefence_typeswire { get; set; }
        public string frontendadtableinformationvaluefence_typeswooden { get; set; }
        public string frontendadtableinformationvaluefifth { get; set; }
        public string frontendadtableinformationvaluefirst { get; set; }
        public string frontendadtableinformationvaluefloor1 { get; set; }
        public string frontendadtableinformationvaluefloor10 { get; set; }
        public string frontendadtableinformationvaluefloor2 { get; set; }
        public string frontendadtableinformationvaluefloor3 { get; set; }
        public string frontendadtableinformationvaluefloor4 { get; set; }
        public string frontendadtableinformationvaluefloor5 { get; set; }
        public string frontendadtableinformationvaluefloor6 { get; set; }
        public string frontendadtableinformationvaluefloor7 { get; set; }
        public string frontendadtableinformationvaluefloor8 { get; set; }
        public string frontendadtableinformationvaluefloor9 { get; set; }
        public string frontendadtableinformationvaluefloorhigher10 { get; set; }
        public string frontendadtableinformationvalueflooringnone { get; set; }
        public string frontendadtableinformationvalueflooringpollen { get; set; }
        public string frontendadtableinformationvalueflooringunpollen { get; set; }
        public string frontendadtableinformationvaluefloors_numground_floor { get; set; }
        public string frontendadtableinformationvaluefloors_nummore { get; set; }
        public string frontendadtableinformationvaluefloors_numone_floor { get; set; }
        public string frontendadtableinformationvaluefloors_numtwo_floors { get; set; }
        public string frontendadtableinformationvaluefloor_nocellar { get; set; }
        public string frontendadtableinformationvaluefloor_nofloor_1 { get; set; }
        public string frontendadtableinformationvaluefloor_nofloor_10 { get; set; }
        public string frontendadtableinformationvaluefloor_nofloor_2 { get; set; }
        public string frontendadtableinformationvaluefloor_nofloor_3 { get; set; }
        public string frontendadtableinformationvaluefloor_nofloor_4 { get; set; }
        public string frontendadtableinformationvaluefloor_nofloor_5 { get; set; }
        public string frontendadtableinformationvaluefloor_nofloor_6 { get; set; }
        public string frontendadtableinformationvaluefloor_nofloor_7 { get; set; }
        public string frontendadtableinformationvaluefloor_nofloor_8 { get; set; }
        public string frontendadtableinformationvaluefloor_nofloor_9 { get; set; }
        public string frontendadtableinformationvaluefloor_nofloor_higher_10 { get; set; }
        public string frontendadtableinformationvaluefloor_nogarret { get; set; }
        public string frontendadtableinformationvaluefloor_noground_floor { get; set; }
        public string frontendadtableinformationvalueforrenewal { get; set; }
        public string frontendadtableinformationvaluefourth { get; set; }
        public string frontendadtableinformationvaluefullownership { get; set; }
        public string frontendadtableinformationvaluegarage { get; set; }
        public string frontendadtableinformationvaluegarageinthebuilding { get; set; }
        public string frontendadtableinformationvaluegarden { get; set; }
        public string frontendadtableinformationvaluegarret { get; set; }
        public string frontendadtableinformationvaluegarret_typenotany { get; set; }
        public string frontendadtableinformationvaluegarret_typeunusable { get; set; }
        public string frontendadtableinformationvaluegarret_typeusable { get; set; }
        public string frontendadtableinformationvaluegas { get; set; }
        public string frontendadtableinformationvalueground { get; set; }
        public string frontendadtableinformationvaluegroundfloor { get; set; }
        public string frontendadtableinformationvalueheatingboiler_room { get; set; }
        public string frontendadtableinformationvalueheatingelectrical { get; set; }
        public string frontendadtableinformationvalueheatinggas { get; set; }
        public string frontendadtableinformationvalueheatingn { get; set; }
        public string frontendadtableinformationvalueheatingother { get; set; }
        public string frontendadtableinformationvalueheatingtiled_stove { get; set; }
        public string frontendadtableinformationvalueheatingurban { get; set; }
        public string frontendadtableinformationvalueheatingy { get; set; }
        public string frontendadtableinformationvalueheating_typesbiomass { get; set; }
        public string frontendadtableinformationvalueheating_typescoal { get; set; }
        public string frontendadtableinformationvalueheating_typeselectric { get; set; }
        public string frontendadtableinformationvalueheating_typesfireplace { get; set; }
        public string frontendadtableinformationvalueheating_typesgas { get; set; }
        public string frontendadtableinformationvalueheating_typesgeothermal { get; set; }
        public string frontendadtableinformationvalueheating_typesheat_pump { get; set; }
        public string frontendadtableinformationvalueheating_typesoil { get; set; }
        public string frontendadtableinformationvalueheating_typessolar_collector { get; set; }
        public string frontendadtableinformationvalueheating_typesstove { get; set; }
        public string frontendadtableinformationvalueheating_typesurban { get; set; }
        public string frontendadtableinformationvaluehide_price0 { get; set; }
        public string frontendadtableinformationvaluehide_price1 { get; set; }
        public string frontendadtableinformationvalueis_bungalow0 { get; set; }
        public string frontendadtableinformationvalueis_bungalow1 { get; set; }
        public string frontendadtableinformationvaluelightingn { get; set; }
        public string frontendadtableinformationvaluelightingy { get; set; }
        public string frontendadtableinformationvaluelocalizationby_the_house { get; set; }
        public string frontendadtableinformationvaluelocalizationin_building { get; set; }
        public string frontendadtableinformationvaluelocalizationseparate { get; set; }
        public string frontendadtableinformationvaluelocationcity { get; set; }
        public string frontendadtableinformationvaluelocationcountry { get; set; }
        public string frontendadtableinformationvaluelocationsuburban { get; set; }
        public string frontendadtableinformationvaluemarketprimary { get; set; }
        public string frontendadtableinformationvaluemarketsecondary { get; set; }
        public string frontendadtableinformationvaluemedia_typescabletelevision { get; set; }
        public string frontendadtableinformationvaluemedia_typescable_television { get; set; }
        public string frontendadtableinformationvaluemedia_typescesspool { get; set; }
        public string frontendadtableinformationvaluemedia_typeselectricity { get; set; }
        public string frontendadtableinformationvaluemedia_typesgas { get; set; }
        public string frontendadtableinformationvaluemedia_typesinternet { get; set; }
        public string frontendadtableinformationvaluemedia_typesphone { get; set; }
        public string frontendadtableinformationvaluemedia_typespower { get; set; }
        public string frontendadtableinformationvaluemedia_typesrafinery { get; set; }
        public string frontendadtableinformationvaluemedia_typessewage { get; set; }
        public string frontendadtableinformationvaluemedia_typestelephone { get; set; }
        public string frontendadtableinformationvaluemedia_typeswater { get; set; }
        public string frontendadtableinformationvaluemedia_typeswater_purification { get; set; }
        //public string frontendadtableinformationvaluen { get; set; }
        public string frontendadtableinformationvaluenew { get; set; }
        public string frontendadtableinformationvalueninth { get; set; }
        public string frontendadtableinformationvaluenon_smokers_only0 { get; set; }
        public string frontendadtableinformationvaluenon_smokers_only1 { get; set; }
        public string frontendadtableinformationvalueoffered_estates_typecommercial_properties { get; set; }
        public string frontendadtableinformationvalueoffered_estates_typeflats { get; set; }
        public string frontendadtableinformationvalueoffered_estates_typehouses { get; set; }
        public string frontendadtableinformationvalueoffered_estates_types_projectcommercial_properties { get; set; }
        public string frontendadtableinformationvalueoffered_estates_types_projectflats { get; set; }
        public string frontendadtableinformationvalueoffered_estates_types_projecthouses { get; set; }
        public string frontendadtableinformationvalueoffered_estates_type_projectflats { get; set; }
        public string frontendadtableinformationvalueoffice_space0 { get; set; }
        public string frontendadtableinformationvalueoffice_space1 { get; set; }
        public string frontendadtableinformationvalueoutdoorbalcony { get; set; }
        public string frontendadtableinformationvalueoutdoorgarden { get; set; }
        public string frontendadtableinformationvalueoutdoorterrace { get; set; }
        public string frontendadtableinformationvalueparking { get; set; }
        public string frontendadtableinformationvalueparkingasphalt { get; set; }
        public string frontendadtableinformationvalueparkingcobblestone { get; set; }
        public string frontendadtableinformationvalueparkingconcrete { get; set; }
        public string frontendadtableinformationvalueparkinghard_surfaced { get; set; }
        public string frontendadtableinformationvalueparkingnone { get; set; }
        public string frontendadtableinformationvalueparkingsoft_surfaced { get; set; }
        public string frontendadtableinformationvaluepreferred_professionemployee { get; set; }
        public string frontendadtableinformationvaluepreferred_professionnone { get; set; }
        public string frontendadtableinformationvaluepreferred_professionstudent { get; set; }
        public string frontendadtableinformationvaluepreferred_sexman { get; set; }
        public string frontendadtableinformationvaluepreferred_sexpair { get; set; }
        public string frontendadtableinformationvaluepreferred_sexwoman { get; set; }
        public string frontendadtableinformationvaluepriceondemand { get; set; }
        public string frontendadtableinformationvalueprivate { get; set; }
        public string frontendadtableinformationvalueproject_amenitiesdisabled_friendly { get; set; }
        public string frontendadtableinformationvalueproject_amenitieselevators { get; set; }
        public string frontendadtableinformationvalueproject_amenitiesgym { get; set; }
        public string frontendadtableinformationvalueproject_amenitiesplayground { get; set; }
        public string frontendadtableinformationvalueproject_amenitiesrelax_area { get; set; }
        public string frontendadtableinformationvalueproject_amenitiessport_fields { get; set; }
        public string frontendadtableinformationvalueproject_amenitiesswimming_pool { get; set; }
        public string frontendadtableinformationvalueramp0 { get; set; }
        public string frontendadtableinformationvalueramp1 { get; set; }
        public string frontendadtableinformationvaluerecreational0 { get; set; }
        public string frontendadtableinformationvaluerecreational1 { get; set; }
        public string frontendadtableinformationvalueremote_services0 { get; set; }
        public string frontendadtableinformationvalueremote_services1 { get; set; }
        public string frontendadtableinformationvaluerent_to_students0 { get; set; }
        public string frontendadtableinformationvaluerent_to_students1 { get; set; }
        public string frontendadtableinformationvalueroofingasbestic_tile { get; set; }
        public string frontendadtableinformationvalueroofingchopper { get; set; }
        public string frontendadtableinformationvalueroofingother { get; set; }
        public string frontendadtableinformationvalueroofingroofing_paper { get; set; }
        public string frontendadtableinformationvalueroofingsheet { get; set; }
        public string frontendadtableinformationvalueroofingshingle { get; set; }
        public string frontendadtableinformationvalueroofingslate { get; set; }
        public string frontendadtableinformationvalueroofingtile { get; set; }
        public string frontendadtableinformationvalueroof_typediagonal { get; set; }
        public string frontendadtableinformationvalueroof_typeflat { get; set; }
        public string frontendadtableinformationvalueroof_typenotany { get; set; }
        public string frontendadtableinformationvalueroomsizeone { get; set; }
        public string frontendadtableinformationvalueroomsizethree { get; set; }
        public string frontendadtableinformationvalueroomsizetwo { get; set; }
        public string frontendadtableinformationvaluerooms_num10ormore { get; set; }
        public string frontendadtableinformationvaluerooms_nummore { get; set; }
        public string frontendadtableinformationvaluesecond { get; set; }
        public string frontendadtableinformationvaluesecurityalarm_system { get; set; }
        public string frontendadtableinformationvaluesecurityclosed_area { get; set; }
        public string frontendadtableinformationvaluesecuritymonitoring { get; set; }
        public string frontendadtableinformationvaluesecuritysecurity_vigilance { get; set; }
        public string frontendadtableinformationvaluesecuritysmoke_detector { get; set; }
        public string frontendadtableinformationvaluesecurity_typesalarm { get; set; }
        public string frontendadtableinformationvaluesecurity_typesanti_burglary_door { get; set; }
        public string frontendadtableinformationvaluesecurity_typesclosed_area { get; set; }
        public string frontendadtableinformationvaluesecurity_typesentryphone { get; set; }
        public string frontendadtableinformationvaluesecurity_typesmonitoring { get; set; }
        public string frontendadtableinformationvaluesecurity_typesroller_shutters { get; set; }
        public string frontendadtableinformationvalueseventh { get; set; }
        public string frontendadtableinformationvalueshare { get; set; }
        public string frontendadtableinformationvaluesixth { get; set; }
        public string frontendadtableinformationvaluesocial_facilities0 { get; set; }
        public string frontendadtableinformationvaluesocial_facilities1 { get; set; }
        public string frontendadtableinformationvaluestatein_building { get; set; }
        public string frontendadtableinformationvaluestatenot_started { get; set; }
        public string frontendadtableinformationvaluestateready { get; set; }
        public string frontendadtableinformationvaluestreetparking { get; set; }
        public string frontendadtableinformationvaluestreetprivateparking { get; set; }
        public string frontendadtableinformationvaluestructure103brick { get; set; }
        public string frontendadtableinformationvaluestructure103wood { get; set; }
        public string frontendadtableinformationvaluestructure45brick { get; set; }
        public string frontendadtableinformationvaluestructure45wood { get; set; }
        public string frontendadtableinformationvaluestructureglass { get; set; }
        public string frontendadtableinformationvaluestructureshed { get; set; }
        public string frontendadtableinformationvaluestructuresteel { get; set; }
        public string frontendadtableinformationvaluestructuretent { get; set; }
        public string frontendadtableinformationvaluestructuretin { get; set; }
        public string frontendadtableinformationvaluetenth { get; set; }
        public string frontendadtableinformationvalueterrace { get; set; }
        public string frontendadtableinformationvaluethird { get; set; }
        public string frontendadtableinformationvaluetiledstove { get; set; }
        public string frontendadtableinformationvaluetypeagricultural { get; set; }
        public string frontendadtableinformationvaluetypeagricultural_building { get; set; }
        public string frontendadtableinformationvaluetypebuilding { get; set; }
        public string frontendadtableinformationvaluetypecommercial { get; set; }
        public string frontendadtableinformationvaluetypehabitat { get; set; }
        public string frontendadtableinformationvaluetypeother { get; set; }
        public string frontendadtableinformationvaluetyperecreational { get; set; }
        public string frontendadtableinformationvaluetypewoodland { get; set; }
        public string frontendadtableinformationvalueundergroundparking { get; set; }
        public string frontendadtableinformationvalueurban { get; set; }
        public string frontendadtableinformationvalueuse_types135office { get; set; }
        public string frontendadtableinformationvalueuse_types25office { get; set; }
        public string frontendadtableinformationvalueuse_typescommercial { get; set; }
        public string frontendadtableinformationvalueuse_typesgastronomy { get; set; }
        public string frontendadtableinformationvalueuse_typeshotel { get; set; }
        public string frontendadtableinformationvalueuse_typesindustrial { get; set; }
        public string frontendadtableinformationvalueuse_typesmanufacturing { get; set; }
        public string frontendadtableinformationvalueuse_typesoffice { get; set; }
        public string frontendadtableinformationvalueuse_typesretail { get; set; }
        public string frontendadtableinformationvalueuse_typesservices { get; set; }
        public string frontendadtableinformationvalueuse_typesstock { get; set; }
        public string frontendadtableinformationvaluevicinity_typesforest { get; set; }
        public string frontendadtableinformationvaluevicinity_typeslake { get; set; }
        public string frontendadtableinformationvaluevicinity_typesmountains { get; set; }
        public string frontendadtableinformationvaluevicinity_typesopen_terrain { get; set; }
        public string frontendadtableinformationvaluevicinity_typessea { get; set; }
        public string frontendadtableinformationvaluevideomeeting { get; set; }
        public string frontendadtableinformationvaluevideopresentation { get; set; }
        public string frontendadtableinformationvaluewindows_typealuminium { get; set; }
        public string frontendadtableinformationvaluewindows_typenotany { get; set; }
        public string frontendadtableinformationvaluewindows_typeplastic { get; set; }
        public string frontendadtableinformationvaluewindows_typewooden { get; set; }
        //public string frontendadtableinformationvaluey { get; set; }
        public string frontendadtableinformationvaluerowpanelblocks { get; set; }
        public string frontendadtableinformationrowlabeladditionalareas { get; set; }
        public string frontendadtableinformationrowlabeladvertisertype { get; set; }
        public string frontendadtableinformationrowlabelareas { get; set; }
        public string frontendadtableinformationrowlabelbuildyear { get; set; }
        public string frontendadtableinformationrowlabelbuildingmaterial { get; set; }
        public string frontendadtableinformationrowlabelbuildingtype { get; set; }
        public string frontendadtableinformationrowlabelconveniencestypes { get; set; }
        public string frontendadtableinformationrowlabelequipment { get; set; }
        public string frontendadtableinformationrowlabelequipmenttypes { get; set; }
        public string frontendadtableinformationrowlabelkitchen { get; set; }
        public string frontendadtableinformationrowlabelkitchentype { get; set; }
        public string frontendadtableinformationrowlabellift { get; set; }
        public string frontendadtableinformationrowlabelmarket { get; set; }
        public string frontendadtableinformationrowlabelpropertytype { get; set; }
        public string frontendadtableinformationrowlabelrooms { get; set; }
        public string frontendadtableinformationrowlabelsecuritytypes { get; set; }
        public string frontendadtableinformationrowlabelwindowdirection { get; set; }
        public string frontendadtableinformationrowlabelwindowsorientation { get; set; }
        public string frontendadtableinformationrowlabelwindowstype { get; set; }
        public string frontendadtableinformationrowvalue0 { get; set; }
        public string frontendadtableinformationrowvalue1 { get; set; }
        public string frontendadtableinformationrowvalueaccessfordisabled { get; set; }
        public string frontendadtableinformationrowvalueagency { get; set; }
        public string frontendadtableinformationrowvalueairconditioning { get; set; }
        public string frontendadtableinformationrowvaluealarm { get; set; }
        public string frontendadtableinformationrowvaluealuminium { get; set; }
        public string frontendadtableinformationrowvalueanteroom { get; set; }
        public string frontendadtableinformationrowvalueantiburglarydoor { get; set; }
        public string frontendadtableinformationrowvalueapartment { get; set; }
        public string frontendadtableinformationrowvalueapartmentblock { get; set; }
        public string frontendadtableinformationrowvalueapartmentbuilding { get; set; }
        public string frontendadtableinformationrowvaluearmoureddoor { get; set; }
        public string frontendadtableinformationrowvalueasphalt { get; set; }
        public string frontendadtableinformationrowvalueasphaltaccess { get; set; }
        public string frontendadtableinformationrowvalueattic { get; set; }
        public string frontendadtableinformationrowvaluebalcony { get; set; }
        public string frontendadtableinformationrowvaluebalconyenclosed { get; set; }
        public string frontendadtableinformationrowvaluebalconyopen { get; set; }
        public string frontendadtableinformationrowvaluebasement { get; set; }
        public string frontendadtableinformationrowvaluebathroom { get; set; }
        public string frontendadtableinformationrowvaluebathtub { get; set; }
        public string frontendadtableinformationrowvaluebathtubwithshower { get; set; }
        public string frontendadtableinformationrowvaluebedroom { get; set; }
        public string frontendadtableinformationrowvaluebicycleroom { get; set; }
        public string frontendadtableinformationrowvalueblock { get; set; }
        public string frontendadtableinformationrowvaluebreezeblock { get; set; }
        public string frontendadtableinformationrowvaluebrick { get; set; }
        public string frontendadtableinformationrowvaluecellar { get; set; }
        public string frontendadtableinformationrowvaluecellularconcrete { get; set; }
        public string frontendadtableinformationrowvaluecelluralconcrete { get; set; }
        public string frontendadtableinformationrowvalueclosedarea { get; set; }
        public string frontendadtableinformationrowvalueconcrete { get; set; }
        public string frontendadtableinformationrowvalueconcreteplate { get; set; }
        public string frontendadtableinformationrowvaluedetachedhouse { get; set; }
        public string frontendadtableinformationrowvaluedeveloper { get; set; }
        public string frontendadtableinformationrowvaluedirt { get; set; }
        public string frontendadtableinformationrowvaluedishwasher { get; set; }
        public string frontendadtableinformationrowvalueduplex { get; set; }
        public string frontendadtableinformationrowvalueeast { get; set; }
        public string frontendadtableinformationrowvalueelevator { get; set; }
        public string frontendadtableinformationrowvalueenclosedbalcony { get; set; }
        public string frontendadtableinformationrowvalueentryphone { get; set; }
        public string frontendadtableinformationrowvaluefence { get; set; }
        public string frontendadtableinformationrowvalueflat { get; set; }
        public string frontendadtableinformationrowvaluefreestandingfurniture { get; set; }
        public string frontendadtableinformationrowvaluefridge { get; set; }
        public string frontendadtableinformationrowvaluefurniture { get; set; }
        public string frontendadtableinformationrowvaluegarage { get; set; }
        public string frontendadtableinformationrowvaluegarden { get; set; }
        public string frontendadtableinformationrowvaluegasdetector { get; set; }
        public string frontendadtableinformationrowvaluegatedcommunity { get; set; }
        public string frontendadtableinformationrowvaluegroundparkingspace { get; set; }
        public string frontendadtableinformationrowvaluehall { get; set; }
        public string frontendadtableinformationrowvaluehallway { get; set; }
        public string frontendadtableinformationrowvaluehardsurfaced { get; set; }
        public string frontendadtableinformationrowvalueheating { get; set; }
        public string frontendadtableinformationrowvaluehollowbrick { get; set; }
        public string frontendadtableinformationrowvaluehouse { get; set; }
        public string frontendadtableinformationrowvaluehydroton { get; set; }
        public string frontendadtableinformationrowvalueindividual { get; set; }
        public string frontendadtableinformationrowvalueinduction { get; set; }
        public string frontendadtableinformationrowvalueinductionhob { get; set; }
        public string frontendadtableinformationrowvalueinfill { get; set; }
        public string frontendadtableinformationrowvalueintercomvideophone { get; set; }
        public string frontendadtableinformationrowvalueinternet { get; set; }
        public string frontendadtableinformationrowvaluekeramzite { get; set; }
        public string frontendadtableinformationrowvaluekitchen { get; set; }
        public string frontendadtableinformationrowvaluelift { get; set; }
        public string frontendadtableinformationrowvaluelivingroom { get; set; }
        public string frontendadtableinformationrowvaluelivingroomwithopenkitchen { get; set; }
        public string frontendadtableinformationrowvalueloggia { get; set; }
        public string frontendadtableinformationrowvaluemonitoring { get; set; }
        public string frontendadtableinformationrowvaluemonitoringsecurity { get; set; }
        public string frontendadtableinformationrowvaluen { get; set; }
        public string frontendadtableinformationrowvaluenaturalgaspipeline { get; set; }
        public string frontendadtableinformationrowvalueno { get; set; }
        public string frontendadtableinformationrowvaluenonsmokersonly { get; set; }
        public string frontendadtableinformationrowvaluenorth { get; set; }
        public string frontendadtableinformationrowvalueopen { get; set; }
        public string frontendadtableinformationrowvalueopenbalcony { get; set; }
        public string frontendadtableinformationrowvalueopticalfiber { get; set; }
        public string frontendadtableinformationrowvalueother { get; set; }
        public string frontendadtableinformationrowvalueoven { get; set; }
        public string frontendadtableinformationrowvalueparking { get; set; }
        public string frontendadtableinformationrowvalueparkingspace { get; set; }
        public string frontendadtableinformationrowvaluepenthouse { get; set; }
        public string frontendadtableinformationrowvalueplastic { get; set; }
        public string frontendadtableinformationrowvaluepool { get; set; }
        public string frontendadtableinformationrowvalueprimary { get; set; }
        public string frontendadtableinformationrowvalueprivate { get; set; }
        public string frontendadtableinformationrowvaluereinforcedconcrete { get; set; }
        public string frontendadtableinformationrowvaluerestroom { get; set; }
        public string frontendadtableinformationrowvalueribbon { get; set; }
        public string frontendadtableinformationrowvaluerollershutters { get; set; }
        public string frontendadtableinformationrowvalueroom { get; set; }
        public string frontendadtableinformationrowvaluesecondary { get; set; }
        public string frontendadtableinformationrowvaluesecurity { get; set; }
        public string frontendadtableinformationrowvaluesecuritydoorswinows { get; set; }
        public string frontendadtableinformationrowvalueseparate { get; set; }
        public string frontendadtableinformationrowvalueseparatekitchen { get; set; }
        public string frontendadtableinformationrowvalueshopwindow { get; set; }
        public string frontendadtableinformationrowvalueshower { get; set; }
        public string frontendadtableinformationrowvaluesilicateblocks { get; set; }
        public string frontendadtableinformationrowvaluesilikat { get; set; }
        public string frontendadtableinformationrowvaluesoftsurfaced { get; set; }
        public string frontendadtableinformationrowvaluesouth { get; set; }
        public string frontendadtableinformationrowvaluestorageroom { get; set; }
        public string frontendadtableinformationrowvaluestove { get; set; }
        public string frontendadtableinformationrowvaluestudio { get; set; }
        public string frontendadtableinformationrowvaluetenement { get; set; }
        public string frontendadtableinformationrowvalueterrace { get; set; }
        public string frontendadtableinformationrowvalueterraced { get; set; }
        public string frontendadtableinformationrowvalueterraces { get; set; }
        public string frontendadtableinformationrowvaluethermopaneglass { get; set; }
        public string frontendadtableinformationrowvaluetriplex { get; set; }
        public string frontendadtableinformationrowvaluetv { get; set; }
        public string frontendadtableinformationrowvaluetwostorey { get; set; }
        public string frontendadtableinformationrowvalueundergroundparkingspace { get; set; }
        public string frontendadtableinformationrowvalueusableroom { get; set; }
        public string frontendadtableinformationrowvaluevideointercom { get; set; }
        public string frontendadtableinformationrowvaluevideosurveillance { get; set; }
        public string frontendadtableinformationrowvaluewashingmachine { get; set; }
        public string frontendadtableinformationrowvaluewc { get; set; }
        public string frontendadtableinformationrowvaluewest { get; set; }
        public string frontendadtableinformationrowvaluewithmezzanine { get; set; }
        public string frontendadtableinformationrowvaluewithwindow { get; set; }
        public string frontendadtableinformationrowvaluewood { get; set; }
        public string frontendadtableinformationrowvaluewooden { get; set; }
        public string frontendadtableinformationrowvaluey { get; set; }
        public string frontendadtableinformationrowvalueyes { get; set; }
        public string frontendadfeaturesapartment { get; set; }
        public string frontendadfeaturesbuilding { get; set; }
        public string frontendadadmortgagesimulatorcalculator { get; set; }
        public string frontendadadmortgagesimulatormortgagebrowser { get; set; }
        public string frontendfinancecallrequestcontactformtitleremt507 { get; set; }
        public string frontendadadmortgagesimulatorpricepermonth { get; set; }
        public string frontendadmortgagesectiontitle { get; set; }
        public string frontendadadmortgagesimulatorinstallment { get; set; }
        public string frontendadadmortgagesimulatorcontribution { get; set; }
        public string frontendadadmortgagesimulatorperiod { get; set; }
        public string frontendadadmortgagesimulatoryears { get; set; }
        public string frontendadfinancialbannercontactbutton { get; set; }
        public string frontendadfinancialbannerheading { get; set; }
        public string frontendadfinancialbannersubtitle { get; set; }
        public string frontendadadsimilarofferregistrationmodaltitle { get; set; }
        public string frontendsearchregistrationmodalloginandsavesearch { get; set; }
        public string frontendsearchregistrationmodalregisterandsavesearch { get; set; }
        public string frontendsearchregistrationmodalregisterdescription { get; set; }
        public string frontendglobalgoback { get; set; }
        public string frontendexpandablecontentshowless { get; set; }
        public string frontendexpandablecontentshowmore { get; set; }
        public string frontendadfeaturesothers { get; set; }
        public string frontendadheaderprice { get; set; }
        public string frontendadheaderpricefrom { get; set; }
        public string frontendadheaderaskforprice { get; set; }
        public string frontendadheaderaddress { get; set; }
        public string frontendadheaderadditionalcosts { get; set; }
        public string frontendadheaderadditionalcostsheader { get; set; }
        public string frontendadheaderadditionalcostsclosebutton { get; set; }
        public string frontendglobalfreeparkingspace { get; set; }
        public string frontendglobalattractivepaymentplan { get; set; }
        public string frontendgloballastminuteoffer { get; set; }
        public string frontendglobalpresaleoffer { get; set; }
        public string frontendadheaderspecialoffertill { get; set; }
        public string frontendglobalspecialoffer { get; set; }
        public string frontendtooltiptriggerlabel { get; set; }
        public string frontendmodallabelclose { get; set; }
        public string frontendadinvestmentsearchcurrency { get; set; }
        public string frontendadinvestmentsearchpriceMinplaceholder { get; set; }
        public string frontendadinvestmentsearchpriceMaxplaceholder { get; set; }
        public string frontendsearchformtitle { get; set; }
        public string frontendadinvestmentsearchpricelabel { get; set; }
        public string frontendadinvestmentsearchareaMinplaceholder { get; set; }
        public string frontendadinvestmentsearchareaMaxplaceholder { get; set; }
        public string frontendadinvestmentsearchnumberofroomsplaceholder { get; set; }
        public string frontendadinvestmentsearchsubmitplaceholder { get; set; }
        public string frontendadinvestmentsearchclearcriteria { get; set; }
        public string frontendsearchnoresultsotherads { get; set; }
        public string frontendadinvestmentunitsnoresultsdescription { get; set; }
        public string frontendsearchareafieldminfieldplaceholder { get; set; }
        public string frontendsearchareafieldmaxfieldplaceholder { get; set; }
        public string frontendsearchareafieldmobilelabel { get; set; }
        public string frontendsearchformareaunit { get; set; }
        public string frontendsearchareafieldminfieldlabel { get; set; }
        public string frontendsearchareafieldmaxfieldlabel { get; set; }
        public string frontendsearchrangefieldminfielddefaultlabel { get; set; }
        public string frontendsearchrangefieldmaxfielddefaultlabel { get; set; }
        public string frontendsearchroomsfieldlabel { get; set; }
        public string frontendsearchroomsfieldmore { get; set; }
        public string frontendsearchroomsfieldmobilelabel { get; set; }
        public string frontendadinvestmentunitspagesize { get; set; }
        public string frontendadinvestmentunitssortingsortby { get; set; }
        public string frontendadprojectunitslistnumberofads { get; set; }
        public string frontendsearchnoresultsinformation { get; set; }
        public string frontendsearchnoresultstitle { get; set; }
        public string frontendpaginationcontrolsnavigationlabel { get; set; }
        public string frontendpaginationcontrolspreviouspage { get; set; }
        public string frontendpaginationcontrolsgotopage { get; set; }
        public string frontendpaginationcontrolsnextpage { get; set; }
        public string frontendadinvestmentunitssortingdefault { get; set; }
        public string frontendadinvestmentunitssortingprice { get; set; }
        public string frontendadinvestmentunitssortingarea { get; set; }
        public string frontendadinvestmentunitspriceondemand { get; set; }
        public string frontendadinvestmentunitsseethisunit { get; set; }
        public string frontendadinvestmentunitssortingrooms { get; set; }
        public string frontendadinvestmentunitssortingfloor { get; set; }
        public string frontendmobileadinvestmentunitssortingrooms { get; set; }
        public string frontendmobileadinvestmentunitssortingfloor { get; set; }
        public string frontendmobileadinvestmentunitssortingarea { get; set; }
        public string frontendmobileadinvestmentunitssortingprice { get; set; }
        public string frontendadprojectunitslistsortingsortby { get; set; }
        public string frontendadprojectunitslistsortingpriceascshort { get; set; }
        public string frontendadprojectunitslistlistitemroomsone { get; set; }
        public string frontendadprojectunitslistlistitemroomsfew { get; set; }
        public string frontendadprojectunitslistlistitemroomsmany { get; set; }
        public string frontendadprojectunitslistlistitembalcony { get; set; }
        public string frontendadprojectunitslistlistitemparking { get; set; }
        public string frontendadprojectunitslistlistitemfloor { get; set; }
        public string frontendsubscribeaddtofavorites { get; set; }
        public string frontendadprojectunitslistlistitemaskforprice { get; set; }
        public string frontendadobserveaddescriptionparagraphs { get; set; }
        public string frontendadobserveadregistrationmodalloginandsave { get; set; }
        public string frontendadobserveadregistrationmodalregisterandsave { get; set; }
        public string frontendadobserveadregistrationmodalregisterdescription { get; set; }
        public string frontendsharedloginmodalfootersocialsheader { get; set; }
        public string frontendsharedloginmodalfooterloginorregisterheader { get; set; }
        public string frontendloginapple { get; set; }
        public string frontendloginfacebooksecondary { get; set; }
        public string frontendlogingoogle { get; set; }
        public string frontendsharedloginmodalfooteremailloginbutton { get; set; }
        public string frontendsearchregistrationmodalsuccesstitleactivate { get; set; }
        public string frontendsearchregistrationmodalsuccessemaildescription { get; set; }
        public string frontendsearchregistrationmodalsuccessbutton { get; set; }
        public string frontendsearchregistrationmodalemailinput { get; set; }
        public string frontendsearchregistrationmodalemailinputplaceholder { get; set; }
        public string frontendsearchregistrationmodalpasswordinput { get; set; }
        public string frontendsearchregistrationmodalpasswordinputloginplaceholder { get; set; }
        public string frontendsearchregistrationmodalnotrememberpassword { get; set; }
        public string frontendsearchregistrationmodaltermsexcerpt { get; set; }
        public string frontendsharedshortregistrationmodalregulationpolicy { get; set; }
        public string frontendsharedshortregistrationmodalprivacypolicy { get; set; }
        public string frontendsharedshortregistrationmodalcookiespolicy { get; set; }
        public string frontendsharedshortregistrationmodalpolicy { get; set; }
        public string frontendsharedalertclosebtn { get; set; }
        public string frontendloginresponsewrongusernameorpassword { get; set; }
        public string frontendloginresponsecannotlogin { get; set; }
        public string frontendloginresponseinternalerror { get; set; }
        public string frontendloginresponsefbnoemail { get; set; }
        public string frontendlogintitle { get; set; }
        public string frontendloginsafetyalertcontent { get; set; }
        public string frontendloginformemail { get; set; }
        public string frontendloginformpassword { get; set; }
        public string frontendloginforgotpassword { get; set; }
        public string frontendloginformsubmit { get; set; }
        public string frontendloginformloggingin { get; set; }
        public string frontendloginnoaccountheader { get; set; }
        public string frontendloginregister { get; set; }
        public string frontendnoscriptyourbrowserisnotsupported { get; set; }
        public string frontendfooterbrandlogoalternativetext { get; set; }
        public string frontendfooterlinksservicestitle { get; set; }
        public string frontendfooterlinkssitemaptitle { get; set; }
        public string frontendfooterlinksappstitle { get; set; }
        public string frontendfooterlinksappsappstorearia { get; set; }
        public string frontendfooterlinksappsgoogleplayaria { get; set; }
        public string frontendfootersocial { get; set; }
        public string frontendfootersociallinksfacebooktitle { get; set; }
        public string frontendfootersociallinksyoutubetitle { get; set; }
        public string frontendfootersociallinksinstagramtitle { get; set; }
        public string frontendfootersociallinkstiktoktitle { get; set; }
        public string frontendfooterprize { get; set; }
        public string frontendfooterconsent { get; set; }
        public string frontendfooterconsenttermsofuse { get; set; }
        public string frontendfooterlinksgeneralcookies { get; set; }
        public string frontendsharedpageheaderuasupportlink { get; set; }
        public string frontendsharedadditembuttondefaultlabel { get; set; }
        public string frontendsharedadditembuttoninvestmentlabel { get; set; }
        public string frontendnavusermenuloadingerror { get; set; }
        public string frontendnavusermenumyaccount { get; set; }
        public string frontendnavusermenuusersfavouriteadverts { get; set; }
        public string frontendnavusermenufavourites { get; set; }
        public string frontendsharednavusermenulabel { get; set; }
        public string frontendnavusermenuusername { get; set; }
        public string frontendnavusermenulogout { get; set; }
        public string frontendlanguageswitcherselectedlanguage { get; set; }
        public string frontendnavmenusavedads { get; set; }
        public string frontendnavusermenuusersfavouritelabel { get; set; }
        public string frontendsharedmenuitemlazysectionloadingstate { get; set; }
        public string frontendsharedmenuitemlazysectionerrorstate { get; set; }
        public string frontendnavbarmenuprimarymarkettoplocations { get; set; }
        public string frontendadprojectunitslistsortingpriceasc { get; set; }
        public string frontendadprojectunitslistsortingpricedesc { get; set; }
        public string frontendadprojectunitslistsortingareaasc { get; set; }
        public string frontendadprojectunitslistsortingareadesc { get; set; }
        public string frontendadprojectunitslistsortingfloornumberasc { get; set; }
        public string frontendadprojectunitslistsortingfloornumberdesc { get; set; }
        public string frontendgooglemapnoexactlocation { get; set; }
        public string frontendadmediagallerypreviousphoto { get; set; }
        public string frontendadmediagallerynextphoto { get; set; }
        public string frontendadcontactformmessage { get; set; }
        public string frontendadcontactformcall { get; set; }
        public string frontendadgalleryvirtualwalkaroundoverlayclosebuttonlabel { get; set; }
        public string frontendadgalleryvirtualwalkaroundoverlaylabel { get; set; }
        public string frontendadgalleryvirtualwalkaroundoverlayseemoviebutton { get; set; }
        public string frontendadgalleryvirtualwalkaroundoverlaybutton { get; set; }
        public string frontendadgalleryexternalcontentiframe { get; set; }
        public string frontendadcontactformnamerequirederror { get; set; }
        public string frontendadcontactformemailrequirederror { get; set; }
        public string frontendadcontactformtextrequirederror { get; set; }
        public string frontendadcontactformemailerror { get; set; }
        public string frontendadcontactformmessagesubjectrequirederror { get; set; }
        public string frontendadcontactformaskaboutprice { get; set; }
        public string frontendadcontactformsubmiterror { get; set; }
        public string frontendadcontactformsubmitsuccess { get; set; }
        public string frontendadcontactformgdprprivacypolicynew { get; set; }
        public string frontendadcontactformfinanceprivacypolicy { get; set; }
        public string frontendadcontactformformlabel { get; set; }
        public string frontendadcontactformfieldplaceholdername { get; set; }
        public string frontendadcontactformfieldplaceholderemail { get; set; }
        public string frontendadcontactformfieldplaceholderphone { get; set; }
        public string frontendadcontactformfieldplaceholdermessagesubject { get; set; }
        public string frontendadcontactformfieldtext { get; set; }
        public string frontendadcontactformfieldplaceholdertext { get; set; }
        public string frontendadcontactformgdpradministrator { get; set; }
        public string frontendadcontactformtitlefinanceleadremt615 { get; set; }
        public string frontendadcontactformallowsendfinancelead { get; set; }
        public string frontendadcontactformallowsendfinanceleadremt615 { get; set; }
        public string frontendadcontactformsendmessage { get; set; }
        public string frontendphonenumberrevealphonenumberbutton { get; set; }
        public string frontendatomsadsubscribebuttonsavedadvert { get; set; }
        public string frontendatomsadsubscribebuttonsaveadvert { get; set; }
        public string frontendadcontactformmissinginformationsaleflatfirstpart { get; set; }
        public string frontendadcontactformmissinginformationrentflatfirstpart { get; set; }
        public string frontendadcontactformmissinginformationsalehousefirstpart { get; set; }
        public string frontendadcontactformmissinginformationrenthousefirstpart { get; set; }
        public string frontendadcontactformmissinginformationrentroomfirstpart { get; set; }
        public string frontendadcontactformmissinginformationsaleplotfirstpart { get; set; }
        public string frontendadcontactformmissinginformationrentplotfirstpart { get; set; }
        public string frontendadcontactformmissinginformationsaleshopfirstpart { get; set; }
        public string frontendadcontactformmissinginformationrentshopfirstpart { get; set; }
        public string frontendadcontactformmissinginformationsalewarehousefirstpart { get; set; }
        public string frontendadcontactformmissinginformationrentwarehousefirstpart { get; set; }
        public string frontendadcontactformmissinginformationsalegaragefirstpart { get; set; }
        public string frontendadcontactformmissinginformationrentgaragefirstpart { get; set; }
        public string frontendadcontactformmissinginformationfirstpart { get; set; }
        public string frontendadcontactformmissinginformationsecondpart { get; set; }
        public string frontendsharedphoneinputerrorrequired { get; set; }
        public string frontendsharedphoneinputerrorcountrycode { get; set; }
        public string frontendsharedphoneinputerrorformat { get; set; }
        public string frontendsharedphoneinputcountrycodelabel { get; set; }
        public string frontendadcontactformdefaultmessagedevelopment { get; set; }
        public string frontendadcontactformdefaultmessagesaleflat { get; set; }
        public string frontendadcontactformdefaultmessagerentflat { get; set; }
        public string frontendadcontactformdefaultmessagesalehouse { get; set; }
        public string frontendadcontactformdefaultmessagerenthouse { get; set; }
        public string frontendadcontactformdefaultmessagerentroom { get; set; }
        public string frontendadcontactformdefaultmessagesaleplot { get; set; }
        public string frontendadcontactformdefaultmessagerentplot { get; set; }
        public string frontendadcontactformdefaultmessagesaleshop { get; set; }
        public string frontendadcontactformdefaultmessagerentshop { get; set; }
        public string frontendadcontactformdefaultmessagesalewarehouse { get; set; }
        public string frontendadcontactformdefaultmessagerentwarehouse { get; set; }
        public string frontendadcontactformdefaultmessagesalegarage { get; set; }
        public string frontendadcontactformdefaultmessagerentgarage { get; set; }
        public string frontendadcontactformdefaultmessageopendays { get; set; }
        public string frontendadcontactformdefaultmessage { get; set; }
        public string frontendadcontactformmessagesubjectaskaboutprice { get; set; }
        public string frontendadcontactformmessagesubjectmoreinfo { get; set; }
        public string frontendadcontactformmessagesubjectother { get; set; }
        public string frontendadcontactformmessagesubjectschedulevisit { get; set; }
        public string frontendadcontactformmessagesubjectsuggestprice { get; set; }
        public string frontendadcontactformmoreinfomessagesaleflat { get; set; }
        public string frontendadcontactformmoreinfomessagerentflat { get; set; }
        public string frontendadcontactformmoreinfomessagesalehouse { get; set; }
        public string frontendadcontactformmoreinfomessagerenthouse { get; set; }
        public string frontendadcontactformmoreinfomessagerentroom { get; set; }
        public string frontendadcontactformmoreinfomessagesaleplot { get; set; }
        public string frontendadcontactformmoreinfomessagerentplot { get; set; }
        public string frontendadcontactformmoreinfomessagesaleshop { get; set; }
        public string frontendadcontactformmoreinfomessagerentshop { get; set; }
        public string frontendadcontactformmoreinfomessagesalewarehouse { get; set; }
        public string frontendadcontactformmoreinfomessagerentwarehouse { get; set; }
        public string frontendadcontactformmoreinfomessagesalegarage { get; set; }
        public string frontendadcontactformmoreinfomessagerentgarage { get; set; }
        public string frontendadcontactformschedulevisitmessagesaleflat { get; set; }
        public string frontendadcontactformschedulevisitmessagerentflat { get; set; }
        public string frontendadcontactformschedulevisitmessagesalehouse { get; set; }
        public string frontendadcontactformschedulevisitmessagerenthouse { get; set; }
        public string frontendadcontactformschedulevisitmessagerentroom { get; set; }
        public string frontendadcontactformschedulevisitmessagesaleplot { get; set; }
        public string frontendadcontactformschedulevisitmessagerentplot { get; set; }
        public string frontendadcontactformschedulevisitmessagesaleshop { get; set; }
        public string frontendadcontactformschedulevisitmessagerentshop { get; set; }
        public string frontendadcontactformschedulevisitmessagesalewarehouse { get; set; }
        public string frontendadcontactformschedulevisitmessagerentwarehouse { get; set; }
        public string frontendadcontactformschedulevisitmessagesalegarage { get; set; }
        public string frontendadcontactformschedulevisitmessagerentgarage { get; set; }
        public string frontendadcontactformsuggestpricemessagesaleflat { get; set; }
        public string frontendadcontactformsuggestpricemessagerentflat { get; set; }
        public string frontendadcontactformsuggestpricemessagesalehouse { get; set; }
        public string frontendadcontactformsuggestpricemessagerenthouse { get; set; }
        public string frontendadcontactformsuggestpricemessagerentroom { get; set; }
        public string frontendadcontactformsuggestpricemessagesaleplot { get; set; }
        public string frontendadcontactformsuggestpricemessagerentplot { get; set; }
        public string frontendadcontactformsuggestpricemessagesaleshop { get; set; }
        public string frontendadcontactformsuggestpricemessagerentshop { get; set; }
        public string frontendadcontactformsuggestpricemessagesalewarehouse { get; set; }
        public string frontendadcontactformsuggestpricemessagerentwarehouse { get; set; }
        public string frontendadcontactformsuggestpricemessagesalegarage { get; set; }
        public string frontendadcontactformsuggestpricemessagerentgarage { get; set; }
        public string frontendadcontactformnotificationondate { get; set; }
        public string frontendadcontactformnotificationtoday { get; set; }
        public string frontendadcontactformnotificationyesterday { get; set; }
        public string frontendadcontactformnotificationndaysago { get; set; }
        public string frontendglobalcontactformgdprless { get; set; }
        public string frontendglobalcontactformgdprmore { get; set; }
        public string frontendadimagegallerythumbnail { get; set; }
        public string frontendadimagegalleryfullimage { get; set; }
        public string frontendadfinancelinktextversionb { get; set; }
        public string frontendadfinancelinkspanversionb { get; set; }
        public string frontendadadmortgagesimulatormonth { get; set; }
        public string frontendadadmortgagesimulatortitle { get; set; }
        public string frontendadadmortgagesimulatorbuttonseesimulation { get; set; }
        public string frontendoverviewcharacteristicsprojectinvestmentstatus { get; set; }
        public string frontendoverviewcharacteristicsprojectinvestmentunitscount { get; set; }
        public string frontendoverviewcharacteristicsprojectads { get; set; }
        public string frontendoverviewcharacteristicsprojectfloors { get; set; }
        public string frontendoverviewcharacteristicsprojectpropertytype { get; set; }
        public string frontendadsimilarofferssubscriptiontoastinfo { get; set; }
        public string frontendadsubscribeadsectionsimilaroffersbuttoncontactform { get; set; }
        public string frontendadsubscribeadsectionsimilaroffersbutton2 { get; set; }
        public string frontendaduseradssingleadlabel { get; set; }
        public string frontendaduseradspriceondemand { get; set; }
        public string frontendadcontactformsimilaradssubscribesuggestiontext { get; set; }
        public string frontendsearchsimilaradsprompttitle { get; set; }
        public string frontendsearchsimilaradspromptparagraph { get; set; }
        public string frontendsearchsimilaradspromptbutton { get; set; }
        public string frontendadsimilaradsprompttitle3 { get; set; }
        public string frontendadsimilaradspromptparagraph2 { get; set; }
        public string frontendadsimilaradspromptbutton2 { get; set; }
        public string frontendatomsadsubscribebuttonsaved { get; set; }
        public string frontendatomsadsubscribebuttonsave { get; set; }
        public string frontendadagencybannerlinkadsagency { get; set; }
        public string frontendadavmmodulefeedbacktitle { get; set; }
        public string frontendadavmmodulefeedbackyes { get; set; }
        public string frontendadavmmodulefeedbackno { get; set; }
        public string frontendadavmmodulefeedbackthanks { get; set; }
        public string frontendadavmmoduletooltiptrigger { get; set; }
        public string frontendadavmmoduletooltiptext { get; set; }
        public string frontendadavmmodulemoreb { get; set; }
        public string frontendadavmreadmoremodaltitle { get; set; }
        public string frontendadavmreadmoremodaldescriptiontitle { get; set; }
        public string frontendadavmreadmoremodaldescriptioncontent { get; set; }
        public string frontendadavmreadmoremodaldescriptioncontentbold { get; set; }
        public string frontendadavmreadmoremodalhowtousetitle { get; set; }
        public string frontendadavmreadmoremodalhowtousecontentcheck { get; set; }
        public string frontendadavmreadmoremodalhowtousecontentchecklimits { get; set; }
        public string frontendadavmreadmoremodalhowtousecontentchecklimitsdescription { get; set; }
        public string frontendadavmreadmoremodalhowtousecontentcheckrangelittle { get; set; }
        public string frontendadavmreadmoremodalhowtousecontentcheckrangelittledescription { get; set; }
        public string frontendadavmreadmoremodalhowtousecontentcheckrangemore { get; set; }
        public string frontendadavmreadmoremodalhowtousecontentcheckrangemoredescription { get; set; }
        public string frontendadavmreadmoremodalhowtousecontentcontactwithseller { get; set; }
        public string frontendadavmreadmoremodalpricedifferencetitle { get; set; }
        public string frontendadavmreadmoremodalpricedifferencecontent1 { get; set; }
        public string frontendadavmreadmoremodalpricedifferencecontent2 { get; set; }
        public string frontendadavmreadmoremodalpricedifferencecontent3 { get; set; }
        public string frontendadavmreadmoremodalmissingfeaturetitle { get; set; }
        public string frontendadavmreadmoremodalmissingfeaturecontent { get; set; }
        public string frontendadreportbutton { get; set; }
        public string frontendgloballogo { get; set; }
        public string frontendadagencybannertype { get; set; }
        public string frontendadagencybannerlicensenumber { get; set; }
        public string frontendadagencybannername { get; set; }
        public string frontendadagencybannerphonenumber { get; set; }
        public string frontendadagencybanneraddress { get; set; }
        public string frontendadagencybannertitledeveloper { get; set; }
        public string frontendadagencybannerlinkadsdeveloper { get; set; }
        public string frontendadagencybannertitleagency { get; set; }
        public string frontendadagencybannertitleconsultant { get; set; }
        public string frontendadagencybannerlinkadsconsultant { get; set; }
        public string frontendadagencybannertitlemanager { get; set; }
        public string frontendadagencybannerlinkadsmanager { get; set; }
        public string frontendadagencybannertitlepromoter { get; set; }
        public string frontendadagencybannerlinkadspromoter { get; set; }
        public string frontendadreportmodalreasonoutofdate { get; set; }
        public string frontendadreportmodalreasonwronglocation { get; set; }
        public string frontendadreportmodalreasonfakedetails { get; set; }
        public string frontendadreportmodalreasonfakead { get; set; }
        public string frontendadreportmodalreasonnoownerconsent { get; set; }
        public string frontendadreportmodalreasonwrongpropertycondition { get; set; }
        public string frontendadreportmodalreasonwrongcontactdata { get; set; }
        public string frontendadreportmodalreasonother { get; set; }
        public string frontendadreportmodaltitle { get; set; }
        public string frontendadreportmodalfieldrequired { get; set; }
        public string frontendadreportmodaldescriptionlabel { get; set; }
        public string frontendadreportmodalsendingerror { get; set; }
        public string frontendadreportmodalsendingreport { get; set; }
        public string frontendadreportmodalsendbutton { get; set; }
        public string frontendadreportmodalsendsuccessfully { get; set; }
        public string frontendadtopinformationdatequarter { get; set; }
        public string frontendadoverviewtablenoinformation { get; set; }
        public string frontendadoverviewtableaskformissinginformation { get; set; }
        public string frontendadoverviewtableremoteservicesinfotooltip { get; set; }
        public string frontendadfinanceexperttitle { get; set; }
        public string frontendadfinanceexpertimgalt { get; set; }
        public string frontendadfinanceexperttext { get; set; }
        public string frontendadadsimilarofferdescriptiontabinfoitem { get; set; }
        public string frontendadadsimilarofferdescriptionemailnotificationsinfoitem { get; set; }
        public string frontendadadsimilarofferdescriptionparagraph { get; set; }
    }

    public class Featureflags
    {
        public bool isAdAvmModuleEnabled { get; set; }
        public bool isAdFinanceBannerEnabled { get; set; }
        public bool isAdFinanceLinkEnabled { get; set; }
        public bool isAdMortgageSimulatorEnabled { get; set; }
        public bool isAdvertsDiscountServiceEnabled { get; set; }
        public bool isAgentsSubaccountsEnabled { get; set; }
        public bool isBulkSchedulingEnabled { get; set; }
        public bool isFeaturedDevelopmentVASEnabled { get; set; }
        public bool isListingRentPriceEnabled { get; set; }
        public bool isMapEnabled { get; set; }
        public bool isNewApartmentsForSalePostingFormEnabled { get; set; }
        public bool isNewPromotePageEnabled { get; set; }
        public bool isOldSaveSearchQueryEnabled { get; set; }
        public bool shouldRolloutLocationService { get; set; }
        public bool isOlxAdvertPromotionEnabled { get; set; }
        public bool isOtodomAnalyticsEnabled { get; set; }
        public bool isProjectREDEnabled { get; set; }
        public bool isPushNotificationServiceWorkerEnabled { get; set; }
        public bool isPushNotificationsToggleEnabled { get; set; }
        public bool isSavedSearchFrequencyChangeEnabled { get; set; }
        public bool isVasRecommendationsEnabled { get; set; }
        public bool isVasSchedulingEnabled { get; set; }
        public bool shouldUseNewInformation { get; set; }
        public bool shouldUseNewProjectPage { get; set; }
        public bool isFinanceCheckboxEnabled { get; set; }
        public bool shouldUsePacketsWithDuration { get; set; }
        public bool isObservedAdsPageEnabled { get; set; }
        public bool isNewAskAboutPriceEnabled { get; set; }
        public bool areAddOnsEnabled { get; set; }
        public bool isHomepageVasConverted { get; set; }
        public bool isDevelopersPricingCardsDesignEnabled { get; set; }
        public bool isUnifiedBusinessInvoicesPageActive { get; set; }
        public bool isSpecialOfferModalEnabled { get; set; }
        public bool isRegularMyAccountAdsPageEnabled { get; set; }
        public bool isBikPromotionEnabled { get; set; }
        public bool isSpecialOfferForUnitsEnabled { get; set; }
        public bool isStudioFlatCategoryEnabled { get; set; }
        public bool isRegularRoomCategoryMigratedToNMF { get; set; }
        public bool isSpecialOfferForUnitsExperimentEnabled { get; set; }
        public string __typename { get; set; }
    }

    public class Query
    {
        public string lang { get; set; }
        public string id { get; set; }
    }

    public class Runtimeconfig
    {
        public string appleCognitoLoginUrl { get; set; }
        public string env { get; set; }
        public string fbCognitoLoginUrl { get; set; }
        public string fileUploadServiceUrl { get; set; }
        public string googleApiKey { get; set; }
        public bool isOneTrustAutoDeleteEnabled { get; set; }
        public bool isOneTrustEnabled { get; set; }
        public bool isSentryEnabled { get; set; }
        public string googleCognitoLoginUrl { get; set; }
        public string oneTrustSiteId { get; set; }
        public string ninjaEnvironment { get; set; }
        public string nodeEnv { get; set; }
        public string pushNotificationPublicKey { get; set; }
        public string sentryDsn { get; set; }
        public string sentryEnvironment { get; set; }
        public float sentrySampleRateClient { get; set; }
        public float sentryTracesSampleRateClient { get; set; }
        public string staticAssetsPrefix { get; set; }
    }
    #endregion
}
