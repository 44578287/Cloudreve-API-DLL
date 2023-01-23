namespace Cloudreve_API_DLL.Json.User
{
    public class FilesDataJson
    {
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        public class Data
        {
            /// <summary>
            /// 创建时间
            /// </summary>
            public string created_at { get; set; }
            /// <summary>
            /// 更新日期
            /// </summary>
            public string updated_at { get; set; }
            /// <summary>
            /// 存储策略
            /// </summary>
            public string policy { get; set; }
            /// <summary>
            /// 大小
            /// </summary>
            public int size { get; set; }
            /// <summary>
            /// 子文件夹编号
            /// </summary>
            public int child_folder_num { get; set; }
            /// <summary>
            /// 文件号??
            /// </summary>
            public int child_file_num { get; set; }
            /// <summary>
            /// 路径
            /// </summary>
            public string path { get; set; }
            /// <summary>
            /// 查询_日期
            /// </summary>
            public string query_date { get; set; }
        }
        /// <summary>
        /// 获取文件信息返回Json
        /// </summary>
        public class FilesDataReturnJson
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
