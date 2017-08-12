using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace DBUtil
{
    /// <summary>
    /// 数据访问类 单例模式
    /// Copyright (C) 2016 By jiangyanbin
    /// </summary>
    public class DbHelperSql
    {
        private static readonly DbHelperSql Instance;//唯一实例
        private string _connectionString;//连接字符串

        #region 单例相关方法


        /// <summary>
        /// 静态构造函数，由单例生成时调用，执行一次
        /// </summary>
        static DbHelperSql()
        {
            var connString = PubConstant.GetConnectionString(out bool isSuccess, out string errMessage);
            if (isSuccess == false)//出现错误，抛出异常，退出程序
            {
                throw new Exception(errMessage);
            }
            Instance = new DbHelperSql {_connectionString = connString};
        }

        /// <summary>
        /// 获得DBHelperSQL的单例
        /// </summary>
        /// <returns></returns>
        public static DbHelperSql GetInstance()
        {
            return Instance;
        }
        #endregion

        #region 私有方法

        /// <summary>
        /// 数据库命令预处理方法
        /// </summary>
        /// <param name="cmd">传入待处理数据库命令</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">数据库事务</param>
        /// <param name="cmdText">命令文本</param>
        /// <param name="isSuccess">执行 成功返回true，失败返回false</param>
        /// <param name="errMessage">返回错误信息</param>
        /// <param name="cmdParms">命令参数</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, out bool isSuccess, out string errMessage, SqlParameter[] cmdParms)
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = cmdText;
                if (trans != null)
                    cmd.Transaction = trans;
                cmd.CommandType = CommandType.Text;//cmdType;
                if (cmdParms != null)
                {
                    foreach (SqlParameter parameter in cmdParms)
                    {
                        if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                            (parameter.Value == null || parameter.Value.ToString() == string.Empty))
                        {
                            parameter.Value = DBNull.Value;
                        }
                        if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                            parameter.DbType == DbType.DateTime && parameter.Value.ToString() == DateTime.MinValue.ToString(CultureInfo.InvariantCulture))
                        {
                            parameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                        }
                        cmd.Parameters.Add(parameter);
                    }
                }
                isSuccess = true;
                errMessage = null;
            }
            catch (Exception err)
            {
                conn.Close();
                isSuccess = false;
                errMessage = err.Message;
            }
        }

        /// <summary>
        /// 构建 SqlCommand 对象 返回结果集
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>返回构建的SqlCommand</returns>
        private static SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(storedProcName, connection) {CommandType = CommandType.StoredProcedure};
            foreach (var dataParameter in parameters)
            {
                var parameter = (SqlParameter) dataParameter;
                if (parameter != null)
                {
                    // 检查未分配值的输出参数,将其分配以DBNull.Value.
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null || parameter.Value.ToString() == string.Empty))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    command.Parameters.Add(parameter);
                }
            }
            return command;
        }

        /// <summary>
        /// 创建 SqlCommand 对象实例 用来返回一个整数值 废弃
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand 对象实例</returns>
        private static SqlCommand BuildIntCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.Parameters.Add(new SqlParameter("ReturnValue",
                SqlDbType.Int, 4, ParameterDirection.ReturnValue,
                false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return command;
        }
        #endregion

        #region sql方法

        /// <summary>
        /// 查询是否存在
        /// </summary>
        /// <param name="strSql">查询是否存在字符串</param>
        /// <param name="isSuccess">查询成功返回true，失败返回false</param>
        /// <param name="errMessage">返回错误信息</param>
        /// <returns>存在返回true，不存在返回false</returns>
        public bool Exists(string strSql, out bool isSuccess, out string errMessage)
        {
            var obj = GetSingle(strSql, out isSuccess, out errMessage);
            if (isSuccess == false)
                return false;
            int cmdresult;
            if ((Equals(obj, null)) || (Equals(obj, DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)//等于0 不存在
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 查询是否存在 带参数
        /// </summary>
        /// <param name="strSql">查询是否存在字符串</param>
        /// <param name="isSuccess">查询成功返回true，失败返回false</param>
        /// <param name="errMessage">返回错误信息</param>
        /// <param name="cmdParms">参数列表</param>
        /// <returns>存在返回true，不存在返回false</returns>
        public bool Exists(string strSql, out bool isSuccess, out string errMessage, params SqlParameter[] cmdParms)
        {
            var obj = GetSingle(strSql, out isSuccess, out errMessage, cmdParms);
            if (isSuccess == false)
                return false;
            int cmdresult;
            if ((Equals(obj, null)) || (Equals(obj, DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)//等于0 不存在
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="sqlString">SQL语句</param>
        /// <param name="isSuccess">执行成功返回true，失败返回false</param>
        /// <param name="errMessage">返回错误信息</param>
        /// <returns>影响的记录数</returns>
        public int ExecuteSql(string sqlString, out bool isSuccess, out string errMessage)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlString, connection))
                    {
                        try
                        {
                            connection.Open();
                            int rows = cmd.ExecuteNonQuery();
                            isSuccess = true;
                            errMessage = null;
                            return rows;
                        }
                        catch (Exception err)
                        {
                            connection.Close();
                            isSuccess = false;
                            errMessage = err.Message;
                            return 0;
                        }
                    }
                }
            }
            catch (Exception err)
            {
                isSuccess = false;
                errMessage = err.Message;
                return 0;
            }
        }

        /// <summary>
        /// 执行SQL语句 带参数，返回影响的记录数
        /// </summary>
        /// <param name="sqlString">SQL语句</param>
        /// <param name="isSuccess">执行成功返回true，失败返回false</param>
        /// <param name="errMessage">返回错误信息</param>
        /// <param name="cmdParms">参数列表</param>
        /// <returns>返回影响的记录数</returns>
        public int ExecuteSql(string sqlString, out bool isSuccess, out string errMessage, params SqlParameter[] cmdParms)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        try
                        {
                            PrepareCommand(cmd, connection, null, sqlString, out isSuccess, out errMessage, cmdParms);
                            int rows = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                            return rows;
                        }
                        catch (Exception err)
                        {
                            connection.Close();
                            isSuccess = false;
                            errMessage = err.Message;
                            return 0;
                        }
                    }
                }
            }
            catch (Exception err)
            {
                isSuccess = false;
                errMessage = err.Message;
                return 0;
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="sqlString">计算查询结果语句</param>
        /// <param name="isSuccess">查询成功返回true，失败返回false</param>
        /// <param name="errMessage">返回错误信息</param>
        /// <returns>查询结果（object）</returns>
        public object GetSingle(string sqlString, out bool isSuccess, out string errMessage)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var cmd = new SqlCommand(sqlString, connection))
                    {
                        try
                        {
                            connection.Open();
                            var obj = cmd.ExecuteScalar();
                            isSuccess = true; errMessage = null;
                            //DbNull 是指数据库中当一个字段没有被设置值的时候的值，相当于数据库中的“空值”;
                            //null 是 C# 中是空引用的意思，表示没有引用任何对象。
                            if (Equals(obj, null) || (Equals(obj, DBNull.Value)))
                            {
                                return null;
                            }
                            return obj;
                        }
                        catch (Exception err)
                        {
                            connection.Close();
                            isSuccess = false;
                            errMessage = err.Message;
                            return null;
                        }
                    }
                }
            }
            catch (Exception err)
            {
                isSuccess = false;
                errMessage = err.Message;
                return null;
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句 带参数，返回查询结果（object）。
        /// </summary>
        /// <param name="sqlString">计算查询结果语句</param>
        /// <param name="isSuccess">查询成功返回true，失败返回false</param>
        /// <param name="errMessage">返回错误信息</param>
        /// <param name="cmdParms">参数列表</param>
        /// <returns>查询结果（object）</returns>
        public object GetSingle(string sqlString, out bool isSuccess, out string errMessage, params SqlParameter[] cmdParms)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var cmd = new SqlCommand())
                    {
                        try
                        {
                            PrepareCommand(cmd, connection, null, sqlString, out isSuccess, out errMessage, cmdParms);
                            var obj = cmd.ExecuteScalar();
                            cmd.Parameters.Clear();
                            if ((Equals(obj, null)) || (Equals(obj, DBNull.Value)))
                            {
                                return null;
                            }
                            else
                            {
                                return obj;
                            }
                        }
                        catch (Exception err)
                        {
                            connection.Close();
                            isSuccess = false;
                            errMessage = err.Message;
                            return null;
                        }
                    }
                }
            }
            catch (Exception err)
            {
                isSuccess = false;
                errMessage = err.Message;
                return null;
            }
        }

        /// <summary>
        /// 判断是否存在某表的某个字段
        /// 从系统视图syscolumns查询
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="columnName">列名称</param>
        /// <param name="isSuccess">查询成功返回true，失败返回false</param>
        /// <param name="errMessage">返回错误信息</param>
        /// <returns>存在返回true，不存在返回false</returns>
        public bool ColumnExists(string tableName, string columnName, out bool isSuccess, out string errMessage)
        {
            string sql = "select count(1) from syscolumns where [id]=object_id('" + tableName + "') and [name]='" + columnName + "'";
            object res = GetSingle(sql, out isSuccess, out errMessage);
            if (res == null)//不存在
            {
                return false;
            }
            return Convert.ToInt32(res) > 0;//存在
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="sqlStringList">多条SQL语句的泛型集合</param>
        /// <param name="isSuccess">执行成功返回true，失败返回false</param>
        /// <param name="errMessage">返回错误信息</param>
        /// <returns>返回影响的行数</returns>
        public int ExecuteSqlTran(List<string> sqlStringList, out bool isSuccess, out string errMessage)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    var cmd = new SqlCommand {Connection = conn};
                    var tx = conn.BeginTransaction();//开始一个数据库事务
                    cmd.Transaction = tx;
                    try
                    {
                        var count = 0;
                        foreach (var strsql in sqlStringList)
                        {
                            if (strsql.Trim().Length <= 1) continue;
                            cmd.CommandText = strsql;
                            count += cmd.ExecuteNonQuery();
                        }
                        tx.Commit();//提交事务
                        isSuccess = true;
                        errMessage = null;
                        return count;
                    }
                    catch (SqlException err)
                    {
                        tx.Rollback();//回滚事务
                        isSuccess = false;
                        errMessage = err.Message;
                        return 0;
                    }
                }
            }
            catch (Exception err)
            {
                isSuccess = false;
                errMessage = err.Message;
                return 0;
            }
        }

        /// <summary>
        /// 向数据库里插入图像格式的字段
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="fs">图像字节,数据库的字段类型为image的情况</param>
        /// <param name="isSuccess">执行成功返回true，失败返回false</param>
        /// <param name="errMessage">返回错误信息</param>
        /// <returns>影响的记录数</returns>
        public int ExecuteSqlInsertImg(string strSql, byte[] fs, out bool isSuccess, out string errMessage)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var cmd = new SqlCommand(strSql, connection);
                    var myParameter = new SqlParameter("@fs", SqlDbType.Image) {Value = fs};
                    cmd.Parameters.Add(myParameter);
                    try
                    {
                        connection.Open();
                        var rows = cmd.ExecuteNonQuery();
                        isSuccess = true;
                        errMessage = null;
                        return rows;
                    }
                    catch (SqlException err)
                    {
                        isSuccess = false;
                        errMessage = err.Message;
                        return 0;
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
            catch (Exception err)
            {
                isSuccess = false;
                errMessage = err.Message;
                return 0;
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="sqlString">查询语句</param>
        /// <param name="isSuccess">查询成功返回true，失败返回false</param>
        /// <param name="errMessage">返回错误信息</param>
        /// <returns>返回查询到的Dataet</returns>
        public DataSet Query(string sqlString, out bool isSuccess, out string errMessage)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var ds = new DataSet();
                    try
                    {
                        connection.Open();
                        var command = new SqlDataAdapter(sqlString, connection);
                        command.Fill(ds, "ds");
                        isSuccess = true;
                        errMessage = null;
                    }
                    catch (SqlException err)
                    {
                        isSuccess = false;
                        errMessage = err.Message;
                        return null;
                    }
                    return ds;
                }
            }
            catch (Exception err)
            {
                isSuccess = false;
                errMessage = err.Message;
                return null;
            }
        }

        /// <summary>
        /// 执行带参数的查询语句， 返回DataSet
        /// </summary>
        /// <param name="sqlString">查询字符串</param>
        /// <param name="isSuccess">查询成功返回true，失败返回false</param>
        /// <param name="errMessage">返回错误信息</param>
        /// <param name="cmdParms">参数列表</param>
        /// <returns></returns>
        public DataSet Query(string sqlString, out bool isSuccess, out string errMessage, params SqlParameter[] cmdParms)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var cmd = new SqlCommand();
                    PrepareCommand(cmd, connection, null, sqlString, out isSuccess, out errMessage, cmdParms);
                    if (isSuccess == false)//上句执行失败直接return 
                        return null;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        try
                        {
                            da.Fill(ds, "ds");
                            cmd.Parameters.Clear();
                        }
                        catch (SqlException err)
                        {
                            isSuccess = false;
                            errMessage = err.Message;
                            return null;
                        }
                        return ds;
                    }
                }
            }
            catch (Exception err)
            {
                isSuccess = false;
                errMessage = err.Message;
                return null;
            }
        }

        /// <summary>
        /// 执行存储过程，返回影响的行数		
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="isSuccess">执行成功返回true，失败返回false</param>
        /// <param name="errMessage">返回错误信息</param>
        /// <returns></returns>
        public int RunProcedure(string storedProcName, out bool isSuccess, out string errMessage, IDataParameter[] parameters)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        connection.Open();
                        var command = BuildIntCommand(connection, storedProcName, parameters);
                        var rowsAffected = command.ExecuteNonQuery();
                        isSuccess = true;
                        errMessage = null;
                        return rowsAffected;
                    }
                    catch (SqlException err)
                    {
                        isSuccess = false;
                        errMessage = err.Message;
                        return 0;
                    }
                }
            }
            catch (Exception err)
            {
                isSuccess = false;
                errMessage = err.Message;
                return 0;
            }
        }

        /// <summary>
        /// 执行存储过程 返回DataSet
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <param name="isSuccess">执行成功返回true，失败返回false</param>
        /// <param name="errMessage">返回错误信息</param>
        /// <returns>返回执行存储过程后的DataSet</returns>
        public DataSet RunProcedure(string storedProcName, string tableName, out bool isSuccess, out string errMessage, IDataParameter[] parameters)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var dataSet = new DataSet();
                    try
                    {
                        connection.Open();
                        var sqlDa = new SqlDataAdapter
                        {
                            SelectCommand = BuildQueryCommand(connection, storedProcName, parameters)
                        };
                        sqlDa.Fill(dataSet, tableName);
                        isSuccess = true;
                        errMessage = null;
                    }
                    catch (SqlException err)
                    {
                        connection.Close();
                        isSuccess = false;
                        errMessage = err.Message;
                    }
                    return dataSet;
                }
            }
            catch (Exception err)
            {
                isSuccess = false;
                errMessage = err.Message;
                return null;
            }
        }
        #endregion
    }
}