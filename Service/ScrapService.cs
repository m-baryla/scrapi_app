using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using scrapi.Base.Helper;
using scrapi.DtoBase;
using scrapi.EmailSenderNotyf;
using scrapi.Engine;

namespace scrapi.Service
{
    internal class ScrapService
    {
        private readonly IDataBaseAcces _dtoBase;
        private readonly ILogger<ScrapService> _log;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IScrapEngine _engine;
        private readonly string _guid;

        public ScrapService(IDataBaseAcces dtoBase, ILogger<ScrapService> log, IConfiguration config, IMapper mapper, IScrapEngine engine)
        {
            this._dtoBase = dtoBase;
            this._log = log;
            this._config = config;
            this._mapper = mapper;
            this._engine = engine;
            this._guid = GUID.GUIDValue;
        }
        public void Run()
        {
            var urlCollection = _dtoBase.GetUrlCollection();

            foreach (var item in urlCollection.Result)
            {
                _engine.Scrap(item.UrlBase, item.Make, item.Model, _dtoBase, item.ScrapItem);
                _log.LogInformation(string.Concat("ScrapItem: ", item.ScrapItem));
                _log.LogInformation(string.Concat("Make: ", item.Make));
                _log.LogInformation(string.Concat("Model: ", item.Model));
                _log.LogInformation(string.Concat("UrlBase: ", item.UrlBase));
                _log.LogInformation(string.Concat("GUID: ", _guid));

                Notify();
            }
        }
        private void Notify()
        {
            //var to = _config.GetSection("MailNotify").GetValue<string>("to_1");
            //var cc = _config.GetSection("MailNotify").GetValue<string>("from");
            //var subject = _config.GetSection("MailNotify").GetValue<string>("from");

            //var emailNotify = _dtoBase.GetScrapNotify().Result;
            //foreach (var item in emailNotify)
            //{
            //    EmailSender.Send(item, to, null, subject);
            //}
            //_log.LogInformation(string.Concat("Count send email:", emailNotify.Count().ToString()));
        }
    }
}
