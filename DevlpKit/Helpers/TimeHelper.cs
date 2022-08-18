using System;

namespace Kits.DevlpKit.Helpers
{
	public static class TimeHelper
	{
		private static readonly long epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
		/// <summary>
		/// 客户端时间
		/// </summary>
		/// <returns></returns>
		public static long ClientNow()
		{
			return (DateTime.UtcNow.Ticks - epoch) / 10000;
		}

		public static long ClientNowSeconds()
		{
			return (DateTime.UtcNow.Ticks - epoch) / 10000000;
		}

		public static long Now()
		{
			return ClientNow();
		}
		
		public static long GetUnixTimestamp()
		{
			return new System.DateTimeOffset(System.DateTime.UtcNow).ToUnixTimeMilliseconds();
		}

		public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
		{
			System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
			dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
			return dtDateTime;
		}
    }
}