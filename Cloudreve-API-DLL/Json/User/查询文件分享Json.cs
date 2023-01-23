namespace Cloudreve_API_DLL.Json.User
{
    public class FileShareShowJson
    {
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        public class Source
        {
            /// <summary>
            /// 文件名
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 大小
            /// </summary>
            public int size { get; set; }
        }
        public class ItemsItem
        {
            /// <summary>
            /// 分享ID
            /// </summary>
            public string key { get; set; }
            /// <summary>
            /// 是否为文件夹
            /// </summary>
            public bool is_dir { get; set; }
            /// <summary>
            /// 密码
            /// </summary>
            public string password { get; set; }
            /// <summary>
            /// 创建日期
            /// </summary>
            public string create_date { get; set; }
            /// <summary>
            /// 下载次数
            /// </summary>
            public int downloads { get; set; }
            /// <summary>
            /// 剩余下载量
            /// </summary>
            public int remain_downloads { get; set; }
            /// <summary>
            /// 查看次数
            /// </summary>
            public int views { get; set; }
            /// <summary>
            /// 过期时间
            /// </summary>
            public int expire { get; set; }
            /// <summary>
            /// 是否可以预览
            /// </summary>
            public bool preview { get; set; }
            /// <summary>
            /// 名称&大小
            /// </summary>
            public Source source { get; set; }
        }
        public class Data
        {
            /// <summary>
            /// 具体分享内容
            /// </summary>
            public List<ItemsItem>? items { get; set; }
            /// <summary>
            /// 生成的分享链接总数
            /// </summary>
            public int total { get; set; }
        }
        /// <summary>
        /// 查询分享文件返回Json
        /// </summary>
        public class FileShareShowreturnJson
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
        /// <summary>
        /// 设置Json合成
        /// </summary>
        public class SettingsJson
        {
            /// <summary>
            /// 设置项目
            /// </summary>
            public string? prop { get; set; }
            /// <summary>
            /// 参数
            /// </summary>
            public string? value { get; set; }
            public string? SettingsDataReturnJson()
            {
                return System.Text.Json.JsonSerializer.Serialize(this);
            }
            public static string? SettingsDataReturnJson(string? prop, string? value)
            {
                return System.Text.Json.JsonSerializer.Serialize<SettingsJson>(new() { prop = prop, value = value });
            }
        }
        /// <summary>
        /// 设置选项
        /// </summary>
        public enum Settings
        {
            /// <summary>
            /// 设置密码
            /// </summary>
            password,
            /// <summary>
            /// 设置文件预览
            /// </summary>
            preview_enabled
        }
        /// <summary>
        /// 设置返回Json
        /// </summary>
        public class SettingsReturnJson
        {
            /// <summary>
            /// 响应代号
            /// </summary>
            public int code { get; set; }
            /// <summary>
            /// 内容
            /// </summary>
            public object? data { get; set; }
            /// <summary>
            /// 反馈信息
            /// </summary>
            public string msg { get; set; }
        }
        /// <summary>
        /// 删除返回Json
        /// </summary>
        public class DeltetShareReturnJson
        {
            /// <summary>
            /// 响应代号
            /// </summary>
            public int code { get; set; }
            /// <summary>
            /// 反馈信息
            /// </summary>
            public string msg { get; set; }
        }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
    }
}
