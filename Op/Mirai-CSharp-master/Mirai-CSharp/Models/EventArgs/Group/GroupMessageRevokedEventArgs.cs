﻿using Mirai_CSharp.Utility.JsonConverters;
using System;
using System.Text.Json.Serialization;

namespace Mirai_CSharp.Models
{
    /// <summary>
    /// 提供群内撤回消息的相关信息接口。继承自 <see cref="IGroupOperatingEventArgs"/> 和 <see cref="IMessageRevokedEventArgs"/>
    /// </summary>
    public interface IGroupMessageRevokedEventArgs : IGroupOperatingEventArgs, IMessageRevokedEventArgs
    {

    }

    public class GroupMessageRevokedEventArgs : MessageRevokedEventArgs, IGroupMessageRevokedEventArgs
    {
        /// <summary>
        /// 被撤回消息所在群信息
        /// </summary>
        [JsonConverter(typeof(ChangeTypeJsonConverter<GroupInfo, IGroupInfo>))]
        [JsonPropertyName("group")]
        public IGroupInfo Group { get; set; }
        /// <summary>
        /// 进行撤回操作的用户信息
        /// </summary>
        [JsonConverter(typeof(ChangeTypeJsonConverter<GroupMemberInfo, IGroupMemberInfo>))]
        [JsonPropertyName("operator")]
        public IGroupMemberInfo Operator { get; set; }

        public GroupMessageRevokedEventArgs() { }

        public GroupMessageRevokedEventArgs(IGroupInfo group, IGroupMemberInfo @operator, long senderId, int messageId, DateTime sentTime) : base(senderId, messageId, sentTime)
        {
            Group = group;
            Operator = @operator;
        }
    }
}
