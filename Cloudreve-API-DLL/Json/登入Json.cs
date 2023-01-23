namespace Cloudreve_API_DLL.Json
{
    public class LoginJson
    {
        /// <summary>
        /// 登入资料处理Json
        /// </summary>
        public class LoginDataJson
        {
            /// <summary>
            /// 账号
            /// </summary>
            public string? UserName { get; set; }
            /// <summary>
            /// 密码
            /// </summary>
            public string? Password { get; set; }
            /// <summary>
            /// 二步验证 没有留空即可
            /// </summary>
            public string? CaptchaCode { get; set; }
            /// <summary>
            /// 合成登入信息
            /// </summary>
            /// <param name="LoginJson">传入登入信息</param>
            /// <returns>string 类型 登入Json信息</returns>
            public static string LoginReturnJson(LoginDataJson LoginJson)
            {
                return System.Text.Json.JsonSerializer.Serialize(LoginJson);
            }
            /// <summary>
            /// 合成登入信息
            /// </summary>
            /// <param name="UserName">账号</param>
            /// <param name="Password">密码</param>
            /// <param name="CaptchaCode">二步验证 没有留空即可</param>
            /// <returns>string 类型 登入Json信息</returns>
            public static string LoginReturnJson(string UserName, string Password, string CaptchaCode)
            {
                return LoginReturnJson(new LoginDataJson() { UserName = UserName, Password = Password, CaptchaCode = CaptchaCode });//合成登入Json
            }
            /// <summary>
            /// 合成登入信息
            /// </summary>
            public string LoginReturnJson()
            {
                return LoginReturnJson(this);//合成登入Json
            }
        }
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        public class @group
        {
            /// <summary>
            /// ID
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// 名字
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 允许分享
            /// </summary>
            public bool allowShare { get; set; }
            /// <summary>
            /// 允许远程下载
            /// </summary>
            public bool allowRemoteDownload { get; set; }
            /// <summary>
            /// 允许下载档案
            /// </summary>
            public bool allowArchiveDownload { get; set; }
            /// <summary>
            /// 分享下载
            /// </summary>
            public bool shareDownload { get; set; }
            /// <summary>
            /// 压缩
            /// </summary>
            public bool compress { get; set; }
            /// <summary>
            /// webdav 状态
            /// </summary>
            public bool webdav { get; set; }
            /// <summary>
            /// 来源批次
            /// </summary>
            public int sourceBatch { get; set; }
        }

        public class Data
        {
            /// <summary>
            /// ID
            /// </summary>
            public string id { get; set; }
            /// <summary>
            /// 用户名称
            /// </summary>
            public string user_name { get; set; }
            /// <summary>
            /// 外号
            /// </summary>
            public string nickname { get; set; }
            /// <summary>
            /// 状态
            /// </summary>
            public int status { get; set; }
            /// <summary>
            /// 阿凡达????
            /// </summary>
            public string avatar { get; set; }
            /// <summary>
            /// 创建时间
            /// </summary>
            public string created_at { get; set; }
            /// <summary>
            /// 首选_主题
            /// </summary>
            public string preferred_theme { get; set; }
            /// <summary>
            /// 匿名的
            /// </summary>
            public bool anonymous { get; set; }
            /// <summary>
            /// 组
            /// </summary>
            public @group @group { get; set; }
            /// <summary>
            /// 标签
            /// </summary>
            public List<string> tags { get; set; }
        }
        /// <summary>
        /// 登入服务器返回Json
        /// </summary>
        public class LoginReturnJson
        {
            /// <summary>
            /// 响应代号
            /// </summary>
            public int code { get; set; }
            /// <summary>
            /// 内容
            /// </summary>
            public Data? data { get; set; }
            /// <summary>
            /// 反馈信息
            /// </summary>
            public string msg { get; set; }
        }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
    }
}
