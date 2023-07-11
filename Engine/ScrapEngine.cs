using scrapi.Base.Model;
using scrapi.DtoBase;
using scrapi.Profile.Olx;
using scrapi.Profile.Otomoto;
using scrapi.Profile.UniformAdvantage;
using scrapig.Profile.Otodom;

namespace scrapi.Engine
{
    internal class ScrapEngine: IScrapEngine
    {
        public void Scrap(string baseUrl, string make, string model, IDataBaseAcces dtoBase, string fromPage)
        {
            if (fromPage == ScrapItem.OLX.ToString())
            {
                OlxEngine.Scrap(fromPage, baseUrl, make, model, dtoBase);
            }
            if (fromPage == ScrapItem.OTOMOTO.ToString())
            {
                OtomotoEngine.Scrap(fromPage, baseUrl, make, model, dtoBase);
            }
            if (fromPage == ScrapItem.OTODOM.ToString())
            {
                OtodomEngine.Scrap(fromPage,baseUrl,make,model, dtoBase);
            }
            if (fromPage == ScrapItem.UNIFORMA_ADVANTAGE.ToString())
            {
                UniformAdvantageEngine.Scrap(fromPage, baseUrl, make, model, dtoBase);
            }
        }
    }
}
