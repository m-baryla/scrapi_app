using scrapi.DtoBase;

namespace scrapi.Engine
{
    internal interface IScrapEngine
    {
        void Scrap(string baseUrl, string make, string model, IDataBaseAcces dtoBase,string fromPage);
    }
}