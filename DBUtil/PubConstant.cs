using System;
using System.Configuration;

namespace DBUtil
{
    /// <summary>
    /// 用于获取数据库连接字符串
    /// copyright (c) 2016 By jiangyanbin
    /// </summary>
    public class PubConstant
    {
        /// <summary>
        /// 获得数据库连接字符串
        /// </summary>
        /// <param name="isSuccess">读取成功返回true，失败返回false</param>
        /// <param name="errMessage">返回错误信息</param>
        /// <returns>返回连接字符串</returns>
        public static string GetConnectionString(out bool isSuccess, out string errMessage)
        {
            try
            {
                string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
                if (connectionString == string.Empty)
                {
                    isSuccess = false;
                    errMessage = "未从配置文件中读取到字符串，请检查配置文件！";
                    return null;
                }
                isSuccess = true;
                errMessage = null;
                return connectionString;
            }
            catch (Exception err)
            {
                isSuccess = false;
                errMessage = err.Message;
                return null;
            }
        }
    }
}
