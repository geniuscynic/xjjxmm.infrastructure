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
    public class SnowFlakeKit
    {
        #region 私有字段

        //private static long _machineId; //机器码
        //private static long _dataId; //数据ID
        
        private static long _sequence; //计数从零开始
        private static long _lastTimestamp = -1L; //最后时间戳

        //2020-01-01
        private const long Twepoch =  1577808000000L; //唯一时间随机量

        private const long MachineIdBits = 5L; //机器码字节数
        private const long DataCenterBits = 5L; //数据字节数
        private const long MaxMachineId = -1L ^ -1L << (int)MachineIdBits; //最大机器码
        private const long MaxDataBitId = -1L ^ (-1L << (int)DataCenterBits); //最大数据字节数

        private const long SequenceBits = 12L; //计数器字节数，12个字节用来保存计数码        
        private const long MachineIdShift = SequenceBits; //机器码数据左移位数，就是后面计数器占用的位数
        private const long DatacenterIdShift = SequenceBits + MachineIdBits;
        private const long TimestampLeftShift = DatacenterIdShift + DataCenterBits; //时间戳左移动位数就是机器码+计数器总字节数+数据字节数
        private const long SequenceMask = -1L ^ -1L << (int)SequenceBits; //一毫秒内可以产生计数，如果达到该值则等到下一毫秒在进行生成

        private static readonly object SyncRoot = new object(); //加锁对象
        private static SnowFlakeKit _snowFlake;

        #endregion
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="machineId">机器码</param>
        /// <param name="datacenterId">数据中心id</param>
        public static long  NextId(long machineId, long datacenterId = 0)
        {
            if (machineId >= 0)
            {
                if (machineId > MaxMachineId)
                {
                    throw new Exception("机器码ID非法");
                }

                //_machineId = machineId;
            }

            if (datacenterId >= 0)
            {
                if (datacenterId > MaxDataBitId)
                {
                    throw new Exception("数据中心ID非法");
                }

                //_dataId = datacenterId;
            }

            return GetLongId(machineId, datacenterId);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="machineId">机器码</param>
        /// <param name="datacenterId">数据中心id</param>
        public static long  NextId(long datacenterId = 0)
        {
	        var machineId = Thread.CurrentThread.ManagedThreadId % (MaxMachineId + 1);
	        if (machineId >= 0)
	        {
		        if (machineId > MaxMachineId)
		        {
			        throw new Exception("机器码ID非法");
		        }

		        //_machineId = machineId;
	        }

	        if (datacenterId >= 0)
	        {
		        if (datacenterId > MaxDataBitId)
		        {
			        throw new Exception("数据中心ID非法");
		        }

		        //_dataId = datacenterId;
	        }

	        return GetLongId(machineId, datacenterId);
        }

        /// <summary>
        /// 获取长整形的ID
        /// </summary>
        /// <returns></returns>
        private static long GetLongId(long machineId, long datacenterId)
        {
            lock (SyncRoot)
            {
                var timestamp = DateTime.UtcNow.ToTotalMilliseconds();
               var dt1 = new DateTime(2020,1,1).ToTotalMilliseconds(); ;
               // var dt2 = Twepoch.ToDateTime();
                if (_lastTimestamp == timestamp)
                {
                    //同一毫秒中生成ID
                    _sequence = (_sequence + 1) & SequenceMask; //用&运算计算该毫秒内产生的计数是否已经到达上限
                    if (_sequence == 0)
                    {
                        //一毫秒内产生的ID计数已达上限，等待下一毫秒
                        timestamp = DateTime.UtcNow.ToTotalMilliseconds();
                    }
                }
                else
                {
                    //不同毫秒生成ID
                    _sequence = 0L;
                }

                _lastTimestamp = timestamp; //把当前时间戳保存为最后生成ID的时间戳
                long id = ((timestamp - Twepoch) << (int)TimestampLeftShift) | (datacenterId << (int)DatacenterIdShift) | (machineId << (int)MachineIdShift) | _sequence;
                return id;
            }
        }

       
        
    }
}
