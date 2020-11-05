using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;

namespace QH.Core.Result
{
    /// <summary>
    /// 返回结果模型接口
    /// </summary>
    public interface IResultModel
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        [JsonIgnore]
        [Description("是否成功")]
        bool Success { get; }

        /// <summary>
        /// 错误信息
        /// </summary>
        [Description("错误信息")]
        string Msg { get; }

        /// <summary>
        /// 状态码
        /// </summary>
        [Description("状态码")]
        int Code => Success ? 1 : 0;

        /// <summary>
        /// 模型验证失败
        /// </summary>
        [Description("模型验证失败")]
        List<Errors> Errors { get; }
    }

    /// <summary>
    /// 返回结果模型泛型接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IResultModel<T> : IResultModel
    {
        /// <summary>
        /// 返回数据
        /// </summary>
        [Description("返回数据")]
        T Data { get; }
    }
}
