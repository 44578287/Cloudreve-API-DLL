using LoongEgg.LoongLogger;
using static Cloudreve_API_DLL.Json.LoginJson;
using static Cloudreve_API_DLL.CloudreveAPI;
using Cloudreve_API_DLL;
using static Cloudreve_API_DLL.MODS.ConsolePrint;

DateTime beforDT = System.DateTime.Now;

Logger.Enable(LoggerType.Console | LoggerType.Debug, LoggerLevel.Debug);//注册Log日志函数



string ApiUrl2 = "http://127.0.0.1:5212";
string ApiUrl = "https://cloud.445720.xyz";
string ApiUrl3 = "http://10.10.10.7:5212";
LoginDataJson logindata = new()
{
    UserName = "test@445720.xyz",
    Password = "test@445720.xyz",
    CaptchaCode = ""
};
//UserGroup UserGroup = new(@"Data/Cookie", ApiUrl, false);
//while (UserGroup.Users.Count == 0)
//UserGroup.Login(logindata);

//UserGroup.Methods(0).GetDirectory("头像");
//UserGroup.Login(logindata2);
//string? cookie = Cloudreve_API_DLL.MODS.NetworkRequest.HttpRequest("http://localhost:5055/API/Chartbed/GetDefaultUser")?.Headers["Set-Cookie"];//获取Cookie
//Cloudreve_API_DLL.CloudreveAPI.User.GetCloudDriveSize(ApiUrl3, cookie);//校验cookie可用
//Cloudreve_API_DLL.MODS.NetworkRequest.HttpRequestToString();
//Cloudreve_API_DLL.CloudreveAPI.User.UpFile(ApiUrl3,cookie,"AAA", sessionID: "a5992993-12ae-40d8-ab3e-32d23af3ff6a", FilesPath: "C:\\Users\\g9964\\Pictures\\Screenshots\\屏幕截图(1).png");//上传
//UserGroup.Methods(0).UpFile("ppIj", "C:\\Users\\g9964\\Pictures\\Screenshots\\螢幕擷取畫面_20221212_023709.png");
//UserGroup.Methods(0).GetDirectory();
//UserGroup.Methods(0).UpFile( "g7SE", "C:\\Users\\g9964\\Pictures\\Screenshots\\螢幕擷取畫面_20221212_023709.png");
//UserGroup.Methods(0).DeleteUpFileList();
//Cloudreve_API_DLL.CloudreveAPI.User.UpFileAuto(ApiUrl2, UserGroup.Methods(0).UserData.Cookie, "C:\\Users\\g9964\\Downloads\\Nosub1.0beta9.2_win_x64.7z");
//Console.WriteLine(File.Exists("Data/cookie/KEY"));

Console.WriteLine("DateTime总共花费{0}ms.",System.DateTime.Now.Subtract(beforDT).TotalMilliseconds );