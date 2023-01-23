namespace Cloudreve_API_DLL.Json.User
{
    public class FileSearchJson
    {
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        public class ObjectsItem
        {
            /// <summary>
            /// 文件ID
            /// </summary>
            public string id { get; set; }
            /// <summary>
            /// 文件名
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 所在路径
            /// </summary>
            public string path { get; set; }
            /// <summary>
            /// 像素 (*,*)
            /// </summary>
            public string pic { get; set; }
            /// <summary>
            /// 文件大小
            /// </summary>
            public int size { get; set; }
            /// <summary>
            /// 文件类型
            /// </summary>
            public string type { get; set; }
            /// <summary>
            /// 上传时间
            /// </summary>
            public string date { get; set; }
            /// <summary>
            /// 创建日期
            /// </summary>
            public string create_date { get; set; }
            /// <summary>
            /// 启用来源
            /// </summary>
            public bool source_enabled { get; set; }
        }

        public class Data
        {
            /// <summary>
            /// 搜索到的文件
            /// </summary>
            public List<ObjectsItem>? objects { get; set; }
            /// <summary>
            /// 父类
            /// </summary>
            public int parent { get; set; }
        }

        public class FileSearchReturnJson
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
