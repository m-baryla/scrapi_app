using scrapi.Base.Model;

namespace scrapi.DtoBase
{
    public interface IDataBaseAcces
    {
        Task AddNewScrap(ModelScrap modelScrapCars);
        Task<IEnumerable<string>> GetScrapNotify();
        Task<IEnumerable<UrlCollection>> GetUrlCollection();
    }
}