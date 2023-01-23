namespace Cloudreve_API_DLL.Json.User
{
    public class ConfigJson
    {
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        public class @group
        {
            /// <summary>
            /// ID
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// 名子
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
            /// 允许档案下载
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

        public class User
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
            /// 身份
            /// </summary>
            public int status { get; set; }
            /// <summary>
            /// 阿凡达?????
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
            /// 匿名
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

        public class Data
        {
            /// <summary>
            /// 标题
            /// </summary>
            public string title { get; set; }
            /// <summary>
            /// 网站简介
            /// </summary>
            public string siteICPId { get; set; }
            /// <summary>
            /// 登录验证码
            /// </summary>
            public bool loginCaptcha { get; set; }
            /// <summary>
            /// 注册验证码
            /// </summary>
            public bool regCaptcha { get; set; }
            /// <summary>
            /// 遗忘验证码
            /// </summary>
            public bool forgetCaptcha { get; set; }
            /// <summary>
            /// 电子邮件状态
            /// </summary>
            public bool emailActive { get; set; }
            /// <summary>
            /// 主题
            /// </summary>
            public string themes { get; set; }
            /// <summary>
            /// 默认主题
            /// </summary>
            public string defaultTheme { get; set; }
            /// <summary>
            /// 展示方法
            /// </summary>
            public string home_view_method { get; set; }
            /// <summary>
            /// 分享_查看方法
            /// </summary>
            public string share_view_method { get; set; }
            /// <summary>
            /// 签名
            /// </summary>
            public bool authn { get; set; }
            /// <summary>
            /// 用户
            /// </summary>
            public User user { get; set; }
            /// <summary>
            /// 验证码识别码
            /// </summary>
            public string captcha_ReCaptchaKey { get; set; }
            /// <summary>
            /// 验证码类型
            /// </summary>
            public string captcha_type { get; set; }
            /// <summary>
            /// 外部身份验证?
            /// </summary>
            public string tcaptcha_captcha_app_id { get; set; }
            /// <summary>
            /// 注册启用
            /// </summary>
            public bool registerEnabled { get; set; }
        }
        /// <summary>
        /// 获取Config返回Json
        /// </summary>
        public class ConfigReturnJson
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
