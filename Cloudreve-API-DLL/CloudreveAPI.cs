using Cloudreve_API_DLL.Json.Admin;
using LoongEgg.LoongLogger;
using System.ComponentModel;
using System.Net;
using System.Text.Json;
using static Cloudreve_API_DLL.Json.Admin.GroupsJson;
using static Cloudreve_API_DLL.Json.Admin.GroupsListJson;
using static Cloudreve_API_DLL.Json.Admin.UserListJson;
using static Cloudreve_API_DLL.Json.LoginJson;
using static Cloudreve_API_DLL.Json.User.CloudDriveSizeJson;
using static Cloudreve_API_DLL.Json.User.ConfigJson;
using static Cloudreve_API_DLL.Json.User.DeleteFiles;
using static Cloudreve_API_DLL.Json.User.DeleteUpFileListJson;
using static Cloudreve_API_DLL.Json.User.DirectoryDataJson;
using static Cloudreve_API_DLL.Json.User.DownloadJson;
using static Cloudreve_API_DLL.Json.User.FilesDataJson;
using static Cloudreve_API_DLL.Json.User.FileSearchJson;
using static Cloudreve_API_DLL.Json.User.FileShareJson;
using static Cloudreve_API_DLL.Json.User.FileShareShowJson;
using static Cloudreve_API_DLL.Json.User.FileSourceJson;
using static Cloudreve_API_DLL.Json.User.ShareSearchJson;
using static Cloudreve_API_DLL.Json.User.UploadFilesJson;
using static Cloudreve_API_DLL.MODS.ConsolePrint;
using static Cloudreve_API_DLL.MODS.Encrypt;
using static Cloudreve_API_DLL.MODS.NetworkRequest;

namespace Cloudreve_API_DLL
{
    public class CloudreveAPI
    {
        /// <summary>
        /// 登入Cloudreve
        /// </summary>
        /// <param name="Url">Cloudreve服务器地址</param>
        /// <param name="LoginData">登入信息 可用LoginDataJson.LoginReturnJson(string UserName, string Password, string CaptchaCode) 替代</param>
        /// <param name="ScreenOut">屏幕显示输出</param>
        /// <returns>string 类型 Cookie</returns>
        public static string? Login(string Url, LoginDataJson LoginData, bool ScreenOut = true)
        {
            HttpWebResponse? ReturnData = HttpRequest(Url + "/api/v3/user/session", httpMod: HttpMods.POST, data: LoginData.LoginReturnJson(), LogOut: false);//登入并获取服务器响应
            if (ReturnData == null)//如果是null那就是连服务器都访问不上
            {
                Logger.WriteError("登入Cloudreve失败!因为上面的原因...");//打印至日志
                if (ScreenOut)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("登入Cloudreve失败!因为上面的原因...");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                return null;
            }
            string? ReturnDataString = HttpReturnData.HttpWebDataToString(ReturnData, false);//返回内容
            Logger.WriteDebug(ReturnDataString);//打印至日志
            LoginReturnJson? LoginReturnJson = JsonSerializer.Deserialize<LoginReturnJson>(ReturnDataString);//返回内容Json序列化
            string? ReturnCookie = ReturnData?.Headers["Set-Cookie"];//返回Cookie

            if (ReturnCookie == null)//用Cookie判断是否有登入成功
            {
                Logger.WriteError("登入Cloudreve失败!因为:" + LoginReturnJson?.msg);//打印至日志
                if (ScreenOut)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("登入Cloudreve失败!因为:" + LoginReturnJson?.msg);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                return null;
            }
            Logger.WriteInfor("登入Cloudrevet成功! 组别:" + LoginReturnJson?.data?.group.name + " 用户名:" + LoginReturnJson?.data?.user_name + " 别名:" + LoginReturnJson?.data?.nickname);//打印至日志
            if (ScreenOut)
                Console.WriteLine("登入Cloudrevet成功! 组别:" + LoginReturnJson?.data?.group.name + " 用户名:" + LoginReturnJson?.data?.user_name + " 别名:" + LoginReturnJson?.data?.nickname);
            return ReturnCookie;
        }
        /// <summary>
        /// 使用者操作
        /// </summary>
        public class User
        {
            /// <summary>
            /// 上传文件至云盘
            /// </summary>
            /// <param name="Url">Cloudreve服务器地址</param>
            /// <param name="cookie">登入返还的Cookie</param>
            /// <param name="policy">Cloudreve的存储策略</param>
            /// <param name="FilesPath">本地文件路径</param>
            /// <param name="CloudFilesPath">云盘上传路径(默认根目录"/")</param>
            /// <param name="sessionID">任务ID 可替PUT请求 前提任务ID没有被清除</param>
            /// <param name="SliceSize">上传分片大小</param>
            /// <param name="Slice">上传任务分片</param>
            /// <param name="ScreenOut">屏幕显示输出</param>
            /// <returns>string 类型 上传状态</returns>
            public static string? UpFile(string Url, string? cookie, string policy, string FilesPath, string CloudFilesPath = "/", string? sessionID = null, int SliceSize = 0, int Slice = 0, bool ScreenOut = true)
            {
                string? HttpRequestToStringData = null;
                if (sessionID == null)
                {
                    PUT_UploadFilesDataJson Upload = new();
                    string? UploadJson = Upload.Updata(FilesPath, policy, CloudFilesPath);
                    string? UploadReturnJson = HttpRequestToString(Url + "/api/v3/file/upload/", cookie: cookie, httpMod: HttpMods.PUT, data: UploadJson);//发送上传请求
                    if (UploadReturnJson == null)//如果是null那就是连服务器都访问不上
                    {
                        Logger.WriteError("发送上传请求失败(PUT)!因为上面的原因...");//打印至日志
                        if (ScreenOut)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("发送上传请求失败(PUT)!因为上面的原因...");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        return null;
                    }
                    PUT_UploadFilesReturnJson? PUT_UploadFilesReturnJson = JsonSerializer.Deserialize<PUT_UploadFilesReturnJson>(UploadReturnJson);
                    if (PUT_UploadFilesReturnJson?.code != 0)
                    {
                        Logger.WriteError("上传失败(PUT)!因为:" + PUT_UploadFilesReturnJson?.msg + " #文件名:" + Path.GetFileName(FilesPath) + " 本地路径" + Path.GetDirectoryName(FilesPath) + " 云盘路径:" + CloudFilesPath);//打印至日志
                        if (ScreenOut)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("上传失败(PUT)!因为:" + PUT_UploadFilesReturnJson?.msg + " #文件名:" + Path.GetFileName(FilesPath) + " 本地路径" + Path.GetDirectoryName(FilesPath) + " 云盘路径:" + CloudFilesPath);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        return UploadJson;
                    }
                    sessionID = PUT_UploadFilesReturnJson.data?.sessionID;
                    double ExpectedSliceNumber = Math.Ceiling((double)(new FileInfo(FilesPath).Length / PUT_UploadFilesReturnJson.data?.chunkSize)!);
                    Logger.WriteInfor("发送上传请求成功(PUT)!ID:" + sessionID + " 预估分片数量:" + ExpectedSliceNumber);//打印至日志

                    List<byte[]> SliceCache = new();//文件分片缓存集合
                    byte[] bytes;//存储读取结果  
                    FileStream fs = new FileStream(FilesPath, FileMode.Open, FileAccess.Read);//打开文件
                    long left = fs.Length;//尚未读取的文件内容长度
                    long Remaining = 0;//分片末尾剩余
                    if (new FileInfo(FilesPath).Length < PUT_UploadFilesReturnJson.data?.chunkSize)//判断文件大小是否超过分片大小
                        bytes = new byte[new FileInfo(FilesPath).Length];
                    else
                    {
                        bytes = new byte[(int)PUT_UploadFilesReturnJson.data?.chunkSize!];
                        Remaining = fs.Length % (int)PUT_UploadFilesReturnJson?.data.chunkSize!;
                    }
                    int maxLength = bytes.Length;//每次读取长度  
                    int start = 0;//读取位置  
                    int num;//实际返回结果长度  
                    while (left > 0)//当文件未读取长度大于0时，不断进行读取  
                    {
                        fs.Position = start;
                        num = 0;
                        if (left < maxLength)//末尾分片
                        {
                            byte[] RemainingByte = new byte[Remaining];
                            num = fs.Read(RemainingByte, 0, Convert.ToInt32(left));
                            SliceCache.Add(RemainingByte);
                        }
                        else
                        {
                            num = fs.Read(bytes, 0, maxLength);
                            SliceCache.Add(bytes);
                        }
                        if (num == 0)
                            break;
                        start += num;
                        left -= num;
                    }
                    fs.Close();
                    for (int i = 0; i < SliceCache.Count; i++)
                    {
                        HttpRequestToStringData = HttpRequestToString(Url + "/api/v3/file/upload/" + sessionID + "/" + i, cookie: cookie, httpMod: HttpMods.POST_UPDATA, dataByte: SliceCache[i]);
                        Logger.WriteInfor("上传成功!(POST) 任务ID:" + sessionID + " 分片:" + i + " #文件名:" + Path.GetFileName(FilesPath) + " 本地路径" + Path.GetDirectoryName(FilesPath) + " 云盘路径:" + CloudFilesPath);
                    }
                }
                else
                {
                    HttpRequestToStringData = HttpRequestToString(Url + "/api/v3/file/upload/" + sessionID + "/" + Slice, cookie: cookie, httpMod: HttpMods.POST_UPDATA, data: FilesPath);
                }

                if (HttpRequestToStringData == null)//如果是null那就是连服务器都访问不上
                {
                    Logger.WriteError("发送上传请求失败(POST)!因为上面的原因...");//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("发送上传请求失败(POST)!因为上面的原因...");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return null;
                }
                POST_UploadFilesReturnJson? POST_UploadFilesReturnJson = JsonSerializer.Deserialize<POST_UploadFilesReturnJson>(HttpRequestToStringData);
                if (POST_UploadFilesReturnJson?.code != 0)
                {
                    Logger.WriteError("上传失败(POST)!因为:" + POST_UploadFilesReturnJson?.msg + " 任务ID:" + sessionID + " #文件名:" + Path.GetFileName(FilesPath) + " 本地路径" + Path.GetDirectoryName(FilesPath) + " 云盘路径:" + CloudFilesPath);//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("上传失败(POST)!因为:" + POST_UploadFilesReturnJson?.msg + " 任务ID:" + sessionID + " #文件名:" + Path.GetFileName(FilesPath) + " 本地路径" + Path.GetDirectoryName(FilesPath) + " 云盘路径:" + CloudFilesPath);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    if (POST_UploadFilesReturnJson?.code == 400011)//如果找不到任物就回传null
                    {
                        return null;
                    }
                    return sessionID;
                }
                Logger.WriteInfor("上传任务完成! #文件名:" + Path.GetFileName(FilesPath) + " 本地路径" + Path.GetDirectoryName(FilesPath) + " 云盘路径:" + CloudFilesPath);
                if (ScreenOut)
                    Console.WriteLine("上传任务完成! #文件名:" + Path.GetFileName(FilesPath) + " 本地路径" + Path.GetDirectoryName(FilesPath) + " 云盘路径:" + CloudFilesPath);
                return HttpRequestToStringData;
            }
            /// <summary>
            /// 删除上传文件列队
            /// </summary>
            /// <param name="Url">Cloudreve服务器地址</param>
            /// <param name="cookie">登入返还的Cookie</param>
            /// <param name="sessionID">任务ID</param>
            /// <param name="ScreenOut">屏幕显示输出</param>
            /// <returns>DeleteUpFileListReturnJson 类型 返回</returns>
            public static DeleteUpFileListReturnJson? DeleteUpFileList(string Url, string? cookie, string? sessionID = null, bool ScreenOut = true)
            {
                string? HttpRequestToStringData = HttpRequestToString(UrlSplice(), cookie, httpMod: HttpMods.DELETE);
                if (HttpRequestToStringData == null)//如果是null那就是连服务器都访问不上
                {
                    Logger.WriteError("发送上传请求失败(DELETE)!因为上面的原因...");//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("发送上传请求失败(DELETE)!因为上面的原因...");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return null;
                }
                DeleteUpFileListReturnJson? DeleteUpFileListReturnJson = JsonSerializer.Deserialize<DeleteUpFileListReturnJson>(HttpRequestToStringData);
                if (sessionID == null)
                    sessionID = "All";
                if (DeleteUpFileListReturnJson?.code != 0)
                {
                    Logger.WriteError("删除上传列队失败(DELETE)!因为:" + DeleteUpFileListReturnJson?.msg + " 任务ID:" + sessionID);//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("删除上传列队失败(DELETE)!因为:" + DeleteUpFileListReturnJson?.msg + " 任务ID:" + sessionID);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return DeleteUpFileListReturnJson;
                }
                Logger.WriteInfor("删除上传列队成功!" + " 任务ID:" + sessionID);
                if (ScreenOut)
                    Console.WriteLine("删除上传列队成功!" + " 任务ID:" + sessionID);
                return DeleteUpFileListReturnJson;

                string UrlSplice()//合成Url
                {
                    if (sessionID != null)
                    {
                        return Url + "/api/v3/file/upload/" + sessionID;
                    }
                    return Url + "/api/v3/file/upload/";
                }
            }
            /// <summary>
            /// 获取目录内容
            /// </summary>
            /// <param name="Url">Cloudreve服务器地址</param>
            /// <param name="cookie">登入返还的Cookie</param>
            /// <param name="Directory">云盘目录</param>
            /// <param name="ScreenOut">屏幕显示输出</param>
            /// <returns>DirectoryDataReturnJson 类型 返回内容</returns>
            public static DirectoryDataReturnJson? GetDirectory(string Url, string? cookie, string? Directory = null, bool ScreenOut = true)
            {
                string? HttpRequestToStringData = HttpRequestToString(Url + "/api/v3/directory/" + Directory, cookie, httpMod: HttpMods.GET);
                if (HttpRequestToStringData == null)//如果是null那就是连服务器都访问不上
                {
                    Logger.WriteError("发送获取目录内容失败(GET)!因为上面的原因...");//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("发送获取目录内容失败(GET)!因为上面的原因...");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return null;
                }
                DirectoryDataReturnJson? DirectoryDataReturnJson = JsonSerializer.Deserialize<DirectoryDataReturnJson>(HttpRequestToStringData);
                if (DirectoryDataReturnJson?.code != 0)
                {
                    Logger.WriteError("获取目录内容失败(GET)!因为:" + DirectoryDataReturnJson?.msg + " 目录:" + Directory);//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("获取目录内容失败(GET)!因为:" + DirectoryDataReturnJson?.msg + " 目录:" + Directory);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return DirectoryDataReturnJson;
                }
                Logger.WriteInfor("获取目录成功!" + " 目录:" + Directory);
                if (ScreenOut)
                    Console.WriteLine("获取目录成功!" + " 目录:" + Directory);
                return DirectoryDataReturnJson;
            }
            /// <summary>
            /// 获取文件下载路径
            /// </summary>
            /// <param name="Url">Cloudreve服务器地址</param>
            /// <param name="cookie">登入返还的Cookie</param>
            /// <param name="ID">文件ID</param>
            /// <param name="ScreenOut">屏幕显示输出</param>
            /// <returns>DirectoryDataReturnJson 类型 返回内容</returns>
            public static DownloadReturnJson? GetDownloadUrl(string Url, string? cookie, string ID, bool ScreenOut = true)
            {
                string? HttpRequestToStringData = HttpRequestToString(Url + "/api/v3/file/download/" + ID, cookie, httpMod: HttpMods.PUT);
                if (HttpRequestToStringData == null)//如果是null那就是连服务器都访问不上
                {
                    Logger.WriteError("发送获取下载文件URL失败(PUT)!因为上面的原因...");//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("发送获取下载文件URL失败(PUT)!因为上面的原因...");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return null;
                }
                DownloadReturnJson? DownloadReturnJson = JsonSerializer.Deserialize<DownloadReturnJson>(HttpRequestToStringData);
                if (DownloadReturnJson?.code != 0)
                {
                    Logger.WriteError("获取下载文件URL失败(PUT)!因为:" + DownloadReturnJson?.msg + " 文件ID:" + ID);//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("获取下载文件URL失败(PUT)!因为:" + DownloadReturnJson?.msg + " 文件ID:" + ID);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return DownloadReturnJson;
                }
                Logger.WriteInfor("获取下载文件URL成功!" + " 文件ID:" + ID);
                if (ScreenOut)
                    Console.WriteLine("获取下载文件URL成功!" + " 文件ID:" + ID);
                return DownloadReturnJson;
            }
            /// <summary>
            /// 获取文件外链
            /// </summary>
            /// <param name="Url">Cloudreve服务器地址</param>
            /// <param name="cookie">登入返还的Cookie</param>
            /// <param name="ID">文件ID</param>
            /// <param name="ScreenOut">屏幕显示输出</param>
            /// <returns>FileSourceDataReturnJson 类型 返回内容</returns>
            public static FileSourceDataReturnJson? GetFileSource(string Url, string? cookie, List<string> ID, bool ScreenOut = true)
            {
                string? HttpRequestToStringData = HttpRequestToString(Url + "/api/v3/file/source", cookie, data: FileSourceDataJson.FileSourceDataReturnJson(ID), httpMod: HttpMods.POST);
                if (HttpRequestToStringData == null)//如果是null那就是连服务器都访问不上
                {
                    Logger.WriteError("发送获取文件外链失败(POST)!因为上面的原因...");//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("发送获取文件外链失败(POST)!因为上面的原因...");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return null;
                }
                FileSourceDataReturnJson? FileSourceDataReturnJson = JsonSerializer.Deserialize<FileSourceDataReturnJson>(HttpRequestToStringData);
                string? ReturnString = null;
                if (ID.Count == 0)//判断传入ID是否为空,诺是为空则添加NULL进去
                    ID.Add("null");
                ReturnString = string.Join(",", ID.ToArray());//组合字符串
                if (FileSourceDataReturnJson?.code != 0 || FileSourceDataReturnJson?.data?.Count != ID.Count)
                {
                    Logger.WriteError("获取文件外链失败(POST)!因为:" + FileSourceDataReturnJson?.msg + " 文件ID:" + ReturnString);//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("获取文件外链失败(POST)!因为:" + FileSourceDataReturnJson?.msg + " 文件ID:" + ReturnString);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return FileSourceDataReturnJson;
                }
                Logger.WriteInfor("获取文件外链成功!" + " 文件ID:" + ReturnString);
                if (ScreenOut)
                    Console.WriteLine("获取文件外链成功!" + " 文件ID:" + ReturnString);
                return FileSourceDataReturnJson;
            }//同时生成多个无法准确判断具体哪个没有正确生成
            /// <summary>
            /// 获取文件分享链接
            /// </summary>
            /// <param name="Url">Cloudreve服务器地址</param>
            /// <param name="cookie">登入返还的Cookie</param>
            /// <param name="ID">文件ID</param>
            /// <param name="is_dir">是否为文件夹</param>
            /// <param name="password">密码</param>
            /// <param name="downloads">下载多少次过期</param>
            /// <param name="expire">过期时间</param>
            /// <param name="preview">是否允许预览</param>
            /// <param name="ScreenOut">屏幕显示输出</param>
            /// <returns>FileSourceDataReturnJson 类型 返回内容</returns>
            public static FileShareDataReturnJson? GetFileShare(string Url, string? cookie, string ID, bool is_dir = false, string? password = null, int downloads = -1, int expire = 86400, bool preview = true, bool ScreenOut = true)
            {
                string? HttpRequestToStringData = HttpRequestToString(Url + "/api/v3/share", cookie, data: FileShareDataJson.FileShareDataReturnJson(ID, is_dir, password, downloads, expire, preview), httpMod: HttpMods.POST);
                if (HttpRequestToStringData == null)//如果是null那就是连服务器都访问不上
                {
                    Logger.WriteError("发送获取文件分享链接失败(POST)!因为上面的原因...");//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("发送获取文件分享链接失败(POST)!因为上面的原因...");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return null;
                }
                FileShareDataReturnJson? FileShareDataReturnJson = JsonSerializer.Deserialize<FileShareDataReturnJson>(HttpRequestToStringData);
                if (FileShareDataReturnJson?.code != 0)
                {
                    Logger.WriteError("获取文件分享链接失败(POST)!因为:" + FileShareDataReturnJson?.msg);//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("获取文件分享链接失败(POST)!因为:" + FileShareDataReturnJson?.msg);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return FileShareDataReturnJson;
                }
                Logger.WriteInfor("获取文件分享链接成功! 地址:" + FileShareDataReturnJson.data);
                if (ScreenOut)
                    Console.WriteLine("获取文件分享链接成功! 地址:" + FileShareDataReturnJson.data);
                return FileShareDataReturnJson;
            }
            /// <summary>
            /// 查询文件分享链接列表
            /// </summary>
            /// <param name="Url">Cloudreve服务器地址</param>
            /// <param name="cookie">登入返还的Cookie</param>
            /// <param name="page">页数</param>
            /// <param name="Sort">排序方式</param>
            /// <param name="ScreenOut">屏幕显示输出</param>
            /// <returns>FileShareShowreturnJson 类型 返回内容</returns>
            public static FileShareShowreturnJson? GetFileShareShow(string Url, string? cookie, int page = 1, Sort Sort = Sort.CreationDateFromLateToEarly, bool ScreenOut = true)
            {
                string? UrlData = null;
                switch (Sort.ToString())
                {
                    case "CreationDateFromLateToEarly":
                        UrlData = "&order_by=created_at&order=DESC";
                        break;
                    case "CreationDateFromEarlyToLate":
                        UrlData = "&order_by=created_at&order=ASC";
                        break;
                    case "DownloadsFromLargestToSmallest":
                        UrlData = "&order_by=downloads&order=DESC";
                        break;
                    case "DownloadTimesFromSmallToLarge":
                        UrlData = "&order_by=downloads&order=ASC";
                        break;
                    case "NumberOfViewsFromLargeToSmall":
                        UrlData = "&order_by=views&order=DESC";
                        break;
                    case "NumberOfViewsFromSmallToLarge":
                        UrlData = "&order_by = views & order = ASC";
                        break;
                }
                string? HttpRequestToStringData = HttpRequestToString(Url + "/api/v3/share?page=" + page + UrlData, cookie, httpMod: HttpMods.GET);
                if (HttpRequestToStringData == null)//如果是null那就是连服务器都访问不上
                {
                    Logger.WriteError("发送查询文件分享链接列表失败(GET)!因为上面的原因...");//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("发送查询文件分享链接列表失败(GET)!因为上面的原因...");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return null;
                }
                FileShareShowreturnJson? FileShareShowreturnJson = JsonSerializer.Deserialize<FileShareShowreturnJson>(HttpRequestToStringData);
                if (FileShareShowreturnJson?.code != 0)
                {
                    Logger.WriteError("查询文件分享链接列表失败(GET)!因为:" + FileShareShowreturnJson?.msg);//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("查询文件分享链接列表失败(GET)!因为:" + FileShareShowreturnJson?.msg);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return FileShareShowreturnJson;
                }
                Logger.WriteInfor("查询文件分享链接列表成功! 分享链接总数量:" + FileShareShowreturnJson.data?.total);
                if (ScreenOut)
                    Console.WriteLine("查询文件分享链接列表成功! 分享链接总数量:" + FileShareShowreturnJson.data?.total);
                return FileShareShowreturnJson;
            }
            /// <summary>
            /// 设置文件分享链接
            /// </summary>
            /// <param name="Url">Cloudreve服务器地址</param>
            /// <param name="cookie">登入返还的Cookie</param>
            /// <param name="Key">分享文件ID</param>
            /// <param name="Settings">设置选项</param>
            /// <param name="value">设置值</param>
            /// <param name="ScreenOut">屏幕显示输出</param>
            /// <returns>FileShareShowreturnJson 类型 返回内容</returns>
            public static SettingsReturnJson? SetFileShare(string Url, string? cookie, string Key, Settings Settings, string value, bool ScreenOut = true)
            {
                string? HttpRequestToStringData = HttpRequestToString(Url + "/api/v3/share/" + Key, cookie, data: SettingsJson.SettingsDataReturnJson(Settings.ToString(), value), httpMod: HttpMods.PATCH);
                if (HttpRequestToStringData == null)//如果是null那就是连服务器都访问不上
                {
                    Logger.WriteError("发送设置文件分享链接失败(PATCH)!因为上面的原因...");//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("发送设置文件分享链接失败(PATCH)!因为上面的原因...");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return null;
                }
                SettingsReturnJson? SettingsReturnJson = JsonSerializer.Deserialize<SettingsReturnJson>(HttpRequestToStringData);
                if (SettingsReturnJson?.code != 0)
                {
                    Logger.WriteError("设置文件分享链接失败(PATCH)!因为:" + SettingsReturnJson?.msg + " Key:" + Key);//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("设置文件分享链接失败(PATCH)!因为:" + SettingsReturnJson?.msg + " Key:" + Key);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return SettingsReturnJson;
                }
                Logger.WriteInfor("设置文件分享链接成功! Key:" + Key + " Data:" + SettingsReturnJson.data);
                if (ScreenOut)
                    Console.WriteLine("设置文件分享链接成功! Key:" + Key + " Data:" + SettingsReturnJson.data);
                return SettingsReturnJson;
            }
            /// <summary>
            /// 删除文件分享链接
            /// </summary>
            /// <param name="Url">Cloudreve服务器地址</param>
            /// <param name="cookie">登入返还的Cookie</param>
            /// <param name="Key">分享文件ID</param>
            /// <param name="Settings">设置选项</param>
            /// <param name="value">设置值</param>
            /// <param name="ScreenOut">屏幕显示输出</param>
            /// <returns>FileShareShowreturnJson 类型 返回内容</returns>
            public static DeltetShareReturnJson? DeltetFileShare(string Url, string? cookie, string Key, bool ScreenOut = true)
            {
                string? HttpRequestToStringData = HttpRequestToString(Url + "/api/v3/share/" + Key, cookie, httpMod: HttpMods.DELETE);
                if (HttpRequestToStringData == null)//如果是null那就是连服务器都访问不上
                {
                    Logger.WriteError("发送删除文件分享链接失败(DELETE)!因为上面的原因...");//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("发送删除文件分享链接失败(DELETE)!因为上面的原因...");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return null;
                }
                DeltetShareReturnJson? DeltetShareReturnJson = JsonSerializer.Deserialize<DeltetShareReturnJson>(HttpRequestToStringData);
                if (DeltetShareReturnJson?.code != 0)
                {
                    Logger.WriteError("删除文件分享链接失败(DELETE)!因为:" + DeltetShareReturnJson?.msg + " Key:" + Key);//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("删除文件分享链接失败(DELETE)!因为:" + DeltetShareReturnJson?.msg + " Key:" + Key);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return DeltetShareReturnJson;
                }
                Logger.WriteInfor("删除文件分享链接成功! Key:" + Key);
                if (ScreenOut)
                    Console.WriteLine("删除文件分享链接成功! Key:" + Key);
                return DeltetShareReturnJson;
            }
            /// <summary>
            /// 获取Config
            /// </summary>
            /// <param name="Url">Cloudreve服务器地址</param>
            /// <param name="cookie">登入返还的Cookie</param>
            /// <param name="ScreenOut">屏幕显示输出</param>
            /// <returns>ConfigReturnJson 类型 返回内容</returns>
            public static ConfigReturnJson? GetConfig(string Url, string? cookie, bool ScreenOut = true)
            {
                string? HttpRequestToStringData = HttpRequestToString(Url + "/api/v3/site/config", cookie, httpMod: HttpMods.GET);
                if (HttpRequestToStringData == null)//如果是null那就是连服务器都访问不上
                {
                    Logger.WriteError("发送获取Config失败(GET)!因为上面的原因...");//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("发送获取Config失败(GET)!因为上面的原因...");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return null;
                }
                ConfigReturnJson? ConfigReturnJson = JsonSerializer.Deserialize<ConfigReturnJson>(HttpRequestToStringData);
                if (ConfigReturnJson?.code != 0)
                {
                    Logger.WriteError("获取Config失败(GET)!因为:" + ConfigReturnJson?.msg);//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("获取Config失败(GET)!因为:" + ConfigReturnJson?.msg);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return ConfigReturnJson;
                }
                Logger.WriteInfor("获取Config成功! 组别:" + ConfigReturnJson.data?.user.group.name + " 用户名:" + ConfigReturnJson.data?.user.user_name + " 别名:" + ConfigReturnJson.data?.user.nickname);
                if (ScreenOut)
                    Console.WriteLine("获取Config成功! 组别:" + ConfigReturnJson.data?.user.group.name + " 用户名:" + ConfigReturnJson.data?.user.user_name + " 别名:" + ConfigReturnJson.data?.user.nickname);
                return ConfigReturnJson;
            }
            /// <summary>
            /// 删除文件
            /// </summary>
            /// <param name="Url">Cloudreve服务器地址</param>
            /// <param name="cookie">登入返还的Cookie</param>
            /// <param name="FilesID">要删除的文件ID</param>
            /// <param name="DirsID">要删除的文件夹ID</param>
            /// <param name="ScreenOut">屏幕显示输出</param>
            /// <returns>DeleteFilesDataReturnJson 类型 返回内容</returns>
            public static DeleteFilesDataReturnJson? DeleteFiles(string Url, string? cookie, List<string>? FilesID = null, List<string>? DirsID = null, bool ScreenOut = true)
            {
                string? HttpRequestToStringData = HttpRequestToString(Url + "/api/v3/object", cookie, data: DeleteFilesDataJson.DeleteFilesDataReturnJson(new() { items = FilesID, dirs = FilesID }), httpMod: HttpMods.DELETE);
                if (HttpRequestToStringData == null)//如果是null那就是连服务器都访问不上
                {
                    Logger.WriteError("发送删除文件请求失败(DELETE)!因为上面的原因...");//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("发送删除文件请求失败(DELETE)!因为上面的原因...");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return null;
                }
                DeleteFilesDataReturnJson? DeleteFilesDataReturnJson = JsonSerializer.Deserialize<DeleteFilesDataReturnJson>(HttpRequestToStringData);
                if (DeleteFilesDataReturnJson?.code != 0)
                {
                    Logger.WriteError("删除文件失败(DELETE)!因为:" + DeleteFilesDataReturnJson?.msg);//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("删除文件失败(DELETE)!因为:" + DeleteFilesDataReturnJson?.msg);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return DeleteFilesDataReturnJson;
                }
                Logger.WriteInfor("删除文件成功!");
                if (ScreenOut)
                    Console.WriteLine("删除文件成功!");
                return DeleteFilesDataReturnJson;
            }
            /// <summary>
            /// 获取文件详细信息
            /// </summary>
            /// <param name="Url">Cloudreve服务器地址</param>
            /// <param name="cookie">登入返还的Cookie</param>
            /// <param name="FilesID">要查询的文件ID</param>
            /// <param name="DirsID">要查询的文件夹ID</param>
            /// <param name="trace_root">文件路径跟踪</param>
            /// <param name="ScreenOut">屏幕显示输出</param>
            /// <returns>FilesDataReturnJson 类型 返回内容</returns>
            public static FilesDataReturnJson? GetFilesData(string Url, string? cookie, string? FilesID = null, string? DirsID = null, bool trace_root = true, bool ScreenOut = true)
            {
                string? HttpRequestToStringData = HttpRequestToString(UrlSynthesis(), cookie, httpMod: HttpMods.GET);
                if (HttpRequestToStringData == null)//如果是null那就是连服务器都访问不上
                {
                    Logger.WriteError("发送获取文件信息请求失败(GET)!因为上面的原因...");//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("发送获取文件信息请求失败(GET)!因为上面的原因...");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return null;
                }
                FilesDataReturnJson? FilesDataReturnJson = JsonSerializer.Deserialize<FilesDataReturnJson>(HttpRequestToStringData);
                if (FilesDataReturnJson?.code != 0)
                {
                    Logger.WriteError("获取文件信息失败(GET)!因为:" + FilesDataReturnJson?.msg + " ID:" + new string(DirsID ?? FilesID));//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("获取文件信息失败(GET)!因为:" + FilesDataReturnJson?.msg + " ID:" + new string(DirsID ?? FilesID));
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return FilesDataReturnJson;
                }
                Logger.WriteInfor("获取文件信息成功! ID:" + new string(DirsID ?? FilesID));
                if (ScreenOut)
                    Console.WriteLine("获取文件信息成功! ID:" + new string(DirsID ?? FilesID));
                return FilesDataReturnJson;

                string UrlSynthesis()//Url合成
                {
                    if (FilesID != null && DirsID == null)
                    {
                        return Url += "/api/v3/object/property/" + FilesID + "?is_folder=false" + "&trace_root=" + trace_root;
                    }
                    if (FilesID == null && DirsID != null)
                    {
                        return Url += "/api/v3/object/property/" + DirsID + "?is_folder=true" + "&trace_root=" + trace_root;
                    }
                    return "啥?你全都要??";
                }
            }
            /// <summary>
            /// 获取云盘容量
            /// </summary>
            /// <param name="Url">Cloudreve服务器地址</param>
            /// <param name="cookie">登入返还的Cookie</param>
            /// <param name="ScreenOut">屏幕显示输出</param>
            /// <returns>CloudDriveSizeReturnJson 类型 返回内容</returns>
            public static CloudDriveSizeReturnJson? GetCloudDriveSize(string Url, string? cookie, bool ScreenOut = true)
            {
                string? HttpRequestToStringData = HttpRequestToString(Url + "/api/v3/user/storage", cookie, httpMod: HttpMods.GET);
                if (HttpRequestToStringData == null)//如果是null那就是连服务器都访问不上
                {
                    Logger.WriteError("发送获取云盘容量请求失败(GET)!因为上面的原因...");//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("发送获取云盘容量请求失败(GET)!因为上面的原因...");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return null;
                }
                CloudDriveSizeReturnJson? CloudDriveSizeReturnJson = JsonSerializer.Deserialize<CloudDriveSizeReturnJson>(HttpRequestToStringData);
                if (CloudDriveSizeReturnJson?.code != 0)
                {
                    Logger.WriteError("获取云盘容量失败(GET)!因为:" + CloudDriveSizeReturnJson?.msg);//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("获取云盘容量失败(GET)!因为:" + CloudDriveSizeReturnJson?.msg);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return CloudDriveSizeReturnJson;
                }
                Logger.WriteInfor("获取云盘容量成功! 总容量:" + SizeConversion.AutoSizeConversion(CloudDriveSizeReturnJson.data?.total) + " 已使用:" + SizeConversion.AutoSizeConversion(CloudDriveSizeReturnJson.data?.used));
                if (ScreenOut)
                    Console.WriteLine("获取云盘容量成功! 总容量:" + SizeConversion.AutoSizeConversion(CloudDriveSizeReturnJson.data?.total) + " 已使用:" + SizeConversion.AutoSizeConversion(CloudDriveSizeReturnJson.data?.used));
                return CloudDriveSizeReturnJson;
            }
            /// <summary>
            /// 搜索文件
            /// </summary>
            /// <param name="Url">Cloudreve服务器地址</param>
            /// <param name="cookie">登入返还的Cookie</param>
            /// <param name="name">要搜索的文件名</param>
            /// <param name="CloudFilesPath">搜索的目录</param>
            /// <param name="ScreenOut">屏幕显示输出</param>
            /// <returns>FileSearchReturnJson 类型 返回内容</returns>
            public static FileSearchReturnJson? FileSearch(string Url, string? cookie, string name, string CloudFilesPath = "", bool ScreenOut = true)
            {
                string? HttpRequestToStringData = HttpRequestToString(Url + "/api/v3/file/search/keywords%2F" + name + "?path=%2F" + CloudFilesPath, cookie, httpMod: HttpMods.GET);
                if (HttpRequestToStringData == null)//如果是null那就是连服务器都访问不上
                {
                    Logger.WriteError("发送搜索文件请求失败(GET)!因为上面的原因...");//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("发送搜索文件请求失败(GET)!因为上面的原因...");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return null;
                }
                FileSearchReturnJson? FileSearchReturnJson = JsonSerializer.Deserialize<FileSearchReturnJson>(HttpRequestToStringData);
                if (FileSearchReturnJson?.code != 0)
                {
                    Logger.WriteError("搜索文件失败(GET)!因为:" + FileSearchReturnJson?.msg + " 关键字:" + name + " 搜索路径:" + CloudFilesPath);//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("搜索文件失败(GET)!因为:" + FileSearchReturnJson?.msg + " 关键字:" + name + " 搜索路径:" + CloudFilesPath);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return FileSearchReturnJson;
                }
                Logger.WriteInfor("搜索文件成功! 关键字:" + name + " 搜索路径:" + CloudFilesPath + " 共有" + FileSearchReturnJson.data?.objects?.Count + "个结果");
                if (ScreenOut)
                    Console.WriteLine("搜索文件成功! 关键字:" + name + " 搜索路径:" + CloudFilesPath + " 共有" + FileSearchReturnJson.data?.objects?.Count + "个结果");
                return FileSearchReturnJson;
            }
            /// <summary>
            /// 搜索分享链接
            /// </summary>
            /// <param name="Url">Cloudreve服务器地址</param>
            /// <param name="cookie">登入返还的Cookie</param>
            /// <param name="name">要搜索的文件名</param>
            /// <param name="page">页数</param>
            /// <param name="Sort">排序方式</param>
            /// <param name="ScreenOut">屏幕显示输出</param>
            /// <returns>ShareSearchReturnJson 类型 返回内容</returns>
            public static ShareSearchReturnJson? ShareSearch(string Url, string? cookie, string name, int page = 1, Sort Sort = Sort.CreationDateFromLateToEarly, bool ScreenOut = true)
            {
                string? UrlData = null;
                switch (Sort.ToString())
                {
                    case "CreationDateFromLateToEarly":
                        UrlData = "&order_by=created_at&order=DESC";
                        break;
                    case "CreationDateFromEarlyToLate":
                        UrlData = "&order_by=created_at&order=ASC";
                        break;
                    case "DownloadsFromLargestToSmallest":
                        UrlData = "&order_by=downloads&order=DESC";
                        break;
                    case "DownloadTimesFromSmallToLarge":
                        UrlData = "&order_by=downloads&order=ASC";
                        break;
                    case "NumberOfViewsFromLargeToSmall":
                        UrlData = "&order_by=views&order=DESC";
                        break;
                    case "NumberOfViewsFromSmallToLarge":
                        UrlData = "&order_by = views & order = ASC";
                        break;
                }
                string? HttpRequestToStringData = HttpRequestToString(Url + "/api/v3/share/search?page=" + page + UrlData + "&keywords=" + name, cookie, httpMod: HttpMods.GET);
                if (HttpRequestToStringData == null)//如果是null那就是连服务器都访问不上
                {
                    Logger.WriteError("发送搜索分享链接请求失败(GET)!因为上面的原因...");//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("发送搜索分享链接请求失败(GET)!因为上面的原因...");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return null;
                }
                ShareSearchReturnJson? ShareSearchReturnJson = JsonSerializer.Deserialize<ShareSearchReturnJson>(HttpRequestToStringData);
                if (ShareSearchReturnJson?.code != 0)
                {
                    Logger.WriteError("搜索文件失败(GET)!因为:" + ShareSearchReturnJson?.msg + " 关键字:" + name);//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("搜索文件失败(GET)!因为:" + ShareSearchReturnJson?.msg + " 关键字:" + name);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return ShareSearchReturnJson;
                }
                Logger.WriteInfor("搜索文件成功! 关键字:" + name + " 共有" + ShareSearchReturnJson.data?.total + "个结果");
                if (ScreenOut)
                    Console.WriteLine("搜索文件成功! 关键字:" + name + " 共有" + ShareSearchReturnJson.data?.total + "个结果");
                return ShareSearchReturnJson;
            }
        }
        /// <summary>
        /// 管理员操作
        /// </summary>
        public class Admin : User
        {
            /// <summary>
            /// 获取用户组定义列表
            /// </summary>
            /// <param name="Url">Cloudreve服务器地址</param>
            /// <param name="cookie">登入返还的Cookie</param>
            /// <param name="page">页数</param>
            /// <param name="page_size">一页所展示的数量</param>
            /// <param name="Sort">排序方式</param>
            /// <param name="ScreenOut">屏幕显示输出</param>
            /// <returns>GroupsListDataReturnJson 类型 返回内容</returns>
            public static GroupsListDataReturnJson? GetGroupsList(string Url, string? cookie, int page = 1, int page_size = 999, GroupsListJson.Sort Sort = GroupsListJson.Sort.id_desc, bool ScreenOut = true)
            {
                string UrlData = "";
                switch (Sort.ToString())
                {
                    case "id_desc":
                        UrlData = "id desc";
                        break;
                    case "id_asc":
                        UrlData = "id asc";
                        break;
                }
                string? HttpRequestToStringData = HttpRequestToString(Url + "/api/v3/admin/group/list", cookie, data: GroupsListDataJson.GroupsListDataReturnJson(page, page_size, UrlData), httpMod: HttpMods.POST);
                if (HttpRequestToStringData == null)//如果是null那就是连服务器都访问不上
                {
                    Logger.WriteError("发送获取用户组定义列表请求失败(POST)!因为上面的原因...");//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("发送获取用户组定义列表请求失败(POST)!因为上面的原因...");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return null;
                }
                GroupsListDataReturnJson? GroupsListDataReturnJson = JsonSerializer.Deserialize<GroupsListDataReturnJson>(HttpRequestToStringData);
                if (GroupsListDataReturnJson?.code != 0)
                {
                    Logger.WriteError("获取用户组定义列表失败(POST)!因为:" + GroupsListDataReturnJson?.msg);//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("获取用户组定义列表失败(POST)!因为:" + GroupsListDataReturnJson?.msg);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return GroupsListDataReturnJson;
                }
                Logger.WriteInfor("获取用户组定义列表成功! 共有" + GroupsListDataReturnJson?.data?.total + "个用户组");
                if (ScreenOut)
                    Console.WriteLine("获取用户组定义列表成功! 共有" + GroupsListDataReturnJson?.data?.total + "个用户组");
                return GroupsListDataReturnJson;
            }//尚未完成Json表定义 Policiesr及Statics
            /// <summary>
            /// 获取用户列表
            /// </summary>
            /// <param name="Url">Cloudreve服务器地址</param>
            /// <param name="cookie">登入返还的Cookie</param>
            /// <param name="ScreenOut">屏幕显示输出</param>
            /// <returns>GroupsReturnJson 类型 返回内容</returns>
            public static GroupsReturnJson? GetGroups(string Url, string? cookie, bool ScreenOut = true)
            {
                string? HttpRequestToStringData = HttpRequestToString(Url + "/api/v3/admin/groups", cookie, httpMod: HttpMods.GET);
                if (HttpRequestToStringData == null)//如果是null那就是连服务器都访问不上
                {
                    Logger.WriteError("发送获取用户组请求失败(GET)!因为上面的原因...");//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("发送获取用户组请求失败(GET)!因为上面的原因...");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return null;
                }
                GroupsReturnJson? GroupsReturnJson = JsonSerializer.Deserialize<GroupsReturnJson>(HttpRequestToStringData);
                if (GroupsReturnJson?.code != 0)
                {
                    Logger.WriteError("获取用户组失败(GET)!因为:" + GroupsReturnJson?.msg);//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("获取用户组失败(GET)!因为:" + GroupsReturnJson?.msg);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return GroupsReturnJson;
                }
                Logger.WriteInfor("获取用户组成功! 共有" + GroupsReturnJson?.data?.Count + "个用户组");
                if (ScreenOut)
                    Console.WriteLine("获取用户组成功! 共有" + GroupsReturnJson?.data?.Count + "个用户组");
                return GroupsReturnJson;
            }
            /// <summary>
            /// 获取用户列表
            /// </summary>
            /// <param name="Url">Cloudreve服务器地址</param>
            /// <param name="cookie">登入返还的Cookie</param>
            /// <param name="page">页数</param>
            /// <param name="page_size">一页所展示的数量</param>
            /// <param name="Sort">排序方式</param>
            /// <param name="ScreenOut">屏幕显示输出</param>
            /// <returns>GroupsListDataReturnJson 类型 返回内容</returns>
            public static UserListDatareturnJson? GetUserList(string Url, string? cookie, int page = 1, int page_size = 999, UserListJson.Sort Sort = UserListJson.Sort.id_desc, bool ScreenOut = true)
            {
                string UrlData = "";
                switch (Sort.ToString())
                {
                    case "id_desc":
                        UrlData = "id desc";
                        break;
                    case "id_asc":
                        UrlData = "id asc";
                        break;
                }
                string? HttpRequestToStringData = HttpRequestToString(Url + "/api/v3/admin/user/list", cookie, data: UserListDataJson.UserListDataReturnJson(page, page_size, UrlData), httpMod: HttpMods.POST);
                if (HttpRequestToStringData == null)//如果是null那就是连服务器都访问不上
                {
                    Logger.WriteError("发送获取用户列表请求失败(POST)!因为上面的原因...");//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("发送获取用户列表请求失败(POST)!因为上面的原因...");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return null;
                }
                UserListDatareturnJson? UserListDatareturnJson = JsonSerializer.Deserialize<UserListDatareturnJson>(HttpRequestToStringData);
                if (UserListDatareturnJson?.code != 0)
                {
                    Logger.WriteError("获取用户列表失败(POST)!因为:" + UserListDatareturnJson?.msg);//打印至日志
                    if (ScreenOut)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("获取用户列表失败(POST)!因为:" + UserListDatareturnJson?.msg);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    return UserListDatareturnJson;
                }
                Logger.WriteInfor("获取用户列表成功! 共有" + UserListDatareturnJson?.data?.total + "个用户");
                if (ScreenOut)
                    Console.WriteLine("获取用户列表成功! 共有" + UserListDatareturnJson?.data?.total + "个用户");
                return UserListDatareturnJson;
            }//Json表定义有歧义
        }

        /// <summary>
        /// 排序方式
        /// </summary>
        public enum Sort
        {
            /// <summary>
            /// 创建日期由晚到早
            /// </summary>
            [Description("&order_by=created_at&order=DESC")]
            CreationDateFromLateToEarly,
            /// <summary>
            /// 创建日期由早到晚
            /// </summary>
            [Description("&order_by=created_at&order=ASC")]
            CreationDateFromEarlyToLate,
            /// <summary>
            /// 下载次数由大到小
            /// </summary>
            [Description("&order_by=downloads&order=DESC")]
            DownloadsFromLargestToSmallest,
            /// <summary>
            /// 下载次数由小到大
            /// </summary>
            [Description("&order_by=downloads&order=ASC")]
            DownloadTimesFromSmallToLarge,
            /// <summary>
            /// 下载次数由大到小
            /// </summary>
            [Description("&order_by=views&order=DESC")]
            NumberOfViewsFromLargeToSmall,
            /// <summary>
            /// 浏览次数由小到大
            /// </summary>
            [Description("&order_by=views&order=ASC")]
            NumberOfViewsFromSmallToLarge
        }

        /// <summary>
        /// 用户组(已登入用户并非"用户组")
        /// </summary>
        public class UserGroup
        {
            public List<UserData> Users { get; } = new();
            public string CookiePath { get; set; }
            public string Url { get; set; }
            public bool ScreenOut { get; set; }
            public string? UUID { get; }
            /// <summary>
            /// 初始化类时读取已登入用户信息
            /// </summary>
            /// <param name="CookiePath">本地Cookie保存位置</param>
            /// <param name="Url">服务器地址</param>
            /// <param name="ScreenOut">屏幕显示输出</param>
            public UserGroup(string CookiePath, string Url, bool ScreenOut = true)
            {
                Logger.WriteDebug("初始化Cloudreve用户组");
                this.CookiePath = CookiePath;
                this.Url = Url;
                this.ScreenOut = ScreenOut;
                try
                {
                    string KeyPath = CookiePath + "/KEY";//KEY位置;
                    InitializationEncrypt(KeyPath, "44578287");//初始化KEY
                    this.UUID = DecryptEncrypt(KeyPath, "44578287")!;//解密KEY 获取UUID
                    if (!Directory.Exists(CookiePath))//如果不存在就创建file文件夹　　             　　              
                        Directory.CreateDirectory(CookiePath);//创建该文件夹

                    DirectoryInfo CookiePaths = new DirectoryInfo(CookiePath);
                    DirectoryInfo[] DirPaths = CookiePaths.GetDirectories();
                    foreach (var CookieArray in DirPaths)
                    {
                        string? Cookie = DecryptUserCookie(CookiePath, CookieArray.Name, UUID);//获取用户Cookie
                        int? code = User.GetCloudDriveSize(Url, Cookie, ScreenOut: false)?.code;
                        if (code == 0)
                        {
                            Users.Add
                                (
                                new()
                                {
                                    Account = CookieArray.Name,
                                    Cookie = Cookie,
                                }
                                );
                            continue;
                        }
                        throw new InvalidOperationException("因为上面的错误");
                    }
                }
                catch
                {
                    Logger.WriteError($"初始化用户组失败!路径:{Path.GetFullPath(CookiePath)}");
                    return;
                }
                Logger.WriteInfor($"初始化用户组成功!路径:{Path.GetFullPath(CookiePath)} 已登入本地用户数量:{Users.Count}");
            }
            /// <summary>
            /// 用户组用户登入
            /// </summary>
            /// <param name="LoginData">登入信息</param>
            public void Login(LoginDataJson LoginData)
            {
                Logger.WriteDebug("用户组 用户登入(UserGroup-Login)");
                int LoginIndex = UserReturnIndex(LoginData.UserName!);
                if (LoginIndex == -1 || !(Directory.GetFiles($"{CookiePath}/{LoginData.UserName}").Length > 0))
                {
                    string? cookie = CloudreveAPI.Login(Url, LoginData, ScreenOut);
                    if (cookie == null) //登入失败处理
                    {
                        return;
                    }
                    InitializationUser(CookiePath, LoginData.UserName!, cookie!, this.UUID!, Forced: true);//保存用户Cookie
                    Users.Add(
                        new()
                        {
                            Account = LoginData.UserName,
                            Cookie = cookie,
                        }
                        );
                    return;
                }
                //int LoginIndex = UserReturnIndex(LoginData.UserName!);
                if (Users[LoginIndex].Cookie == null)//Cookie过期
                {
                    string? cookie = CloudreveAPI.Login(Url, LoginData, ScreenOut);
                    if (cookie == null) //登入失败处理
                    {
                        return;
                    }
                    InitializationUser(CookiePath, LoginData.UserName!, cookie!, this.UUID!);//保存用户Cookie
                    Users[LoginIndex].Cookie = cookie;
                }
            }
            /// <summary>
            /// 搜索已登入用户
            /// </summary>
            /// <param name="Account">用户名</param>
            /// <returns>UserData 类型 返回内容 用户资料</returns>
            public UserData? UserReturn(string Account)
            {
                Logger.WriteDebug($"搜索本地登入用户:{Account}");
                int index = Users.FindIndex(x => x.Account == Account);
                if (index != -1)
                {
                    Logger.WriteInfor($"搜索本地登入用户:{Account}成功! index:{index}");
                    return Users[index];
                }
                Logger.WriteInfor($"搜索本地登入用户:{Account} 未有登入记录!");
                return null;
            }
            /// <summary>
            /// 搜索已登入用户
            /// </summary>
            /// <param name="Account">用户名</param>
            /// <returns>int 类型 返回内容 用户列表index</returns>
            public int UserReturnIndex(string Account)
            {
                Logger.WriteDebug($"搜索本地登入用户:{Account}");
                int index = Users.FindIndex(x => x.Account == Account);
                if (index != -1)
                {
                    Logger.WriteInfor($"搜索本地登入用户:{Account}成功! index:{index}");
                    return index;
                }
                Logger.WriteInfor($"搜索本地登入用户:{Account} 未有登入记录!");
                return -1;
            }


            /*/// <summary>
            /// 上传文件至云盘
            /// </summary>
            /// <param name="UsersIndex">用户本地列表index</param>
            /// <param name="policy">Cloudreve的存储策略</param>
            /// <param name="FilesPath">本地文件路径</param>
            /// <param name="CloudFilesPath">云盘上传路径(默认根目录"/")</param>
            /// <param name="sessionID">任务ID 可替PUT请求 前提任务ID没有被清除</param>
            /// <param name="SliceSize">上传分片大小</param>
            /// <param name="Slice">上传任务分片</param>
            /// <param name="ScreenOut">屏幕显示输出</param>
            /// <returns>string 类型 上传状态</returns>
            public string? UpFile(int UsersIndex, string policy, string FilesPath, string CloudFilesPath = "/", string? sessionID = null, int SliceSize = 0, int Slice = 0, bool ScreenOut = true)
            {
                string? Cookie = Users[UsersIndex].Cookie;
                return User.UpFile(Url,Cookie, policy, FilesPath, CloudFilesPath, sessionID, SliceSize, Slice, ScreenOut);
            }*/
            /// <summary>
            /// 用户方法
            /// </summary>
            public MethodsDefinition Methods(int UserIdex)
            {
                return new(UserIdex, this);
            }
            public MethodsDefinition Methods(string UserName)
            {
                return new(UserReturnIndex(UserName), this);
            }


            /// <summary>
            /// 用户组方法定义
            /// </summary>
            public class MethodsDefinition
            {
                public UserGroup UserGroup;
                public int UsersIndex = -1;
                public UserData UserData = new();

                public MethodsDefinition(int UsersIndex, UserGroup UserGroup)
                {
                    if (UserGroup.Users.Count == 0)
                    {
                        Logger.WriteError("传入的用户组内容数量不可以为0");
                        throw new InvalidOperationException("传入的用户组内容数量不可以为0");
                    }
                    if (UsersIndex == -1 || UserGroup.Users.Count < UsersIndex)
                    {
                        Logger.WriteError($"传入的用户Index错误 Index:{UsersIndex}");
                        throw new InvalidOperationException("传入的用户Index错误");
                    }
                    this.UserGroup = UserGroup;
                    this.UsersIndex = UsersIndex;
                    UserData = UserGroup.Users[UsersIndex];
                }

                //User操作
                /// <summary>
                /// 上传文件至云盘
                /// </summary>
                /// <param name="Url">Cloudreve服务器地址</param>
                /// <param name="cookie">登入返还的Cookie</param>
                /// <param name="policy">Cloudreve的存储策略</param>
                /// <param name="FilesPath">本地文件路径</param>
                /// <param name="CloudFilesPath">云盘上传路径(默认根目录"/")</param>
                /// <param name="sessionID">任务ID 可替PUT请求 前提任务ID没有被清除</param>
                /// <param name="SliceSize">上传分片大小</param>
                /// <param name="Slice">上传任务分片</param>
                /// <param name="ScreenOut">屏幕显示输出</param>
                /// <returns>string 类型 上传状态</returns>
                public string? UpFile(string policy, string FilesPath, string CloudFilesPath = "/", bool ScreenOut = true)
                {
                    return User.UpFile(UserGroup.Url, UserData.Cookie, policy, FilesPath, CloudFilesPath, ScreenOut: ScreenOut);
                }
                /// <summary>
                /// 删除上传文件列队
                /// </summary>
                /// <param name="Url">Cloudreve服务器地址</param>
                /// <param name="cookie">登入返还的Cookie</param>
                /// <param name="sessionID">任务ID</param>
                /// <param name="ScreenOut">屏幕显示输出</param>
                /// <returns>DeleteUpFileListReturnJson 类型 返回</returns>
                public DeleteUpFileListReturnJson? DeleteUpFileList(string? sessionID = null, bool ScreenOut = true)
                {
                    return User.DeleteUpFileList(UserGroup.Url, UserData.Cookie, sessionID, ScreenOut);
                }
                /// <summary>
                /// 获取目录内容
                /// </summary>
                /// <param name="Url">Cloudreve服务器地址</param>
                /// <param name="cookie">登入返还的Cookie</param>
                /// <param name="Directory">云盘目录</param>
                /// <param name="ScreenOut">屏幕显示输出</param>
                /// <returns>DirectoryDataReturnJson 类型 返回内容</returns>
                public DirectoryDataReturnJson? GetDirectory(string? Directory = null, bool ScreenOut = true)
                {
                    return User.GetDirectory(UserGroup.Url, UserData.Cookie, Directory, ScreenOut);
                }
                /// <summary>
                /// 获取文件下载路径
                /// </summary>
                /// <param name="Url">Cloudreve服务器地址</param>
                /// <param name="cookie">登入返还的Cookie</param>
                /// <param name="ID">文件ID</param>
                /// <param name="ScreenOut">屏幕显示输出</param>
                /// <returns>DirectoryDataReturnJson 类型 返回内容</returns>
                public DownloadReturnJson? GetDownloadUrl(string ID, bool ScreenOut = true)
                {
                    return User.GetDownloadUrl(UserGroup.Url, UserData.Cookie, ID, ScreenOut);
                }
                /// <summary>
                /// 获取文件外链
                /// </summary>
                /// <param name="Url">Cloudreve服务器地址</param>
                /// <param name="cookie">登入返还的Cookie</param>
                /// <param name="ID">文件ID</param>
                /// <param name="ScreenOut">屏幕显示输出</param>
                /// <returns>FileSourceDataReturnJson 类型 返回内容</returns>
                public FileSourceDataReturnJson? GetFileSource(List<string> ID, bool ScreenOut = true)
                {
                    return User.GetFileSource(UserGroup.Url, UserData.Cookie, ID, ScreenOut);
                }
                /// <summary>
                /// 获取文件分享链接
                /// </summary>
                /// <param name="Url">Cloudreve服务器地址</param>
                /// <param name="cookie">登入返还的Cookie</param>
                /// <param name="ID">文件ID</param>
                /// <param name="is_dir">是否为文件夹</param>
                /// <param name="password">密码</param>
                /// <param name="downloads">下载多少次过期</param>
                /// <param name="expire">过期时间</param>
                /// <param name="preview">是否允许预览</param>
                /// <param name="ScreenOut">屏幕显示输出</param>
                /// <returns>FileSourceDataReturnJson 类型 返回内容</returns>
                public FileShareDataReturnJson? GetFileShare(string ID, bool is_dir = false, string? password = null, int downloads = -1, int expire = 86400, bool preview = true, bool ScreenOut = true)
                {
                    return User.GetFileShare(UserGroup.Url, UserData.Cookie, ID, is_dir, password, downloads, expire, preview, ScreenOut);
                }
                /// <summary>
                /// 查询文件分享链接列表
                /// </summary>
                /// <param name="Url">Cloudreve服务器地址</param>
                /// <param name="cookie">登入返还的Cookie</param>
                /// <param name="page">页数</param>
                /// <param name="Sort">排序方式</param>
                /// <param name="ScreenOut">屏幕显示输出</param>
                /// <returns>FileShareShowreturnJson 类型 返回内容</returns>
                public FileShareShowreturnJson? GetFileShareShow(int page = 1, Sort Sort = Sort.CreationDateFromLateToEarly, bool ScreenOut = true)
                {
                    return User.GetFileShareShow(UserGroup.Url, UserData.Cookie, page, Sort, ScreenOut);
                }
                /// <summary>
                /// 设置文件分享链接
                /// </summary>
                /// <param name="Url">Cloudreve服务器地址</param>
                /// <param name="cookie">登入返还的Cookie</param>
                /// <param name="Key">分享文件ID</param>
                /// <param name="Settings">设置选项</param>
                /// <param name="value">设置值</param>
                /// <param name="ScreenOut">屏幕显示输出</param>
                /// <returns>FileShareShowreturnJson 类型 返回内容</returns>
                public SettingsReturnJson? SetFileShare(string Key, Settings Settings, string value, bool ScreenOut = true)
                {
                    return User.SetFileShare(UserGroup.Url, UserData.Cookie, Key, Settings, value, ScreenOut);
                }
                /// <summary>
                /// 删除文件分享链接
                /// </summary>
                /// <param name="Url">Cloudreve服务器地址</param>
                /// <param name="cookie">登入返还的Cookie</param>
                /// <param name="Key">分享文件ID</param>
                /// <param name="Settings">设置选项</param>
                /// <param name="value">设置值</param>
                /// <param name="ScreenOut">屏幕显示输出</param>
                /// <returns>FileShareShowreturnJson 类型 返回内容</returns>
                public DeltetShareReturnJson? DeltetFileShare(string Key, bool ScreenOut = true)
                {
                    return User.DeltetFileShare(UserGroup.Url, UserData.Cookie, Key, ScreenOut);
                }
                /// <summary>
                /// 获取Config
                /// </summary>
                /// <param name="Url">Cloudreve服务器地址</param>
                /// <param name="cookie">登入返还的Cookie</param>
                /// <param name="ScreenOut">屏幕显示输出</param>
                /// <returns>ConfigReturnJson 类型 返回内容</returns>
                public ConfigReturnJson? GetConfig(bool ScreenOut = true)
                {
                    return User.GetConfig(UserGroup.Url, UserData.Cookie, ScreenOut);
                }
                /// <summary>
                /// 删除文件
                /// </summary>
                /// <param name="Url">Cloudreve服务器地址</param>
                /// <param name="cookie">登入返还的Cookie</param>
                /// <param name="FilesID">要删除的文件ID</param>
                /// <param name="DirsID">要删除的文件夹ID</param>
                /// <param name="ScreenOut">屏幕显示输出</param>
                /// <returns>DeleteFilesDataReturnJson 类型 返回内容</returns>
                public DeleteFilesDataReturnJson? DeleteFiles(List<string>? FilesID = null, List<string>? DirsID = null, bool ScreenOut = true)
                {
                    return User.DeleteFiles(UserGroup.Url, UserData.Cookie, FilesID, DirsID, ScreenOut);
                }
                /// <summary>
                /// 获取文件详细信息
                /// </summary>
                /// <param name="Url">Cloudreve服务器地址</param>
                /// <param name="cookie">登入返还的Cookie</param>
                /// <param name="FilesID">要查询的文件ID</param>
                /// <param name="DirsID">要查询的文件夹ID</param>
                /// <param name="trace_root">文件路径跟踪</param>
                /// <param name="ScreenOut">屏幕显示输出</param>
                /// <returns>FilesDataReturnJson 类型 返回内容</returns>
                public FilesDataReturnJson? GetFilesData(string? FilesID = null, string? DirsID = null, bool trace_root = true, bool ScreenOut = true)
                {
                    return User.GetFilesData(UserGroup.Url, UserData.Cookie, FilesID, DirsID, trace_root, ScreenOut);
                }
                /// <summary>
                /// 获取云盘容量
                /// </summary>
                /// <param name="Url">Cloudreve服务器地址</param>
                /// <param name="cookie">登入返还的Cookie</param>
                /// <param name="ScreenOut">屏幕显示输出</param>
                /// <returns>CloudDriveSizeReturnJson 类型 返回内容</returns>
                public CloudDriveSizeReturnJson? GetCloudDriveSize(bool ScreenOut = true)
                {
                    return User.GetCloudDriveSize(UserGroup.Url, UserData.Cookie, ScreenOut);
                }
                /// <summary>
                /// 搜索文件
                /// </summary>
                /// <param name="Url">Cloudreve服务器地址</param>
                /// <param name="cookie">登入返还的Cookie</param>
                /// <param name="name">要搜索的文件名</param>
                /// <param name="CloudFilesPath">搜索的目录</param>
                /// <param name="ScreenOut">屏幕显示输出</param>
                /// <returns>FileSearchReturnJson 类型 返回内容</returns>
                public FileSearchReturnJson? FileSearch(string name, string CloudFilesPath = "", bool ScreenOut = true)
                {
                    return User.FileSearch(UserGroup.Url, UserData.Cookie, name, CloudFilesPath, ScreenOut);
                }
                /// <summary>
                /// 搜索分享链接
                /// </summary>
                /// <param name="Url">Cloudreve服务器地址</param>
                /// <param name="cookie">登入返还的Cookie</param>
                /// <param name="name">要搜索的文件名</param>
                /// <param name="page">页数</param>
                /// <param name="Sort">排序方式</param>
                /// <param name="ScreenOut">屏幕显示输出</param>
                /// <returns>ShareSearchReturnJson 类型 返回内容</returns>
                public ShareSearchReturnJson? ShareSearch(string name, int page = 1, Sort Sort = Sort.CreationDateFromLateToEarly, bool ScreenOut = true)
                {
                    return User.ShareSearch(UserGroup.Url, UserData.Cookie, name, page, Sort, ScreenOut);
                }


                //Admin操作
                /// <summary>
                /// 获取用户组定义列表
                /// </summary>
                /// <param name="Url">Cloudreve服务器地址</param>
                /// <param name="cookie">登入返还的Cookie</param>
                /// <param name="page">页数</param>
                /// <param name="page_size">一页所展示的数量</param>
                /// <param name="Sort">排序方式</param>
                /// <param name="ScreenOut">屏幕显示输出</param>
                /// <returns>GroupsListDataReturnJson 类型 返回内容</returns>
                public GroupsListDataReturnJson? GetGroupsList(int page = 1, int page_size = 999, GroupsListJson.Sort Sort = GroupsListJson.Sort.id_desc, bool ScreenOut = true)
                {
                    return Admin.GetGroupsList(UserGroup.Url, UserData.Cookie, page, page_size, Sort, ScreenOut);
                }
                /// <summary>
                /// 获取用户列表
                /// </summary>
                /// <param name="Url">Cloudreve服务器地址</param>
                /// <param name="cookie">登入返还的Cookie</param>
                /// <param name="ScreenOut">屏幕显示输出</param>
                /// <returns>GroupsReturnJson 类型 返回内容</returns>
                public GroupsReturnJson? GetGroups(bool ScreenOut = true)
                {
                    return Admin.GetGroups(UserGroup.Url, UserData.Cookie, ScreenOut);
                }
                /// <summary>
                /// 获取用户列表
                /// </summary>
                /// <param name="Url">Cloudreve服务器地址</param>
                /// <param name="cookie">登入返还的Cookie</param>
                /// <param name="page">页数</param>
                /// <param name="page_size">一页所展示的数量</param>
                /// <param name="Sort">排序方式</param>
                /// <param name="ScreenOut">屏幕显示输出</param>
                /// <returns>GroupsListDataReturnJson 类型 返回内容</returns>
                public UserListDatareturnJson? GetUserList(int page = 1, int page_size = 999, UserListJson.Sort Sort = UserListJson.Sort.id_desc, bool ScreenOut = true)
                {
                    return Admin.GetUserList(UserGroup.Url, UserData.Cookie, page, page_size, Sort, ScreenOut);
                }
            }

        }
        /// <summary>
        /// 用户组信息
        /// </summary>
        public class UserData
        {
            /// <summary>
            /// 账号
            /// </summary>
            public string? Account { get; set; } = null;
            /*/// <summary>
            /// 用户名
            /// </summary>
            public string? Name { get; set; } = null;
            /// <summary>
            /// 登入时间
            /// </summary>
            public DateTimeOffset? LoginTime { get; set; } = null;*/
            /// <summary>
            /// Cookie
            /// </summary>
            public string? Cookie { get; set; } = null;
            /*/// <summary>
            /// 总容量
            /// </summary>
            public double? Space { get; set; } = null;
            /// <summary>
            /// 已使用容量
            /// </summary>
            public double? UsedSpace { get; set; } = null;
            /// <summary>
            /// 剩余容量
            /// </summary>
            public double? RemainingSpace { get; set; } = null;*/
        }
    }
}