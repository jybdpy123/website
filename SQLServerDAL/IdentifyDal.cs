using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtil;
using Models;

namespace SQLServerDAL
{
    public class IdentifyDal
    {
        /// <summary>
        /// 私有方法 将数据表单行制作成模型
        /// </summary>
        /// <param name="dr">单个数据表行</param>
        /// <param name="isSuccess">执行成功返回true,失败返回false</param>
        /// <param name="errMessage">返回错误信息</param>
        /// <returns>返回制作完成的模型</returns>
        private static Identify DataRowToModel(DataRow dr, out bool isSuccess, out string errMessage)
        {
            var model = new Identify();
            try
            {
                if(dr["I_No"].ToString() != "")
                    model.No = int.Parse(dr["I_No"].ToString());
                model.Date = dr["I_Date"].ToString() != ""
                    ? DateTime.Parse(dr["I_Date"].ToString())
                    : System.Data.SqlTypes.SqlDateTime.MinValue.Value;
                model.IdentityCode = dr["I_Code"].ToString();
                isSuccess = true;
                errMessage = null;
                return model;
            }
            catch (Exception ex)
            {
                isSuccess = false;
                errMessage = ex.Message;
                return null;
            }
        }

        public bool Add(Identify model, out bool isSuccess, out string errMessage)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into Identify(");
            strSql.Append("I_No,I_Date,I_Code)");
            strSql.Append(" values(@I_No,@I_Date,@I_Code)");
            SqlParameter[] parameters =
            {
                new SqlParameter("@I_No", SqlDbType.Int),
                new SqlParameter("@I_Date", SqlDbType.DateTime),
                new SqlParameter("@I_Code", SqlDbType.VarChar, 10)
            };

            parameters[0].Value = model.No;
            parameters[1].Value = model.Date;
            parameters[2].Value = model.IdentityCode;

            return DbHelperSql.GetInstance()
                       .ExecuteSql(strSql.ToString(), out isSuccess, out errMessage, parameters) > 0;
        }

        public bool Delete(string identifyNo, out string errMessage)
        {
            throw new Exception("函数未实现");
            //StringBuilder strSql = new StringBuilder();
            //strSql.Append("delete from Users ");
            //strSql.Append(" where U_id=@Id ");
            //SqlParameter[] parameters = {
            //    new SqlParameter("@Id", SqlDbType.VarBinary,15)};
            //parameters[0].Value = UserId;
            //bool isSuccess;
            //DBHelperSQL.GetInstance().ExecuteSql(strSql.ToString(), out isSuccess, out err_message, parameters);
            //return isSuccess;
        }

        public bool LogicDelete(string identifyNo, out string errMessage)
        {
            throw new Exception("函数未实现");
            //Hospital.Model.Users_Model model;
            //bool isSuccess;
            //model = this.GetModel(UserId, out isSuccess, out err_message);
            //if (isSuccess == false)
            //    return false;
            //model.DeleteFlag = true;
            //return this.Update(model, out err_message);
        }

        public bool Exists(string identifyNo, out bool isSuccess, out string errMessage)
        {
            var strSql = new StringBuilder();
            strSql.Append("select count(1) from Identify");
            strSql.Append(" where I_No=@I_No ");
            SqlParameter[] parameters = {
                new SqlParameter("@I_No", SqlDbType.Int)};
            parameters[0].Value = identifyNo;
            return DbHelperSql.GetInstance().Exists(strSql.ToString(), out isSuccess, out errMessage, parameters);
        }

        public List<Identify> GetList(out bool isSuccess, out string errMessage)
        {
            return GetList("", 0, out isSuccess, out errMessage);
        }

        public List<Identify> GetList(string strWhere, out bool isSuccess, out string errMessage)
        {
            return GetList(strWhere, 0, out isSuccess, out errMessage);
        }

        public List<Identify> GetList(string strWhere, int num, out bool isSuccess, out string errMessage)
        {
            var ds = GetSet(strWhere, num, out isSuccess, out errMessage);
            if (isSuccess == false)
                return null;
            var list = new List<Identify>();
            foreach (DataRow i in ds.Tables[0].Rows)
            {
                list.Add(DataRowToModel(i, out isSuccess, out errMessage));
                if (isSuccess == false)
                    return null;
            }
            return list;
        }

        public Identify GetModel(string identifyNo, out bool isSuccess, out string errMessage)
        {
            var strSql = new StringBuilder();
            strSql.Append("select  top 1 * from Identify ");
            strSql.Append("where I_No = @I_No ");
            SqlParameter[] parameters = {
                new SqlParameter("@I_No", SqlDbType.Int)};
            parameters[0].Value = identifyNo;
            var ds = DbHelperSql.GetInstance().Query(strSql.ToString(), out isSuccess, out errMessage, parameters);
            if (isSuccess == false)//查询失败返回空
                return null;
            return ds.Tables[0].Rows.Count > 0
                ? DataRowToModel(ds.Tables[0].Rows[0], out isSuccess, out errMessage)
                : null;
        }

        public DataSet GetSet(out bool isSuccess, out string errMessage)
        {
            return GetSet("", 0, out isSuccess, out errMessage);
        }

        public DataSet GetSet(string strWhere, out bool isSuccess, out string errMessage)
        {
            return GetSet(strWhere, 0, out isSuccess, out errMessage);
        }

        public DataSet GetSet(string strWhere, int num, out bool isSuccess, out string errMessage)
        {
            var strSql = new StringBuilder();
            strSql.Append("select ");
            if (num > 0)
            {
                strSql.Append(" top " + num);
            }
            strSql.Append(" * ");
            strSql.Append(" FROM Identify ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSql.GetInstance().Query(strSql.ToString(), out isSuccess, out errMessage);
        }

        public bool Update(Identify model, out bool isSuccess, out string errMessage)
        {
            var strSql = new StringBuilder();
            strSql.Append("update Users set ");
            strSql.Append("I_Date = @I_Date,");
            strSql.Append("I_Code = @I_Code,");
            SqlParameter[] parameters = {
                new SqlParameter("@I_Date",SqlDbType.DateTime),
                new SqlParameter("@U_intro",SqlDbType.VarChar,10)};

            parameters[0].Value = model.Date;
            parameters[1].Value = model.IdentityCode;

            DbHelperSql.GetInstance().ExecuteSql(strSql.ToString(), out isSuccess, out errMessage, parameters);
            return isSuccess;
        }
    }

    public class IdentifyQuestion
    {
        private static Identify DataRowToModel(DataRow dr, out bool isSuccess, out string errMessage)
        {
            var model = new Identify();
            try
            {
                if (dr["I_No"].ToString() != "")
                    model.No = int.Parse(dr["I_No"].ToString());
                model.Date = dr["I_Date"].ToString() != ""
                    ? DateTime.Parse(dr["I_Date"].ToString())
                    : System.Data.SqlTypes.SqlDateTime.MinValue.Value;
                model.IdentityCode = dr["I_Code"].ToString();
                isSuccess = true;
                errMessage = null;
                return model;
            }
            catch (Exception ex)
            {
                isSuccess = false;
                errMessage = ex.Message;
                return null;
            }
        }

        public bool Add(Identify model, out bool isSuccess, out string errMessage)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into Identify(");
            strSql.Append("I_No,I_Date,I_Code)");
            strSql.Append(" values(@I_No,@I_Date,@I_Code)");
            SqlParameter[] parameters =
            {
                new SqlParameter("@I_No", SqlDbType.Int),
                new SqlParameter("@I_Date", SqlDbType.DateTime),
                new SqlParameter("@I_Code", SqlDbType.VarChar, 10)
            };

            parameters[0].Value = model.No;
            parameters[1].Value = model.Date;
            parameters[2].Value = model.IdentityCode;

            return DbHelperSql.GetInstance()
                       .ExecuteSql(strSql.ToString(), out isSuccess, out errMessage, parameters) > 0;
        }

        public bool Delete(string identifyNo, out string errMessage)
        {
            throw new Exception("函数未实现");
            //StringBuilder strSql = new StringBuilder();
            //strSql.Append("delete from Users ");
            //strSql.Append(" where U_id=@Id ");
            //SqlParameter[] parameters = {
            //    new SqlParameter("@Id", SqlDbType.VarBinary,15)};
            //parameters[0].Value = UserId;
            //bool isSuccess;
            //DBHelperSQL.GetInstance().ExecuteSql(strSql.ToString(), out isSuccess, out err_message, parameters);
            //return isSuccess;
        }

        public bool LogicDelete(string identifyNo, out string errMessage)
        {
            throw new Exception("函数未实现");
            //Hospital.Model.Users_Model model;
            //bool isSuccess;
            //model = this.GetModel(UserId, out isSuccess, out err_message);
            //if (isSuccess == false)
            //    return false;
            //model.DeleteFlag = true;
            //return this.Update(model, out err_message);
        }

        public bool Exists(string identifyNo, out bool isSuccess, out string errMessage)
        {
            var strSql = new StringBuilder();
            strSql.Append("select count(1) from Identify");
            strSql.Append(" where I_No=@I_No ");
            SqlParameter[] parameters = {
                new SqlParameter("@I_No", SqlDbType.Int)};
            parameters[0].Value = identifyNo;
            return DbHelperSql.GetInstance().Exists(strSql.ToString(), out isSuccess, out errMessage, parameters);
        }

        public List<Identify> GetList(out bool isSuccess, out string errMessage)
        {
            return GetList("", 0, out isSuccess, out errMessage);
        }

        public List<Identify> GetList(string strWhere, out bool isSuccess, out string errMessage)
        {
            return GetList(strWhere, 0, out isSuccess, out errMessage);
        }

        public List<Identify> GetList(string strWhere, int num, out bool isSuccess, out string errMessage)
        {
            var ds = GetSet(strWhere, num, out isSuccess, out errMessage);
            if (isSuccess == false)
                return null;
            var list = new List<Identify>();
            foreach (DataRow i in ds.Tables[0].Rows)
            {
                list.Add(DataRowToModel(i, out isSuccess, out errMessage));
                if (isSuccess == false)
                    return null;
            }
            return list;
        }

        public Identify GetModel(string identifyNo, out bool isSuccess, out string errMessage)
        {
            var strSql = new StringBuilder();
            strSql.Append("select  top 1 * from Identify ");
            strSql.Append("where I_No = @I_No ");
            SqlParameter[] parameters = {
                new SqlParameter("@I_No", SqlDbType.Int)};
            parameters[0].Value = identifyNo;
            var ds = DbHelperSql.GetInstance().Query(strSql.ToString(), out isSuccess, out errMessage, parameters);
            if (isSuccess == false)//查询失败返回空
                return null;
            return ds.Tables[0].Rows.Count > 0
                ? DataRowToModel(ds.Tables[0].Rows[0], out isSuccess, out errMessage)
                : null;
        }

        public DataSet GetSet(out bool isSuccess, out string errMessage)
        {
            return GetSet("", 0, out isSuccess, out errMessage);
        }

        public DataSet GetSet(string strWhere, out bool isSuccess, out string errMessage)
        {
            return GetSet(strWhere, 0, out isSuccess, out errMessage);
        }

        public DataSet GetSet(string strWhere, int num, out bool isSuccess, out string errMessage)
        {
            var strSql = new StringBuilder();
            strSql.Append("select ");
            if (num > 0)
            {
                strSql.Append(" top " + num);
            }
            strSql.Append(" * ");
            strSql.Append(" FROM Identify ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSql.GetInstance().Query(strSql.ToString(), out isSuccess, out errMessage);
        }

        public bool Update(Identify model, out bool isSuccess, out string errMessage)
        {
            var strSql = new StringBuilder();
            strSql.Append("update Users set ");
            strSql.Append("I_Date = @I_Date,");
            strSql.Append("I_Code = @I_Code,");
            SqlParameter[] parameters = {
                new SqlParameter("@I_Date",SqlDbType.DateTime),
                new SqlParameter("@U_intro",SqlDbType.VarChar,10)};

            parameters[0].Value = model.Date;
            parameters[1].Value = model.IdentityCode;

            DbHelperSql.GetInstance().ExecuteSql(strSql.ToString(), out isSuccess, out errMessage, parameters);
            return isSuccess;
        }
    }

    public class IdentityLog
    {
        private static Identify DataRowToModel(DataRow dr, out bool isSuccess, out string errMessage)
        {
            var model = new Identify();
            try
            {
                if (dr["I_No"].ToString() != "")
                    model.No = int.Parse(dr["I_No"].ToString());
                model.Date = dr["I_Date"].ToString() != ""
                    ? DateTime.Parse(dr["I_Date"].ToString())
                    : System.Data.SqlTypes.SqlDateTime.MinValue.Value;
                model.IdentityCode = dr["I_Code"].ToString();
                isSuccess = true;
                errMessage = null;
                return model;
            }
            catch (Exception ex)
            {
                isSuccess = false;
                errMessage = ex.Message;
                return null;
            }
        }

        public bool Add(Identify model, out bool isSuccess, out string errMessage)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into Identify(");
            strSql.Append("I_No,I_Date,I_Code)");
            strSql.Append(" values(@I_No,@I_Date,@I_Code)");
            SqlParameter[] parameters =
            {
                new SqlParameter("@I_No", SqlDbType.Int),
                new SqlParameter("@I_Date", SqlDbType.DateTime),
                new SqlParameter("@I_Code", SqlDbType.VarChar, 10)
            };

            parameters[0].Value = model.No;
            parameters[1].Value = model.Date;
            parameters[2].Value = model.IdentityCode;

            return DbHelperSql.GetInstance()
                       .ExecuteSql(strSql.ToString(), out isSuccess, out errMessage, parameters) > 0;
        }

        public bool Delete(string identifyNo, out string errMessage)
        {
            throw new Exception("函数未实现");
            //StringBuilder strSql = new StringBuilder();
            //strSql.Append("delete from Users ");
            //strSql.Append(" where U_id=@Id ");
            //SqlParameter[] parameters = {
            //    new SqlParameter("@Id", SqlDbType.VarBinary,15)};
            //parameters[0].Value = UserId;
            //bool isSuccess;
            //DBHelperSQL.GetInstance().ExecuteSql(strSql.ToString(), out isSuccess, out err_message, parameters);
            //return isSuccess;
        }

        public bool LogicDelete(string identifyNo, out string errMessage)
        {
            throw new Exception("函数未实现");
            //Hospital.Model.Users_Model model;
            //bool isSuccess;
            //model = this.GetModel(UserId, out isSuccess, out err_message);
            //if (isSuccess == false)
            //    return false;
            //model.DeleteFlag = true;
            //return this.Update(model, out err_message);
        }

        public bool Exists(string identifyNo, out bool isSuccess, out string errMessage)
        {
            var strSql = new StringBuilder();
            strSql.Append("select count(1) from Identify");
            strSql.Append(" where I_No=@I_No ");
            SqlParameter[] parameters = {
                new SqlParameter("@I_No", SqlDbType.Int)};
            parameters[0].Value = identifyNo;
            return DbHelperSql.GetInstance().Exists(strSql.ToString(), out isSuccess, out errMessage, parameters);
        }

        public List<Identify> GetList(out bool isSuccess, out string errMessage)
        {
            return GetList("", 0, out isSuccess, out errMessage);
        }

        public List<Identify> GetList(string strWhere, out bool isSuccess, out string errMessage)
        {
            return GetList(strWhere, 0, out isSuccess, out errMessage);
        }

        public List<Identify> GetList(string strWhere, int num, out bool isSuccess, out string errMessage)
        {
            var ds = GetSet(strWhere, num, out isSuccess, out errMessage);
            if (isSuccess == false)
                return null;
            var list = new List<Identify>();
            foreach (DataRow i in ds.Tables[0].Rows)
            {
                list.Add(DataRowToModel(i, out isSuccess, out errMessage));
                if (isSuccess == false)
                    return null;
            }
            return list;
        }

        public Identify GetModel(string identifyNo, out bool isSuccess, out string errMessage)
        {
            var strSql = new StringBuilder();
            strSql.Append("select  top 1 * from Identify ");
            strSql.Append("where I_No = @I_No ");
            SqlParameter[] parameters = {
                new SqlParameter("@I_No", SqlDbType.Int)};
            parameters[0].Value = identifyNo;
            var ds = DbHelperSql.GetInstance().Query(strSql.ToString(), out isSuccess, out errMessage, parameters);
            if (isSuccess == false)//查询失败返回空
                return null;
            return ds.Tables[0].Rows.Count > 0
                ? DataRowToModel(ds.Tables[0].Rows[0], out isSuccess, out errMessage)
                : null;
        }

        public DataSet GetSet(out bool isSuccess, out string errMessage)
        {
            return GetSet("", 0, out isSuccess, out errMessage);
        }

        public DataSet GetSet(string strWhere, out bool isSuccess, out string errMessage)
        {
            return GetSet(strWhere, 0, out isSuccess, out errMessage);
        }

        public DataSet GetSet(string strWhere, int num, out bool isSuccess, out string errMessage)
        {
            var strSql = new StringBuilder();
            strSql.Append("select ");
            if (num > 0)
            {
                strSql.Append(" top " + num);
            }
            strSql.Append(" * ");
            strSql.Append(" FROM Identify ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSql.GetInstance().Query(strSql.ToString(), out isSuccess, out errMessage);
        }

        public bool Update(Identify model, out bool isSuccess, out string errMessage)
        {
            var strSql = new StringBuilder();
            strSql.Append("update Users set ");
            strSql.Append("I_Date = @I_Date,");
            strSql.Append("I_Code = @I_Code,");
            SqlParameter[] parameters = {
                new SqlParameter("@I_Date",SqlDbType.DateTime),
                new SqlParameter("@U_intro",SqlDbType.VarChar,10)};

            parameters[0].Value = model.Date;
            parameters[1].Value = model.IdentityCode;

            DbHelperSql.GetInstance().ExecuteSql(strSql.ToString(), out isSuccess, out errMessage, parameters);
            return isSuccess;
        }
    }
}
