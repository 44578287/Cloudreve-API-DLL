namespace Cloudreve_API_DLL.Json.User
{
    public class ShareSearchJson
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
            /// 剩余下载次数
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
            /// 允许预览
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
            /// 内容
            /// </summary>
            public List<ItemsItem>? items { get; set; }
            /// <summary>
            /// 总数
            /// </summary>
            public int total { get; set; }
        }

        public class ShareSearchReturnJson
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
