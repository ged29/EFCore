using Microsoft.AspNetCore.Http;
using System;
using System.Net;

namespace EfCoreInAction.Helpers
{
    public static class IsLocalExtension
    {
        private const string NullIpAddress = "::1";

        public static bool IsLocal(this HttpRequest httpRequest)
        {
            var connection = httpRequest.HttpContext.Connection;
            if (connection.RemoteIpAddress.IsSet())
            {
                return connection.LocalIpAddress.IsSet()
                    ? connection.RemoteIpAddress.Equals(connection.LocalIpAddress)
                    : IPAddress.IsLoopback(connection.RemoteIpAddress);
            }
            return true;
        }

        public static void ThrowErrorIfNotLocal(this HttpRequest httpRequest)
        {
            if (!httpRequest.IsLocal())
            {
                throw new InvalidOperationException("You can only call this command if you are running locally");
            }
        }

        public static bool IsSet(this IPAddress address)
        {
            return address != null && address.ToString().Equals(NullIpAddress);
        }
    }
}
