using System;
using Microsoft.AspNetCore.Http;

namespace ServiceLayer.Utils
{
    public class CookieTemplate
    {
        private readonly string cookieName;
        private readonly IRequestCookieCollection cookiesIn;
        private readonly IResponseCookies cookiesOut;

        protected CookieTemplate(string cookieName, IRequestCookieCollection cookiesIn, IResponseCookies cookiesOut = null)
        {
            this.cookieName = cookieName;
            this.cookiesIn = cookiesIn ?? throw new ArgumentNullException(nameof(cookiesIn));
            this.cookiesOut = cookiesOut;
        }

        protected virtual int ExpiresInThisManyDays => 0;

        public void AddOrUpdateCookie(string value)
        {
            if (cookiesOut == null)
            {
                throw new NullReferenceException("You must supply a IResponseCookies value if you want to use this command.");
            }

            var options = new CookieOptions();
            if (ExpiresInThisManyDays > 0)
            {
                //set/update expires if ExpiresInThisManyDays has been overrriden with postive num
                options.Expires = DateTime.Now.AddDays(ExpiresInThisManyDays);
            }

            cookiesOut.Append(cookieName, value, options);
        }

        public bool Exists()
        {
            return cookiesIn[cookieName] != null;
        }

        public string GetValue()
        {
            var cookie = cookiesIn[cookieName];
            return string.IsNullOrEmpty(cookie) ? null : cookie;
        }

        public void DeleteCookie()
        {
            if (cookiesOut == null)
            {
                throw new NullReferenceException("You must supply a IResponseCookies value if you want to use this command.");
            }

            if (Exists())
            {
                CookieOptions options = new CookieOptions { Expires = DateTime.Now.AddYears(-1) };
                cookiesOut.Append(cookieName, "", options);
            }
        }
    }
}