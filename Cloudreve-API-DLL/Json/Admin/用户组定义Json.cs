using System.ComponentModel;

namespace Cloudreve_API_DLL.Json.Admin
{
    public class GroupsListJson
    {
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        /// <summary>
        /// 用户组定义合成Json
        /// </summary>
        public class GroupsListDataJson
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
            public string? GroupsListDataReturnJson()
            {
                return System.Text.Json.JsonSerializer.Serialize(this);
            }
            public static string? GroupsListDataReturnJson(int page, int page_size, string? order_by)
            {
                return System.Text.Json.JsonSerializer.Serialize<GroupsListDataJson>(new() { page = page, page_size = page_size, order_by = order_by });
            }
        }

        public class OptionsSerialized
        {
            /// <summary>
            /// 档案下载
            /// </summary>
            public bool archive_download { get; set; }
            /// <summary>
            /// 归档任务
            /// </summary>
            public bool archive_task { get; set; }
            /// <summary>
            /// 分享下载
            /// </summary>
            public bool share_download { get; set; }
            /// <summary>
            /// 启用aria2
            /// </summary>
            public bool aria2 { get; set; }
            /// <summary>
            /// 来源批量
            /// </summary>
            public int source_batch { get; set; }
            /// <summary>
            /// aria2批量生成
            /// </summary>
            public int aria2_batch { get; set; }
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
            /// 被删除在
            /// </summary>
            public string DeletedAt { get; set; }
            /// <summary>
            /// 用户组名
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 存储策略
            /// </summary>
            public string Policies { get; set; }
            /// <summary>
            /// 最大储存容量
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
            /// 储存策略列表
            /// </summary>
            public List<int> PolicyList { get; set; }
            /// <summary>
            /// 串行化的选项
            /// </summary>
            public OptionsSerialized OptionsSerialized { get; set; }
        }

        public class Policies//尚未想好如何定义
        {
        }

        public class Statics//尚未想好如何定义
        {
        }

        public class Data
        {
            /// <summary>
            /// 用户组内容
            /// </summary>
            public List<ItemsItem>? items { get; set; }
            /// <summary>
            /// 存储策略
            /// </summary>
            public Policies? policies { get; set; }
            /// <summary>
            /// 统计内容
            /// </summary>
            public Statics? statics { get; set; }
            /// <summary>
            /// 总数
            /// </summary>
            public int total { get; set; }
        }
        /// <summary>
        /// 用户组定义返回Json
        /// </summary>
        public class GroupsListDataReturnJson
        {
            /// <summary>
            /// 响应代号
            /// </summary>
            public int code { get; set; }
            /// <summary>
            /// 用户组内容
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
