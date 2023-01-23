using System.ComponentModel;

namespace Cloudreve_API_DLL.Json.Admin
{
    public class UserListJson
    {
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        /// <summary>
        /// 用户合成Json
        /// </summary>
        public class UserListDataJson
        {
            /// <summary>
            /// 当前页
            /// </summary>
            public int page { get; set; }
            /// <summary>
            /// 一页显示多少个
            /// </summary>
            public int page_size { get; set; }
            /// <summary>
            /// 排序方式
            /// </summary>
            public string? order_by { get; set; }
            public string? UserListDataReturnJson()
            {
                return System.Text.Json.JsonSerializer.Serialize(this);
            }
            public static string? UserListDataReturnJson(int page, int page_size, string? order_by)
            {
                return System.Text.Json.JsonSerializer.Serialize<UserListDataJson>(new() { page = page, page_size = page_size, order_by = order_by });
            }
        }
        public class OptionsSerialized
        {
            /// <summary>
            /// 分享下载
            /// </summary>
            public bool share_download { get; set; }
            /// <summary>
            /// 来源批次
            /// </summary>
            public int source_batch { get; set; }
            /// <summary>
            /// aria2批量生成
            /// </summary>
            public int aria2_batch { get; set; }
            /// <summary>
            /// ??
            /// </summary>
            public string token { get; set; }
            /// <summary>
            /// 文件类型
            /// </summary>
            public List<string> file_type { get; set; }
            /// <summary>
            /// ??
            /// </summary>
            public string mimetype { get; set; }
            /// <summary>
            /// 块大小
            /// </summary>
            public int chunk_size { get; set; }
        }

        public class @Group
        {
            /// <summary>
            /// ID
            /// </summary>
            public int ID { get; set; }
            /// <summary>
            /// 创建时间
            /// </summary>
            public string CreatedAt { get; set; }
            /// <summary>
            /// 更新时间
            /// </summary>
            public string UpdatedAt { get; set; }
            /// <summary>
            /// 删除时间
            /// </summary>
            public string DeletedAt { get; set; }
            /// <summary>
            /// 用户名
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 储存策略
            /// </summary>
            public string Policies { get; set; }
            /// <summary>
            /// 最大储存空间
            /// </summary>
            public long MaxStorage { get; set; }
            /// <summary>
            /// 启用分享功能
            /// </summary>
            public bool ShareEnabled { get; set; }
            /// <summary>
            /// 启用WebDAVE功能
            /// </summary>
            public bool WebDAVEnabled { get; set; }
            /// <summary>
            /// 速度限制
            /// </summary>
            public int SpeedLimit { get; set; }
            /// <summary>
            /// 策略列表
            /// </summary>
            public List<int> PolicyList { get; set; }
            /// <summary>
            /// 串行化的选项
            /// </summary>
            public OptionsSerialized OptionsSerialized { get; set; }
        }
        public class Policy
        {
            /// <summary>
            /// ID
            /// </summary>
            public int ID { get; set; }
            /// <summary>
            /// 创建时间
            /// </summary>
            public string CreatedAt { get; set; }
            /// <summary>
            /// 更新时间
            /// </summary>
            public string UpdatedAt { get; set; }
            /// <summary>
            /// 删除时间
            /// </summary>
            public string DeletedAt { get; set; }
            /// <summary>
            /// 储存策略名
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 类型
            /// </summary>
            public string Type { get; set; }
            /// <summary>
            /// 服务器
            /// </summary>
            public string Server { get; set; }
            /// <summary>
            /// 水桶名称??
            /// </summary>
            public string BucketName { get; set; }
            /// <summary>
            /// 是私人的
            /// </summary>
            public bool IsPrivate { get; set; }
            /// <summary>
            /// 基础URL
            /// </summary>
            public string BaseURL { get; set; }
            /// <summary>
            /// 访问密钥
            /// </summary>
            public string AccessKey { get; set; }
            /// <summary>
            /// 秘密钥匙
            /// </summary>
            public string SecretKey { get; set; }
            /// <summary>
            /// 最大容量
            /// </summary>
            public long MaxSize { get; set; }
            /// <summary>
            /// 自动重命名
            /// </summary>
            public bool AutoRename { get; set; }
            /// <summary>
            /// 目录重名规则
            /// </summary>
            public string DirNameRule { get; set; }
            /// <summary>
            /// 文件重命名规则
            /// </summary>
            public string FileNameRule { get; set; }
            /// <summary>
            /// 允许获取外链
            /// </summary>
            public bool IsOriginLinkEnable { get; set; }
            /// <summary>
            /// 方案
            /// </summary>
            public string Options { get; set; }
            /// <summary>
            /// 串行化的选项
            /// </summary>
            public OptionsSerialized OptionsSerialized { get; set; }
            /// <summary>
            /// 主人ID
            /// </summary>
            public string MasterID { get; set; }
        }


        public class ItemsItem
        {
            /// <summary>
            /// ID
            /// </summary>
            public int ID { get; set; }
            /// <summary>
            /// 创建时间
            /// </summary>
            public string CreatedAt { get; set; }
            /// <summary>
            /// 更新时间
            /// </summary>
            public string UpdatedAt { get; set; }
            /// <summary>
            /// 删除时间
            /// </summary>
            public string DeletedAt { get; set; }
            /// <summary>
            /// 电子邮件
            /// </summary>
            public string Email { get; set; }
            /// <summary>
            /// 别名??
            /// </summary>
            public string Nick { get; set; }
            /// <summary>
            /// 用户状态
            /// </summary>
            public int Status { get; set; }
            /// <summary>
            /// 用户组ID
            /// </summary>
            public int GroupID { get; set; }
            /// <summary>
            /// 已使用空间大小
            /// </summary>
            public long Storage { get; set; }
            /// <summary>
            /// 两步验证
            /// </summary>
            public string TwoFactor { get; set; }
            /// <summary>
            /// 阿凡达???
            /// </summary>
            public string Avatar { get; set; }
            /// <summary>
            /// 不知道这是啥
            /// </summary>
            public string Authn { get; set; }
            /// <summary>
            /// 所在用户组
            /// </summary>
            public @Group @Group { get; set; }
            /// <summary>
            /// 储存策略
            /// </summary>
            public Policy Policy { get; set; }
            /// <summary>
            /// 串行化的选项
            /// </summary>
            public OptionsSerialized OptionsSerialized { get; set; }
        }

        public class Data
        {
            /// <summary>
            /// 用户列表
            /// </summary>
            public List<ItemsItem> items { get; set; }
            /// <summary>
            /// 用户总数
            /// </summary>
            public int total { get; set; }
        }

        public class UserListDatareturnJson
        {
            /// <summary>
            /// 响应代号
            /// </summary>
            public int code { get; set; }
            /// <summary>
            /// 用户内容
            /// </summary>
            public Data? data { get; set; }
            /// <summary>
            /// 反馈信息
            /// </summary>
            public string msg { get; set; }
        }
        public enum Sort
        {
            /// <summary>
            /// ID倒序
            /// </summary>
            [Description("id desc")]
            id_desc,
            /// <summary>
            /// ID正序
            /// </summary>
            [Description("id asc")]
            id_asc,
        }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
    }
}
