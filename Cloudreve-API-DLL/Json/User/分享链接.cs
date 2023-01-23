namespace Cloudreve_API_DLL.Json.User
{
    public class FileShareJson
    {
        /// <summary>
        /// 获取分享链接合成Json
        /// </summary>
        public class FileShareDataJson
        {
            /// <summary>
            /// 文件ID
            /// </summary>
            public string? id { get; set; }
            /// <summary>
            /// 是否为文件夹
            /// </summary>
            public bool is_dir { get; set; }
            /// <summary>
            /// 分享密码
            /// </summary>
            public string? password { get; set; }
            /// <summary>
            /// 限制下载次数 不限制为-1
            /// </summary>
            public int downloads { get; set; }
            /// <summary>
            /// 过期时间 默认86400秒
            /// </summary>
            public int expire { get; set; }
            /// <summary>
            /// 是否可以预览
            /// </summary>
            public bool preview { get; set; }
            public string? FileShareDataReturnJson()
            {
                return System.Text.Json.JsonSerializer.Serialize(this);
            }
            public static string? FileShareDataReturnJson(string ID, bool is_dir = false, string? password = null, int downloads = -1, int expire = 86400, bool preview = true)
            {
                return System.Text.Json.JsonSerializer.Serialize<FileShareDataJson>(new() { id = ID, is_dir = is_dir, password = password, downloads = downloads, expire = expire, preview = preview });
            }
        }
        /// <summary>
        /// 获取分享链接返回Json
        /// </summary>
        public class FileShareDataReturnJson
        {
            /// <summary>
            /// 响应代号
            /// </summary>
            public int code { get; set; }
            /// <summary>
            /// 分享Url
            /// </summary>
            public string? data { get; set; }
            /// <summary>
            /// 反馈信息
            /// </summary>
            public string? msg { get; set; }
        }

    }
}
