namespace Cloudreve_API_DLL.Json.User
{
    public class FileSourceJson
    {
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        /// <summary>
        /// 合成获取外链请求Json
        /// </summary>
        public class FileSourceDataJson
        {
            /// <summary>
            /// 文件ID
            /// </summary>
            public List<string>? items { get; set; }
            public string? FileSourceDataReturnJson()
            {
                return System.Text.Json.JsonSerializer.Serialize(this);
            }
            public static string? FileSourceDataReturnJson(List<string>? items)
            {
                return System.Text.Json.JsonSerializer.Serialize<FileSourceDataJson>(new() { items = items });
            }
        }

        public class DataItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int parent { get; set; }
        }
        /// <summary>
        /// 获取外链返回Json
        /// </summary>
        public class FileSourceDataReturnJson
        {
            /// <summary>
            /// 响应代号
            /// </summary>
            public int code { get; set; }
            /// <summary>
            /// 内容
            /// </summary>
            public List<DataItem>? data { get; set; }
            /// <summary>
            /// 反馈信息
            /// </summary>
            public string msg { get; set; }
        }

#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
    }
}
