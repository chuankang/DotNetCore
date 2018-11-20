using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DotNetCore.Common.Utils
{
    public class BulkUtil
    {
        public static ResultMsg Execute(IDictionary<string, DataTable> dics)
        {
            var resultMsg = new ResultMsg();
            using (var conn = DataBaseConfig.GetConnection())
            {
                var trans = conn.BeginTransaction();
                var sqlBulk = new SqlBulkCopy(conn, SqlBulkCopyOptions.KeepNulls, trans)
                {
                    BulkCopyTimeout = 600
                };
                try
                {
                    foreach (var dic in dics)
                    {
                        sqlBulk.DestinationTableName = dic.Key;
                        sqlBulk.WriteToServer(dic.Value);
                    }

                    trans.Commit();
                    resultMsg.MsgState = true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    resultMsg.MsgState = false;
                    resultMsg.MsgData = "操作失败,详细原因：" + ex.Message;
                }

                return resultMsg;
            }
        }
    }
}
