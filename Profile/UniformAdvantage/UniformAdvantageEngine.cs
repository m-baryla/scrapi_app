using scrapi.Base.Model;
using scrapi.DtoBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scrapi.Profile.UniformAdvantage
{
    public class UniformAdvantageEngine
    {
        public static void Scrap(string? scrapItem, string baseUrl, string make, string model, IDataBaseAcces dtoBase)
        {
            ModelScrap modelScrap = new ModelScrap();

            var uniformAdvantageData = REST.GET<RootobjectUniformAdvantageEngine>(baseUrl);


            modelScrap.ScrapItem = scrapItem;
            modelScrap.OfferName = uniformAdvantageData.Result.product.productName;
            modelScrap.OfferId = string.Concat(scrapItem, "_", model);
            modelScrap.Make = uniformAdvantageData.Result.product.brand;
            modelScrap.Model = uniformAdvantageData.Result.product.pdpFabricDescription;
            modelScrap.PageBaseUrl = uniformAdvantageData.Result.product.selectedProductUrl;
            modelScrap.SubPageBaseUrl = baseUrl;
            modelScrap.Price = string.Concat(uniformAdvantageData.Result.product.price.sales.decimalPrice," ", uniformAdvantageData.Result.product.price.sales.currency);
            modelScrap.Location = null;
            modelScrap.AddedDate = "";
            modelScrap.AdditionalInfo1 = string.Concat("Size : ",uniformAdvantageData.Result.product.variationAttributes[0].values.Where(o => o.id == "M").Select(o => o.value).FirstOrDefault()," ", uniformAdvantageData.Result.product.variationAttributes[0].values.Where(o => o.id == "M").Select(o => o.stockMessage).FirstOrDefault());
            modelScrap.AdditionalInfo2 = string.Concat("Size : ",uniformAdvantageData.Result.product.variationAttributes[0].values.Where(o => o.id == "L").Select(o => o.value).FirstOrDefault()," ", uniformAdvantageData.Result.product.variationAttributes[0].values.Where(o => o.id == "L").Select(o => o.stockMessage).FirstOrDefault());
            modelScrap.AdditionalInfo3 = string.Concat("Size : ",uniformAdvantageData.Result.product.variationAttributes[0].values.Where(o => o.id == "XL").Select(o => o.value).FirstOrDefault()," ", uniformAdvantageData.Result.product.variationAttributes[0].values.Where(o => o.id == "XL").Select(o => o.stockMessage).FirstOrDefault()); 
            modelScrap.AdditionalInfo4 = string.Concat("Size : ",uniformAdvantageData.Result.product.variationAttributes[0].values.Where(o => o.id == "2X").Select(o => o.value).FirstOrDefault()," ", uniformAdvantageData.Result.product.variationAttributes[0].values.Where(o => o.id == "2X").Select(o => o.stockMessage).FirstOrDefault());
            modelScrap.AdditionalInfo5 = string.Concat("Size : ",uniformAdvantageData.Result.product.variationAttributes[0].values.Where(o => o.id == "3X").Select(o => o.value).FirstOrDefault()," ", uniformAdvantageData.Result.product.variationAttributes[0].values.Where(o => o.id == "3X").Select(o => o.stockMessage).FirstOrDefault());
            modelScrap.AdditionalInfo6 = null;
            modelScrap.AdditionalInfo7 = null;
            modelScrap.AdditionalInfo8 = null;
            modelScrap.AdditionalInfo9 = uniformAdvantageData.Result.product.images.pdp[0].url; 
            modelScrap.AdditionalInfo10 = uniformAdvantageData.Result.product.images.pdp[1].url; 
            dtoBase.AddNewScrap(modelScrap);
        }
    }
}
#region UNIFORMA_ADVANTAGE DATA

public class RootobjectUniformAdvantageEngine
{
    public string action { get; set; }
    public string queryString { get; set; }
    public string locale { get; set; }
    public Product product { get; set; }
    public Addtocarturl addToCartUrl { get; set; }
    public Resources resources { get; set; }
    public string quickViewFullDetailMsg { get; set; }
    public string closeButtonText { get; set; }
    public string enterDialogMessage { get; set; }
    public string template { get; set; }
    public Yotpowidgetdata yotpoWidgetData { get; set; }
    public Embroiderytextprice embroideryTextPrice { get; set; }
    public string renderedTemplate { get; set; }
    public string productUrl { get; set; }
}

public class Product
{
    public string uuid { get; set; }
    public string id { get; set; }
    public string productName { get; set; }
    public string productType { get; set; }
    public string brand { get; set; }
    public bool isSoftStretch { get; set; }
    public object isMeshPanel { get; set; }
    public object isLightweight { get; set; }
    public object isSlipResistant { get; set; }
    public bool isNew { get; set; }
    public object isIrregular { get; set; }
    public object isSlightlyIrregular { get; set; }
    public bool isNewBadge { get; set; }
    public bool isShipsFree { get; set; }
    public string plpSaleMessage { get; set; }
    public object lengthAvailableIn { get; set; }
    public bool isOnlyAtUa { get; set; }
    public bool isNonMerchProduct { get; set; }
    public bool isGiftCard { get; set; }
    public string firstColor { get; set; }
    public object isSkuOnSaleOrClearance { get; set; }
    public Price price { get; set; }
    public string renderedPrice { get; set; }
    public bool hideUpChargeText { get; set; }
    public Images images { get; set; }
    public Imageszoom imagesZoom { get; set; }
    public int selectedQuantity { get; set; }
    public int minOrderQuantity { get; set; }
    public int maxOrderQuantity { get; set; }
    public Variationattribute[] variationAttributes { get; set; }
    public object longDescription { get; set; }
    public object shortDescription { get; set; }
    public float rating { get; set; }
    public object promotions { get; set; }
    public object attributes { get; set; }
    public Availability availability { get; set; }
    public bool available { get; set; }
    public object[] options { get; set; }
    public Quantity[] quantities { get; set; }
    public Masterprice masterPrice { get; set; }
    public string selectedProductUrl { get; set; }
    public bool readyToOrder { get; set; }
    public bool online { get; set; }
    public string pageTitle { get; set; }
    public string pageDescription { get; set; }
    public string pageKeywords { get; set; }
    public object[] pageMetaTags { get; set; }
    public object template { get; set; }
    public string thresholdMessage { get; set; }
    public string pdpDynamicSaleMessage { get; set; }
    public string antimicrobial { get; set; }
    public bool isApmaBadge { get; set; }
    public bool isEcoFriendly { get; set; }
    public bool isEmbroiderable { get; set; }
    public bool isMoistureWicking { get; set; }
    public bool isPrints { get; set; }
    public bool isStainResistant { get; set; }
    public bool isStretch { get; set; }
    public bool isStyleOnSale { get; set; }
    public bool isWrinkleResistant { get; set; }
    public string legShape { get; set; }
    public string length { get; set; }
    public string pdpAdditionalDescription { get; set; }
    public string pdpDisclaimer { get; set; }
    public string[] pdpFabricBullets { get; set; }
    public string pdpFabricDescription { get; set; }
    public string pdpProductDescription { get; set; }
    public string colorMatch { get; set; }
    public string[] pdpProductDetailsBullets { get; set; }
    public string[] upChargeText { get; set; }
    public object isSkuOnSale { get; set; }
    public object isSkuOnClearance { get; set; }
    public string masterID { get; set; }
    public Availablerecommendation[] availableRecommendations { get; set; }
    public object doNotDisplaySizeChart { get; set; }
    public object pdpVideoID { get; set; }
    public string pdpStaticMessageTop { get; set; }
    public string pdpStaticMessageBottom { get; set; }
    public bool isStyleOnClearance { get; set; }
    public float regularPrice { get; set; }
    public Regularpriceobject regularPriceObject { get; set; }
    public object regularPriceBookPrice { get; set; }
    public string subcollection { get; set; }
    public Fylclasses fylClasses { get; set; }
    public string fylSubcollection { get; set; }
    public string fylSwatchArray { get; set; }
    public object fylSortingRule { get; set; }
    public string fylGroup { get; set; }
    public bool isPrintsAvailable { get; set; }
    public string selectedSize { get; set; }
    public string displaySizeChartID { get; set; }
    public string displayHowToMeasureID { get; set; }
    public string dispayProductAdsID { get; set; }
    public object itemStatus { get; set; }
    public string gender { get; set; }
    public bool showFylLength { get; set; }
    public string styleNumber { get; set; }
    public bool isVariant { get; set; }
    public string[] fylProductsAvailable { get; set; }
    public string productUrl { get; set; }
    public string pdParams { get; set; }
    public object categoryBanner { get; set; }
    public Embroiderygarmenttype embroideryGarmentType { get; set; }
    public Embroideryplacementgroup embroideryPlacementGroup { get; set; }
    public string axClass { get; set; }
    public int baseTime { get; set; }
    public int priceTime { get; set; }
    public int imagesTime { get; set; }
    public int variationAttributesTime { get; set; }
    public int colorCategoryTime { get; set; }
    public int sizeChartTime { get; set; }
    public int colorSwatchCodeTime { get; set; }
    public int pdpCustomAttributesTime { get; set; }
    public int pageDesignerModulesTime { get; set; }
    public int embroideryTime { get; set; }
    public int rawTime { get; set; }
    public int templateTime { get; set; }
    public int thresholdMessageTime { get; set; }
}

public class Price
{
    public Sales sales { get; set; }
    public List list { get; set; }
}

public class Sales
{
    public int value { get; set; }
    public string currency { get; set; }
    public string formatted { get; set; }
    public string decimalPrice { get; set; }
    public float pricebookPrice { get; set; }
}

public class List
{
    public string currency { get; set; }
    public float decimalPrice { get; set; }
    public string formatted { get; set; }
    public float value { get; set; }
}

public class Images
{
    public Pdp[] pdp { get; set; }
    public Addedmodal[] addedModal { get; set; }
    public Quickview[] quickView { get; set; }
    public Multicolormodal[] multiColorModal { get; set; }
}

public class Pdp
{
    public string alt { get; set; }
    public string url { get; set; }
    public string title { get; set; }
    public bool isPlaceHolder { get; set; }
}

public class Addedmodal
{
    public string alt { get; set; }
    public string url { get; set; }
    public string title { get; set; }
    public bool isPlaceHolder { get; set; }
}

public class Quickview
{
    public string alt { get; set; }
    public string url { get; set; }
    public string title { get; set; }
    public bool isPlaceHolder { get; set; }
}

public class Multicolormodal
{
    public string alt { get; set; }
    public string url { get; set; }
    public string title { get; set; }
    public bool isPlaceHolder { get; set; }
}

public class Imageszoom
{
    public Pdpzoom[] pdpZoom { get; set; }
}

public class Pdpzoom
{
    public string alt { get; set; }
    public string url { get; set; }
    public string title { get; set; }
    public bool isPlaceHolder { get; set; }
}

public class Availability
{
    public string[] messages { get; set; }
    public string[] messagesTypes { get; set; }
    public object inStockDate { get; set; }
}

public class Masterprice
{
    public Sales1 sales { get; set; }
    public List1 list { get; set; }
}

public class Sales1
{
    public int value { get; set; }
    public string currency { get; set; }
    public string formatted { get; set; }
    public string decimalPrice { get; set; }
    public float pricebookPrice { get; set; }
}

public class List1
{
    public float value { get; set; }
    public string currency { get; set; }
    public string formatted { get; set; }
    public string decimalPrice { get; set; }
    public float pricebookPrice { get; set; }
}

public class Regularpriceobject
{
    public string currency { get; set; }
    public float decimalPrice { get; set; }
    public string formatted { get; set; }
    public float value { get; set; }
}

public class Fylclasses
{
    public string[][] carousels { get; set; }
}

public class Embroiderygarmenttype
{
}

public class Embroideryplacementgroup
{
}

public class Variationattribute
{
    public string attributeId { get; set; }
    public string displayName { get; set; }
    public string id { get; set; }
    public bool swatchable { get; set; }
    public string displayValue { get; set; }
    public Value[] values { get; set; }
    public string resetUrl { get; set; }
}

public class Value
{
    public string id { get; set; }
    public object description { get; set; }
    public string displayValue { get; set; }
    public string value { get; set; }
    public bool selected { get; set; }
    public bool selectable { get; set; }
    public string url { get; set; }
    public string stockMessage { get; set; }
}

public class Quantity
{
    public string value { get; set; }
    public bool selected { get; set; }
    public string url { get; set; }
}

public class Availablerecommendation
{
    public string id { get; set; }
    public string length { get; set; }
}

public class Addtocarturl
{
}

public class Resources
{
    public string info_selectforstock { get; set; }
    public string assistiveSelectedText { get; set; }
}

public class Yotpowidgetdata
{
    public bool isReviewsEnabled { get; set; }
    public bool isRatingsEnabled { get; set; }
    public string yotpoAppKey { get; set; }
    public Domainaddress domainAddress { get; set; }
    public string productID { get; set; }
    public string productName { get; set; }
    public object productDesc { get; set; }
    public string productModel { get; set; }
    public string productURL { get; set; }
    public string imageURL { get; set; }
    public string productCategory { get; set; }
}

public class Domainaddress
{
}

public class Embroiderytextprice
{
    public Componentpricemap componentPriceMap { get; set; }
    public Componentdiscountedpricemap componentDiscountedPriceMap { get; set; }
}

public class Componentpricemap
{
    public Text Text { get; set; }
    public Customlogo CustomLogo { get; set; }
    public Smallflag SmallFlag { get; set; }
    public Largeflag LargeFlag { get; set; }
    public Stockimage StockImage { get; set; }
    public DIGITIZE DIGITIZE { get; set; }
}

public class Text
{
    public Sales2 sales { get; set; }
    public object list { get; set; }
}

public class Sales2
{
    public int value { get; set; }
    public string currency { get; set; }
    public string formatted { get; set; }
    public string decimalPrice { get; set; }
    public float pricebookPrice { get; set; }
}

public class Customlogo
{
    public Sales3 sales { get; set; }
    public object list { get; set; }
}

public class Sales3
{
    public int value { get; set; }
    public string currency { get; set; }
    public string formatted { get; set; }
    public string decimalPrice { get; set; }
    public float pricebookPrice { get; set; }
}

public class Smallflag
{
    public Sales4 sales { get; set; }
    public List2 list { get; set; }
}

public class Sales4
{
    public int value { get; set; }
    public string currency { get; set; }
    public string formatted { get; set; }
    public string decimalPrice { get; set; }
    public float pricebookPrice { get; set; }
}

public class List2
{
    public int value { get; set; }
    public string currency { get; set; }
    public string formatted { get; set; }
    public string decimalPrice { get; set; }
    public float pricebookPrice { get; set; }
}

public class Largeflag
{
    public Sales5 sales { get; set; }
    public List3 list { get; set; }
}

public class Sales5
{
    public int value { get; set; }
    public string currency { get; set; }
    public string formatted { get; set; }
    public string decimalPrice { get; set; }
    public float pricebookPrice { get; set; }
}

public class List3
{
    public int value { get; set; }
    public string currency { get; set; }
    public string formatted { get; set; }
    public string decimalPrice { get; set; }
    public float pricebookPrice { get; set; }
}

public class Stockimage
{
    public Sales6 sales { get; set; }
    public object list { get; set; }
}

public class Sales6
{
    public int value { get; set; }
    public string currency { get; set; }
    public string formatted { get; set; }
    public string decimalPrice { get; set; }
    public float pricebookPrice { get; set; }
}

public class DIGITIZE
{
    public Sales7 sales { get; set; }
    public object list { get; set; }
}

public class Sales7
{
    public int value { get; set; }
    public string currency { get; set; }
    public string formatted { get; set; }
    public string decimalPrice { get; set; }
    public int pricebookPrice { get; set; }
}

public class Componentdiscountedpricemap
{
}

#endregion