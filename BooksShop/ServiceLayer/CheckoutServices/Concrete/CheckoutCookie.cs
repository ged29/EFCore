using Microsoft.AspNetCore.Http;
using ServiceLayer.Utils;

namespace ServiceLayer.CheckoutServices.Concrete
{
    public class CheckoutCookie : CookieTemplate
    {
        public const string CheckoutCookieName = "EfCoreInAction-Checkout";

        public CheckoutCookie(IRequestCookieCollection cookiesIn, IResponseCookies cookiesOut = null)
            : base(CheckoutCookieName, cookiesIn, cookiesOut)
        {
        }

        protected override int ExpiresInThisManyDays => 200;
    }
}