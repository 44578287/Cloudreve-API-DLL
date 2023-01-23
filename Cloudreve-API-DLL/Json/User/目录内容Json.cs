namespace Cloudreve_API_DLL.Json.User
{
    public class DirectoryDataJson
    {
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        public class ObjectsItem
        {
            /// <summary>
            /// ID
            /// </summary>
            public string id { get; set; }
            /// <summary>
            /// 名子
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 路径
            /// </summary>
            public string path { get; set; }
            /// <summary>
            /// 图片
            /// </summary>
            public string pic { get; set; }
            /// <summary>
            /// 大小
            /// </summary>
            public int size { get; set; }
            /// <summary>
            /// 类型
            /// </summary>
            public string type { get; set; }
            /// <summary>
            /// 资料
            /// </summary>
            public string date { get; set; }
            /// <summary>
            /// 创建日期
            /// </summary>
            public string create_date { get; set; }
            /// <summary>
            /// 启用的源码
            /// </summary>
            public bool source_enabled { get; set; }
        }

        public class Policy
        {
            /// <summary>
            /// ID
            /// </summary>
            public string id { get; set; }
            /// <summary>
            /// 名子
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 类型
            /// </summary>
            public string type { get; set; }
            /// <summary>
            /// 最大大小
            /// </summary>
            public int max_size { get; set; }
            /// <summary>
            /// 文件类型
            /// </summary>
            public List<string> file_type { get; set; }
        }

        public class Data
        {
            /// <summary>
            /// 父类
            /// </summary>
            public string parent { get; set; }
            /// <summary>
            /// 对象
            /// </summary>
            public List<ObjectsItem> objects { get; set; }
            /// <summary>
            /// 策略
            /// </summary>
            public Policy policy { get; set; }
        }
        /// <summary>
        /// 目录内容返回Json
        /// </summary>
        public class DirectoryDataReturnJson
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
