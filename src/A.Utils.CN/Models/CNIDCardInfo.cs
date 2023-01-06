using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A.Utils.CN.Models
{
    /// <summary>
    /// 中文身份证信息类
    /// </summary>
    public class CNIDCardInfo
    {
        /// <summary>
        /// 身份证号码
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 行政区划码
        /// </summary>
        public string DistrictCode { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 校验码
        /// </summary>
        public string CheckCode { get; set; }

        /// <summary>
        /// 性别(M-男，F-女，N-未知)
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 顺序码
        /// </summary>
        public string OrderCode { get; set; }
    }
}
