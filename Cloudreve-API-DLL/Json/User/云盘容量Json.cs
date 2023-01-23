namespace Cloudreve_API_DLL.Json.User
{
    public class CloudDriveSizeJson
    {
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        public class Data
        {
            /// <summary>
            /// 已使用空间
            /// </summary>
            public long used { get; set; }
            /// <summary>
            /// ??
            /// </summary>
            public long free { get; set; }
            /// <summary>
            /// 总空间
            /// </summary>
            public long total { get; set; }
        }

        public class CloudDriveSizeReturnJson
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
