namespace Cloudreve_API_DLL.Json.User
{
    public class DeleteFiles
    {
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        /// <summary>
        /// 删除文件合成Json
        /// </summary>
        public class DeleteFilesDataJson
        {
            /// <summary>
            /// 文件ID
            /// </summary>
            public List<string>? items { get; set; }
            /// <summary>
            /// 文件夹ID
            /// </summary>
            public List<string>? dirs { get; set; }
            public string? DeleteFilesDataReturnJson()
            {
                return System.Text.Json.JsonSerializer.Serialize(this);
            }
            public static string? DeleteFilesDataReturnJson(DeleteFilesDataJson data)
            {
                return System.Text.Json.JsonSerializer.Serialize<DeleteFilesDataJson>(new() { items = data.items, dirs = data.dirs });
            }
            public static string? DeleteFilesDataReturnJson(List<string>? items, List<string>? dirs)
            {
                return System.Text.Json.JsonSerializer.Serialize<DeleteFilesDataJson>(new() { items = items, dirs = dirs });
            }
        }
        /// <summary>
        /// 删除文件返回Json
        /// </summary>
        public class DeleteFilesDataReturnJson
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
