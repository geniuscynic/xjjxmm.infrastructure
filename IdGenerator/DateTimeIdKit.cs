using XjjXmm.Infrastructure.ToolKit;

namespace XjjXmm.Infrastructure.IdGenerator
{
	/// <summary>
	/// 根据twitter的snowflake算法生成唯一ID
	/// snowflake算法 64 位
	/// 0---0000000000 0000000000 0000000000 0000000000 0 --- 00000 ---00000 ---000000000000
	/// 第一位为未使用（实际上也可作为long的符号位），接下来的41位为毫秒级时间，然后5位datacenter标识位，5位机器ID（并不算标识符，实际是为线程标识），然后12位该毫秒内的当前毫秒内的计数，加起来刚好64位，为一个Long型。
	/// 其中datacenter标识位起始是机器位，机器ID其实是线程标识，可以同一一个10位来表示不同机器
	/// </summary>
    public class DateTimeIdKit
	{
        #region 私有字段

        
        private static long _sequence = 0; //计数从零开始
        private static string _lastTimestamp = ""; //最后时间戳


		private const long SequenceBits = 12L; //计数器字节数，12个字节用来保存计数码        
		private const long SequenceMask = -1L ^ -1L << (int)SequenceBits; //一毫秒内可以产生计数，如果达到该值则等到下一毫秒在进行生成


		private static readonly object SyncRoot = new object(); //加锁对象
       
        #endregion
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="machineId">机器码</param>
        /// <param name="datacenterId">数据中心id</param>
        public static string  NextId()
        {
            return GetLongId();
        }

        /// <summary>
        /// 获取长整形的ID
        /// </summary>
        /// <returns></returns>
        private static string GetLongId()
        {
            lock (SyncRoot)
            {
				var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
               
               // var dt2 = Twepoch.ToDateTime();
                if (_lastTimestamp == timestamp)
                {
					Console.WriteLine($"seq:{_sequence}");
                    //同一毫秒中生成ID
                    _sequence = (_sequence + 1) & SequenceMask; //用&运算计算该毫秒内产生的计数是否已经到达上限
					
                    if (_sequence == 0)
                    {
						Thread.Sleep(1000);
                        //一毫秒内产生的ID计数已达上限，等待下一毫秒
                        timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                    }
                }
                else
                {
                    //不同毫秒生成ID
                    _sequence = 0;
                }

                _lastTimestamp = timestamp; //把当前时间戳保存为最后生成ID的时间戳
                return $"{timestamp}{_sequence}";
            }
        } 
    }
}
