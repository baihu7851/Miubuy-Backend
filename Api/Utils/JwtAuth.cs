using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Http.Filters;
using Jose;

namespace Api.Utils
{
    public class JwtAuth : ActionFilterAttribute
    {
        private const string Key = "miumiu";

        public static Dictionary<string, object> GetToken(string token)
        {
            return JWT.Decode<Dictionary<string, object>>(token, Encoding.UTF8.GetBytes(Key), JwsAlgorithm.HS512);
        }

        public static int GetTokenId(string token)
        {
            var tokenData = JWT.Decode<Dictionary<string, object>>(token, Encoding.UTF8.GetBytes(Key), JwsAlgorithm.HS512);
            return (int)tokenData["Id"];
        }

        public static int GetTokenPermission(string token)
        {
            var tokenData = JWT.Decode<Dictionary<string, object>>(token, Encoding.UTF8.GetBytes(Key), JwsAlgorithm.HS512);
            return (int)tokenData["Permission"];
        }

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var request = actionContext.Request;

            if (request.Headers.Authorization == null || request.Headers.Authorization.Scheme != "Bearer")
            {
                throw new Exception("Token 遺失");
            }

            //解密後會回傳Json格式的物件(即加密前的資料)
            try
            {
                var jwtObject = JWT.Decode<Dictionary<string, object>>(request.Headers.Authorization.Parameter,
                    Encoding.UTF8.GetBytes(Key), JwsAlgorithm.HS512);
                if (IsTokenExpired(jwtObject["Exp"].ToString()))
                {
                    throw new Exception("Token 過期");
                }

                base.OnActionExecuting(actionContext);
            }
            catch
            {
                // ignored
            }
        }

        //驗證token時效
        public bool IsTokenExpired(string dateTime)
        {
            return Convert.ToDateTime(dateTime) < DateTime.Now;
        }
    }
}