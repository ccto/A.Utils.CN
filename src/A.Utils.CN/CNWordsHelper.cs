using System.Text;

namespace A.Utils.CN
{
    /// <summary>
    /// 中文汉字帮助类
    /// </summary>
    public static class CNWordsHelper
    {
        /// <summary>   
        /// 随机产生常用汉字   
        /// </summary>   
        /// <param name="count">要产生汉字的个数</param>   
        /// <returns>常用汉字</returns>   
        public static List<string> GenerateWords(int count)
        {
            List<string> chineseWords = new List<string>();
            Random rm = new Random();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding gb = Encoding.GetEncoding("gb2312");

            for (int i = 0; i < count; i++)
            {
                // 获取区码(常用汉字的区码范围为16-55)   
                int regionCode = rm.Next(16, 56);
                // 获取位码(位码范围为1-94 由于55区的90,91,92,93,94为空,故将其排除)   
                int positionCode;
                if (regionCode == 55)
                {
                    // 55区排除90,91,92,93,94   
                    positionCode = rm.Next(1, 90);
                }
                else
                {
                    positionCode = rm.Next(1, 95);
                }

                // 转换区位码为机内码   
                int regionCode_Machine = regionCode + 160;// 160即为十六进制的20H+80H=A0H   
                int positionCode_Machine = positionCode + 160;// 160即为十六进制的20H+80H=A0H   

                // 转换为汉字   
                byte[] bytes = new byte[] { (byte)regionCode_Machine, (byte)positionCode_Machine };
                chineseWords.Add(gb.GetString(bytes));
            }

            return chineseWords;
        }

    }
}