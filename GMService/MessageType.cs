/// <summary>
/// 消息类型
/// </summary>
public static class MessageType
{

#region 活动

	/// <summary>
	/// 活动：网址
	/// <para>W2S</para>
	/// <para>　string 网址</para>
	/// </summary>
	public const ushort Url = 10;

	/// <summary>
	/// 活动：开始
	/// </summary>
	public const ushort ActivityBegin = 100;

	/// <summary>
	/// 活动：检查
	/// <para>W2S</para>
	/// <para>　(无)</para>
	/// <para>S2W</para>
	/// <para>　bool 是否已有数据</para>
	/// </summary>
	public const ushort ActivityCheck = MessageType.ActivityBegin;

	/// <summary>
	/// 活动：更新
	/// <para>W2S</para>
	/// <para>　ushort　 数量</para>
	/// <para>　　string 发送地址</para>
	/// <para>　　string 服务器编号</para>
	/// <para>　　ushort 更新年</para>
	/// <para>　　byte　 更新月</para>
	/// <para>　　byte　 更新日</para>
	/// <para>　　byte[] 缓冲区</para>
	/// <para>S2W</para>
	/// <para>　ushort　 数量</para>
	/// <para>　　string 发送地址</para>
	/// <para>　　string 服务器编号</para>
	/// </summary>
	public const ushort ActivityUpdate = MessageType.ActivityBegin + 1;

#endregion
	
#region PVP奖励

	/// <summary>
	/// PVP奖励：开始
	/// </summary>
	public const ushort PVPRewardBegin = 110;

	/// <summary>
	/// PVP奖励：检查
	/// <para>W2S</para>
	/// <para>　(无)</para>
	/// <para>S2W</para>
	/// <para>　bool 是否已有数据</para>
	/// </summary>
	public const ushort PVPRewardCheck = MessageType.PVPRewardBegin;

	/// <summary>
	/// PVP奖励：更新
	/// <para>W2S</para>
	/// <para>　ushort　 数量</para>
	/// <para>　　string 发送地址</para>
	/// <para>　　string 服务器编号</para>
	/// <para>　　ushort 更新年</para>
	/// <para>　　byte　 更新月</para>
	/// <para>　　byte　 更新日</para>
	/// <para>　　int　　礼包编号</para>
	/// <para>S2W</para>
	/// <para>　ushort　 数量</para>
	/// <para>　　string 发送地址</para>
	/// <para>　　string 服务器编号</para>
	/// </summary>
	public const ushort PVPRewardUpdate = MessageType.PVPRewardBegin + 1;
	
#endregion
	
#region 广播

	/// <summary>
	/// 广播：开始
	/// </summary>
	public const ushort BroadcastBegin = 120;

	/// <summary>
	/// PVP奖励：更新
	/// <para>W2S</para>
	/// <para>　ushort　 数量</para>
	/// <para>　　string 发送地址</para>
	/// <para>　　string 服务器编号</para>
	/// <para>　　int　　间隔秒</para>
	/// <para>　　string 内容</para>
	/// </summary>
	public const ushort BroadcastUpdate = MessageType.BroadcastBegin + 1;
	
#endregion

}