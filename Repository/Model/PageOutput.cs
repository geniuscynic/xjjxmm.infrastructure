using System.Collections.Generic;
using XjjXmm.Infrastructure.Mapper;

namespace XjjXmm.Infrastructure.Repository.Model
{
    /// <summary>
    /// 通用分页信息类
    /// </summary>
    public class PageOutput<T>
    {
        /// <summary>
        /// 当前页标
        /// </summary>
        public int CurrentPage { get; set; } = 1;

        //分页最大分页数
        public int MaxOffset { get; set; } = 5;

        /// <summary>
        /// 数据总数
        /// </summary>
        public int Total { get; set; } = 0;
        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { set; get; } = 10;
        /// <summary>
        /// 返回数据
        /// </summary>

        public List<T> Data { get; set; } = new List<T>();

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount => Total / PageSize + 1;
        //public int PageCount { get; set; } = 6;


        public int Start
        {
            get
            {
                var start = CurrentPage - MaxOffset;
                if(start < 1)
                {
                    start = 1;
                }

                return start;
            }
        }

        public int End
        {
            get
            {
              
                var end = CurrentPage + MaxOffset;
                //if (end > PageCount)
                //{
                //    end = PageCount;
                //}

                if (end < MaxOffset * 2)
                {
                    end = MaxOffset * 2;
                }

                if(end > PageCount)
                {
                    end = PageCount;
                }

               

                return end;
            }
        }

        public int? PrevPage
        {
            get {
               var prev =  CurrentPage - 1;
                if(prev < 1) return null;
                return prev;
            }
        }

        public int? NextPage
        {
            get
            {
                var next = CurrentPage + 1;
                if (next > PageCount) return null;
                return next;
            }
        }

        public PageOutput<T2> MapTo<T2>()
        {
	        return new PageOutput<T2>()
	        {
		        CurrentPage = CurrentPage,
		        Total = Total,
		        PageSize = PageSize,
		        Data = Data.MapTo<T, T2>().ToList()
	        };
        } 
    }

}
