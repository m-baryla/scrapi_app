using scrapi.Base.Model;
using scrapi.DbAccess;

namespace scrapi.DtoBase
{
    public class DataBaseAcces : IDataBaseAcces
    {
        private ISqlDataAccess sqlDataAccess;

        public DataBaseAcces(ISqlDataAccess sqlDataAccess)
        {
            this.sqlDataAccess = sqlDataAccess;
        }
        public Task<IEnumerable<UrlCollection>> GetUrlCollection() => sqlDataAccess.LoadData<UrlCollection, dynamic>("[CORE].[GetUrlCollection]", new { });
        public Task<IEnumerable<string>> GetScrapNotify() => sqlDataAccess.LoadData<string, dynamic>("[CORE].[GetScrapNotify]", new { });
        public Task AddNewScrap(ModelScrap modelScrap) => sqlDataAccess.SaveData("[CORE].[AddNewScrap]", new
        {
            modelScrap.ScrapItem,
            modelScrap.OfferId,
            modelScrap.Make,
            modelScrap.Model,
            modelScrap.OfferName,
            modelScrap.PageBaseUrl,
            modelScrap.SubPageBaseUrl,
            modelScrap.Price,
            modelScrap.Location,
            modelScrap.AddedDate,
            modelScrap.AdditionalInfo1,
            modelScrap.AdditionalInfo2,
            modelScrap.AdditionalInfo3,
            modelScrap.AdditionalInfo4,
            modelScrap.AdditionalInfo5,
            modelScrap.AdditionalInfo6,
            modelScrap.AdditionalInfo7,
            modelScrap.AdditionalInfo8,
            modelScrap.AdditionalInfo9,
            modelScrap.AdditionalInfo10
        });
    }
}
