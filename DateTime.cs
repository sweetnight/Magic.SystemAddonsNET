using System;

namespace Magic.SystemAddonsNET
{
	public static class DateTime
	{
		public static long NowUnixLocal()
		{
			TimeSpan offset = TimeZoneInfo.Local.GetUtcOffset(System.DateTime.UtcNow);
			DateTimeOffset saatIni = DateTimeOffset.Now;

			// jadikan ke unix timestamp untuk digunakan nanti
			long saatIniUnix = saatIni.ToUnixTimeSeconds(); // UTC time
			saatIniUnix = saatIniUnix + ((long)offset.TotalSeconds); // Local time

			return saatIniUnix;
		} // end of method

		public static long NowUnix()
		{
			DateTimeOffset saatIni = DateTimeOffset.Now;

			long saatIniUnix = saatIni.ToUnixTimeSeconds(); // UTC time

			return saatIniUnix;
		} // end of method

		public class UnixTimeStampRemote
		{
			public string? UnixTimeStamp { get; set; }
		}
	} // end of class
} // end of namespace
