using LoongEgg.LoongLogger;
using static Cloudreve_API_DLL.Json.LoginJson;
using static Cloudreve_API_DLL.CloudreveAPI;

DateTime beforDT = System.DateTime.Now;

Logger.Enable(LoggerType.Console | LoggerType.Debug, LoggerLevel.Debug);//注册Log日志函数



string ApiUrl = "http://127.0.0.1:5212";
LoginDataJson logindata = new()
{
    UserName = "test@445720.xyz",
    Password = "test@445720.xyz",
    CaptchaCode = ""
};
UserGroup UserGroup = new(@"Data/Cookie", ApiUrl, false);
UserGroup.Login(logindata);
//UserGroup.Login(logindata2);

//UserGroup.Methods(0).UpFile("ppIj", "C:\\Users\\g9964\\Pictures\\Screenshots\\螢幕擷取畫面_20221212_023709.png");
//UserGroup.Methods(0).GetDirectory();


//Console.WriteLine(File.Exists("Data/cookie/KEY"));
Console.WriteLine("DateTime总共花费{0}ms.", System.DateTime.Now.Subtract(beforDT).TotalMilliseconds);