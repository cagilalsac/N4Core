using Microsoft.AspNetCore.Mvc;
using N4Core.Enums;
using N4Core.Managers.Bases;
using System.Globalization;

namespace N4Core.Controllers.Bases
{
    public abstract class MvcController : Controller
    {
        protected readonly CultureManagerBase _cultureManager;
        protected readonly CookieManagerBase _cookieManager;
        protected readonly SessionManagerBase _sessionManager;

        protected string _listCardsSessionKey = "ListCardsSessionKey";

        public bool? ListCards
        {
            get
            {
                bool? listCards = null;
                var listCardsSession = _sessionManager.GetSession<string>(_listCardsSessionKey);
                if (listCardsSession is not null)
                {
                    listCards = Convert.ToBoolean(listCardsSession);
                }
                return listCards;
            }
            set
            {
                if (value.HasValue)
                    _sessionManager.SetSession(value.Value.ToString(), _listCardsSessionKey);
            }
        }

        protected MvcController(CultureManagerBase cultureManager, CookieManagerBase cookieManager, SessionManagerBase sessionManager)
        {
            _cultureManager = cultureManager;
            _cookieManager = cookieManager;
            _sessionManager = sessionManager;
            string language = _cookieManager.GetCookie(nameof(Languages));
            CultureInfo culture = _cultureManager.GetCulture(language);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }
    }
}
