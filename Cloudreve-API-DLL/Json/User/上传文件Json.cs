namespace Cloudreve_API_DLL.Json.User
{
    public class UploadFilesJson
    {
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        /// <summary>
        /// 文件上传Json
        /// </summary>
        public class PUT_UploadFilesDataJson
        {
            /// <summary>
            /// 上传位置(云盘的位置)
            /// </summary>
            public string path { get; set; }
            /// <summary>
            /// 文件大小
            /// </summary>
            public long size { get; set; }
            /// <summary>
            /// 文件名称
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 存储策略
            /// </summary>
            public string policy_id { get; set; }
            /// <summary>
            /// 最后修改时间戳
            /// </summary>
            public long last_modified { get; set; }
            /// <summary>
            /// 计算文件大小
            /// </summary>
            /// <param name="FilesPath">文件路径</param>
            /// <returns>long 类型 文件大小</returns>
            public long Size(string FilesPath)
            {
                return size = new FileInfo(FilesPath).Length;
            }
            /// <summary>
            /// 获取文件名
            /// </summary>
            /// <param name="FilesPath">文件路径</param>
            /// <returns>string 类型 文件名称</returns>
            public string Name(string FilesPath)
            {
                return name = Path.GetFileName(FilesPath);
            }
            /// <summary>
            /// 合成上传信息Json
            /// </summary>
            /// <param name="UploadFilesDataJson">传入上传信息</param>
            /// <returns>string 类型 上传信息Json</returns>
            public string UpdataJson(PUT_UploadFilesDataJson UploadFilesDataJson)
            {
                return System.Text.Json.JsonSerializer.Serialize(UploadFilesDataJson);//合成Json
            }
            /// <summary>
            /// 合成上传信息
            /// </summary>
            /// <param name="FilesPath">本地文件位置</param>
            /// <param name="policy">存储策略</param>
            /// <param name="CloudFilesPath">上传云盘位置</param>
            /// <returns>string 类型 上传信息</returns>
            public string? Updata(string FilesPath, string policy, string CloudFilesPath = "/")
            {
                if (File.Exists(FilesPath))
                {
                    path = CloudFilesPath;
                    Name(FilesPath);
                    Size(FilesPath);
                    policy_id = policy;
                    //last_modified = DateTimeOffset.Now.ToUnixTimeSeconds();//获取时间戳(Unix)
                    DateTimeOffset TempTime = File.GetLastWriteTime(FilesPath);
                    last_modified = TempTime.ToUnixTimeMilliseconds();
                    return UpdataJson(this);
                }
                return null;
            }
        }
        public class Data
        {
            /// <summary>
            /// 任务ID
            /// </summary>
            public string sessionID { get; set; }
            /// <summary>
            /// 文件大小
            /// </summary>
            public int chunkSize { get; set; }
            /// <summary>
            /// 时间戳
            /// </summary>
            public int expires { get; set; }
        }
        /// <summary>
        /// PUT 上传文件返回Json
        /// </summary>
        public class PUT_UploadFilesReturnJson
        {
            /// <summary>
            /// 响应代号
            /// </summary>
            public int code { get; set; }
            /// <summary>
            /// 返回内容
            /// </summary>
            public Data? data { get; set; }
            /// <summary>
            /// 反馈信息
            /// </summary>
            public string msg { get; set; }
        }
        /// <summary>
        /// POST 上传文件返回Json
        /// </summary>
        public class POST_UploadFilesReturnJson
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
