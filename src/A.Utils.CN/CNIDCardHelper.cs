using A.Utils.CN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace A.Utils.CN
{
    /// <summary>
    /// 身份证号码帮助
    /// </summary>
    public static class CNIDCardHelper
    {
        /// <summary>
        /// 身份证加权因子
        /// </summary>
        public static int[] wi = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1 };
        /// <summary>
        /// 校验码
        /// </summary>
        public static string[] eccCode = { "1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2" };


        /// <summary>
        /// 按照规则生成身份证信息
        /// </summary>
        /// <param name="districtCode">区划码 6位数字</param>
        /// <param name="birthday">生日</param>
        /// <param name="orderCode">顺序码 3位数字</param>
        /// <returns></returns>
        public static CNIDCardInfo GenerateIDCard(string districtCode, DateTime birthday, string orderCode)
        {
            CNIDCardInfo result = new CNIDCardInfo() { DistrictCode = districtCode, Birthday = birthday, OrderCode = orderCode };

            var id17 = districtCode + birthday.ToString("yyyyMMdd") + orderCode;
            var id17Arr = id17.ToCharArray().Select(m => Convert.ToInt32(m)).ToArray();

            int iSum = GetPowerSum(id17Arr);
            string checkCode = GetCheckCode(iSum);

            result.CheckCode = checkCode;
            result.ID = id17 + checkCode;
            result.Gender = GetGenderByOrderCode(orderCode);

            return result;
        }

        /// <summary>
        /// 随机生成一个身份证号码
        /// </summary>
        /// <param name="disCode">行政区划编码</param>
        /// <returns></returns>
        public static string GenerateIDCard(string disCode)
        {
            return GenerateIDCardInfo(disCode).ID;
        }

        /// <summary>
        /// 随机生成一个身份证信息
        /// </summary>
        /// <param name="disCode">行政区划编码</param>
        /// <returns></returns>
        public static CNIDCardInfo GenerateIDCardInfo(string disCode)
        {
            var birthDay = GenerateDate(DateTime.Now.Date.AddYears(-70), DateTime.Now.Date.AddYears(-18));
            var ran3 = new Random().Next(1, 999).ToString().PadLeft(3, '0');

            var IdModelInfo = GenerateIDCard(disCode, birthDay, ran3);

            return IdModelInfo;
        }

        /// <summary>
        /// 生成随机日期
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private static DateTime GenerateDate(DateTime startDate, DateTime endDate)
        {
            return startDate.Date.AddDays(new Random().Next((endDate.Date - startDate.Date).Days));
        }


        /// <summary>
        /// 验证18位身份编码是否合法 
        /// </summary>
        /// <param name="idCard">身份编码</param>
        /// <returns></returns>
        public static bool ValidateIdCard(string idCard)
        {
            bool bTrue = false;
            if (idCard.Length == 18)
            {
                // 前17位  
                string code17 = idCard.Substring(0, 17);
                // 第18位  
                string code18 = idCard.Substring(17, 1);
                if (Regex.IsMatch(code17, @"^[+-]?\d*$"))
                {
                    var cArr = code17.ToCharArray();
                    if (cArr != null)
                    {
                        int[] iCard = cArr.Select(m => Convert.ToInt32(m)).ToArray();
                        int iSum17 = GetPowerSum(iCard);
                        // 获取校验位  
                        string val = GetCheckCode(iSum17);
                        return code17 == val;
                    }
                }
            }
            return bTrue;
        }


        /// <summary>
        /// 将身份证的每位和对应位的加权因子相乘之后，再得到和值 
        /// </summary>
        /// <param name="iArr"></param>
        /// <returns></returns>
        private static int GetPowerSum(int[] iArr)
        {
            int iSum = 0;
            if (wi.Length == iArr.Length)
            {
                for (int i = 0; i < iArr.Length; i++)
                {
                    iSum = iSum + iArr[i] * wi[i];
                }
            }
            return iSum;
        }

        /// <summary>
        /// 将power和值与11取模获得余数进行校验码判断 
        /// </summary>
        /// <param name="iSum"></param>
        /// <returns></returns>
        private static string GetCheckCode(int iSum)
        {
            string checkCode = eccCode[iSum % 11];
            return checkCode;
        }


        /// <summary>
        /// 根据身份编号获取性别
        /// </summary>
        /// <param name="idCard">idCard 身份编号</param>
        /// <returns>性别(M-男，F-女，N-未知) </returns>
        public static string GetGenderByIdCard(string idCard)
        {
            string sGender = "N";
            string sCardNum = idCard.Substring(16, 1);
            if (Convert.ToInt32(sCardNum) % 2 != 0)
            {
                sGender = "M";
            }
            else
            {
                sGender = "F";
            }
            return sGender;
        }

        /// <summary>
        /// 从顺序码中获取性别
        /// </summary>
        /// <param name="orderCode">顺序码</param>
        /// <returns>性别(M-男，F-女，N-未知)</returns>

        public static string GetGenderByOrderCode(string orderCode)
        {
            string sGender = "N";
            string sCardNum = orderCode.Substring(2, 1);
            if (Convert.ToInt32(sCardNum) % 2 != 0)
            {
                sGender = "M";
            }
            else
            {
                sGender = "F";
            }
            return sGender;
        }


        /// <summary>
        /// 根据身份编号获取生日
        /// </summary>
        /// <param name="idCard">身份证编号</param>
        /// <returns></returns>
        public static DateTime GetBirthdayByIdCard(string idCard)
        {
            var basicStr = idCard.Substring(6, 8);

            return Convert.ToDateTime(basicStr.Substring(0, 4) + "-" + basicStr.Substring(4, 2) + "-" + basicStr.Substring(6, 2));
        }

        /// <summary>
        /// 根据身份证编号获取身份证信息
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        public static CNIDCardInfo GeCardInfoByIdCard(string idCard)
        {
            CNIDCardInfo result = new CNIDCardInfo() { ID = idCard, DistrictCode = idCard.Substring(0, 6), Birthday = GetBirthdayByIdCard(idCard), OrderCode = idCard.Substring(14, 3), CheckCode = idCard.Substring(17, 1) };
            result.Gender = GetGenderByOrderCode(result.OrderCode);

            return result;
        }


    }
}
