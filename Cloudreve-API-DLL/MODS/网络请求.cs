using LoongEgg.LoongLogger;
using System.ComponentModel;
using System.Net;
using System.Text;

namespace Cloudreve_API_DLL.MODS
{
    internal class NetworkRequest
    {
        /// <summary>
        /// 通用请求方法
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="cookie">cookie</param>
        /// <param name="data">发送资料</param>
        /// <param name="dataByte">发送资料 Byte</param>
        /// <param name="httpMod">GET POST PUT DELETE 请求模式</param>
        /// <param name="contentType">"默认自动选择 可手动指定 请求头" 请求头</param>
        /// <param name="UserAgent">设置用户UA</param>
        /// <param name="Timeout">设置超时时间</param>
        /// <param name="encoding">编码</param>
        /// <param name="LogOut">设置是否打印日志</param>
        /// <returns>返回响应内容String</returns>
        public static string? HttpRequestToString(string url, string? cookie = null, string? data = null, byte[]? dataByte = null, HttpMods httpMod = HttpMods.GET, string? contentType = null, string? UserAgent = null, int Timeout = 15000, Encoding? encoding = null, bool LogOut = true)
        {
#pragma warning disable SYSLIB0014 // 类型或成员已过时
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
#pragma warning restore SYSLIB0014 // 类型或成员已过时
            HttpWebResponse? response = null;
            HttpMods httpModBak = httpMod;
            if (contentType == null)//自动指定contentType
            {
                switch (httpMod.ToString())
                {
                    case "GET":
                        contentType = "text/html;charset=UTF-8";
                        break;
                    case "POST":
                        contentType = "application/x-www-form-urlencoded";
                        break;
                    case "PATCH":
                        contentType = "application/json";
                        break;
                    case "POST_UPDATA":
                        contentType = "multipart/form-data;charset=utf-8";
                        request.AllowWriteStreamBuffering = false;
                        httpMod = HttpMods.POST;
                        break;
                    case "PUT":
                        contentType = "application/ json";
                        break;
                    case "DELETE":
                        contentType = null;
                        break;
                }
            }
            if (encoding == null)//未指定编码则自动指定ASCII
            {
                encoding = Encoding.ASCII;
            }
            string method = httpMod.ToString();
            request.Method = method;
            request.ContentType = contentType;
            request.UserAgent = UserAgent;
            request.Timeout = Timeout;
            request.Headers.Add("Cookie", cookie);
            if (data != null && method != "GET" && httpModBak != HttpMods.POST_UPDATA && dataByte == null)//string 类型发送
            {
                request.ContentLength = data.Length;
                StreamWriter writer = new StreamWriter(request.GetRequestStream(), encoding);
                writer.Write(data);
                writer.Flush();
            }
            if (httpModBak == HttpMods.POST_UPDATA && data != null && dataByte == null)// 文件路径 发送
            {
                FileStream fs = new FileStream(data, FileMode.Open, FileAccess.Read);
                byte[] bArr = new byte[fs.Length];
                fs.Read(bArr, 0, bArr.Length);
                fs.Close();
                Stream writer = request.GetRequestStream();
                writer.Write(bArr, 0, bArr.Length);
                writer.Close();
            }
            if (httpModBak == HttpMods.POST_UPDATA && data == null && dataByte != null)//Byte 发送
            {
                Stream writer = request.GetRequestStream();
                writer.Write(dataByte, 0, dataByte.Length);
                writer.Close();
            }
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException Message)
            {
                Logger.WriteError("(" + url + ")(" + httpModBak.ToString() + ")" + Message.Message);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("使用:" + httpModBak.ToString() + " 访问:" + url + " 时出现错误:" + Message.Message);
                Console.ForegroundColor = ConsoleColor.White;
                return null;
            }
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("UTF-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            if (LogOut)
                Logger.WriteDebug(retString);
            return retString;
        }
        /// <summary>
        /// 通用请求方法
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="cookie">cookie</param>
        /// <param name="data">发送资料 string 或 文件路径</param>
        /// <param name="dataByte">发送资料 Byte</param>
        /// <param name="httpMod">GET POST PUT DELETE 请求模式</param>
        /// <param name="contentType">"默认自动选择 可手动指定 请求头</param>
        /// <param name="UserAgent">设置用户UA</param>
        /// <param name="Timeout">设置超时时间</param>
        /// <param name="encoding">编码</param>
        /// <param name="LogOut">设置是否打印日志</param>
        /// <returns>返回响应内容</returns>
        public static HttpWebResponse? HttpRequest(string url, string? cookie = null, string? data = null, byte[]? dataByte = null, HttpMods httpMod = HttpMods.GET, string? contentType = null, string? UserAgent = null, int Timeout = 15000, Encoding? encoding = null, bool LogOut = true)
        {
#pragma warning disable SYSLIB0014 // 类型或成员已过时
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
#pragma warning restore SYSLIB0014 // 类型或成员已过时
            HttpWebResponse? response = null;
            HttpMods httpModBak = httpMod;
            if (contentType == null)//自动指定contentType
            {
                switch (httpMod.ToString())
                {
                    case "GET":
                        contentType = "text/html;charset=UTF-8";
                        break;
                    case "POST":
                        contentType = "application/x-www-form-urlencoded";
                        break;
                    case "PATCH":
                        contentType = "application/json";
                        break;
                    case "POST_UPDATA":
                        contentType = "multipart/form-data;charset=utf-8";
                        request.AllowWriteStreamBuffering = false;
                        httpMod = HttpMods.POST;
                        break;
                    case "PUT":
                        contentType = "application/ json";
                        break;
                    case "DELETE":
                        contentType = null;
                        break;
                }
            }
            if (encoding == null)//未指定编码则自动指定ASCII
            {
                encoding = Encoding.ASCII;
            }
            string method = httpMod.ToString();
            request.Method = method;
            request.ContentType = contentType;
            request.UserAgent = UserAgent;
            request.Timeout = Timeout;
            request.Headers.Add("Cookie", cookie);
            if (data != null && method != "GET" && httpModBak != HttpMods.POST_UPDATA && dataByte == null)//string 类型发送
            {
                request.ContentLength = data.Length;
                StreamWriter writer = new StreamWriter(request.GetRequestStream(), encoding);
                writer.Write(data);
                writer.Flush();
            }
            if (httpModBak == HttpMods.POST_UPDATA && data != null && dataByte == null)// 文件路径 发送
            {
                FileStream fs = new FileStream(data, FileMode.Open, FileAccess.Read);
                byte[] bArr = new byte[fs.Length];
                fs.Read(bArr, 0, bArr.Length);
                fs.Close();
                Stream writer = request.GetRequestStream();
                writer.Write(bArr, 0, bArr.Length);
                writer.Close();
            }
            if (httpModBak == HttpMods.POST_UPDATA && data == null && dataByte != null)//Byte 发送
            {
                Stream writer = request.GetRequestStream();
                writer.Write(dataByte, 0, dataByte.Length);
                writer.Close();
            }
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException Message)
            {
                Logger.WriteError("(" + url + ")(" + httpModBak.ToString() + ")" + Message.Message);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("使用:" + httpModBak.ToString() + " 访问:" + url + " 时出现错误:" + Message.Message);
                Console.ForegroundColor = ConsoleColor.White;
                return null;
            }
            if (LogOut)
                Logger.WriteDebug(HttpReturnData.HttpWebDataToString(response, false));
            return response;
        }
        /// <summary>
        /// Http请求实例的类型 
        /// </summary>
        /// <remarks>
        ///     用来设置指定的请求类型
        /// </remarks>
        public enum HttpMods
        {
            /// <summary>
            /// GET请求
            /// </summary>
            [Description("GET")]
            GET,
            /// <summary>
            /// POST请求
            /// </summary>
            [Description("POST")]
            POST,
            /// <summary>
            /// POST请求
            /// </summary>
            [Description("POST_UPDATA")]
            POST_UPDATA,
            /// <summary>
            /// PUT请求
            /// </summary>
            [Description("PUT")]
            PUT,
            /// <summary>
            /// DELTE请求
            /// </summary>
            [Description("DELTE")]
            DELETE,
            /// <summary>
            /// PATCH请求
            /// </summary>
            [Description("PATCH")]
            PATCH
        }
        /// <summary>
        /// 接收web回传内容
        /// </summary>
        public class HttpReturnData
        {
            /// <summary>
            /// web接收回传内容
            /// </summary>
            public HttpWebResponse HttpWebResponse { get; set; }

            /// <summary>
            /// 设置接收web回传内容
            /// </summary>
            public HttpReturnData(HttpWebResponse httpWebResponse)
            {
                HttpWebResponse = httpWebResponse;
            }
            /// <summary>
            /// 以string类型回传接收资料
            /// </summary>
            /// <param name="LogOut">设置是否打印日志</param> 
            public string HttpWebDataToString(bool LogOut = true)
            {
                Stream myResponseStream = HttpWebResponse.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("UTF-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                if (LogOut)
                    Logger.WriteDebug(retString);
                return retString;
            }
            /// <summary>
            /// 以string类型回传接收资料
            /// </summary>
            /// <param name="HttpWebResponse">Http请求回传值</param> 
            /// <param name="LogOut">设置是否打印日志</param> 
            public static string HttpWebDataToString(HttpWebResponse HttpWebResponse, bool LogOut = true)
            {
                Stream myResponseStream = HttpWebResponse.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("UTF-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                if (LogOut)
                    Logger.WriteDebug(retString);
                return retString;
            }
            /// <summary>
            /// 以string类型回传Cookie
            /// </summary>
            /// <param name="LogOut">设置是否打印日志</param> 
            public string? HttpWebCookie(bool LogOut = true)
            {
                string? retString = HttpWebResponse.Headers["Set-Cookie"];
                if (LogOut)
                    Logger.WriteDebug(retString);
                return retString;
            }
            /// <summary>
            /// 以string类型回传Cookie
            /// </summary>
            /// <param name="HttpWebResponse">Http请求回传值</param> 
            /// <param name="LogOut">设置是否打印日志</param> 
            public static string? HttpWebCookie(HttpWebResponse HttpWebResponse, bool LogOut = true)
            {
                string? retString = HttpWebResponse.Headers["Set-Cookie"];
                if (LogOut)
                    Logger.WriteDebug(retString);
                return retString;
            }
        }
    }
}
