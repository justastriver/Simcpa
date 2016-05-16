using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ADOX;
using ISimcpa.Util;
using System.Collections;
using System.IO;

namespace ISimcpa.DB
{
    public class OrderDetail
    {
        public string id;
        public string order_id;
        public string date;
        public string status;
        public string message;
    }
    public class Storage
    {
        private static ADOX.Catalog catalog = new Catalog();
        public static bool CreateDB(string db_file_name = "../../Data/Simcpa.mdb")//.mdb
        {
            if (File.Exists(db_file_name))
            {
                Logger.Debug("数据库已经存在: " + db_file_name);
                return true;
            }
            try
            {
                catalog.Create("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + db_file_name + ";Jet OLEDB:Engine Type=5");
            }
            catch (System.Exception ex)
            {
                Logger.Error("创建数据库失败：" + ex.Message);
                return false;
            }
            return true;
        }
        public static bool CreateTable(string db_file_name="../../Data/Simcpa.mdb", string table_name = "Simcpa")
        {
            try
            {
                ADODB.Connection cn = new ADODB.Connection();

                cn.Open("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + db_file_name, null, null, -1);
                catalog.ActiveConnection = cn;

                ADOX.Table table = new ADOX.Table();
                table.Name = table_name;

                ADOX.Column column = new ADOX.Column();
                column.ParentCatalog = catalog;
                column.Name = "id";
                column.Type = DataTypeEnum.adInteger;
                column.DefinedSize = 9;
                column.Properties["AutoIncrement"].Value = true;
                table.Columns.Append(column, DataTypeEnum.adInteger, 9);
                table.Keys.Append("FirstTablePrimaryKey", KeyTypeEnum.adKeyPrimary, column, null, null);
                table.Columns.Append("order_id", DataTypeEnum.adVarWChar, 50);
                table.Columns.Append("detail", DataTypeEnum.adLongVarWChar);
                table.Columns.Append("status", DataTypeEnum.adInteger, 2);
                table.Columns.Append("created_at", DataTypeEnum.adDate, 0);
                catalog.Tables.Append(table);

                cn.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                Logger.Error("创建数据库表 出错：" + ex.Message);
                return false;
            }
            
        }
        public static bool InsertOrder(string order_id, int err_count, string detail, string db_file_name = "../../Data/Simcpa.mdb", string table_name = "Simcpa")
        {
            try
            {
                ADODB.Connection cn = new ADODB.Connection();
                cn.Open("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + db_file_name, null, null, -1);

                ADODB.Recordset rs;
                rs = new ADODB.Recordset();
                int status = err_count;
                DateTime dt = DateTime.Now;
                string sql = "insert into " + table_name + " (order_id, detail,status,created_at) values('"
                    + order_id + "','"
                    + detail + "','"
                    + status.ToString() + "','"
                    + dt.ToLocalTime().ToString()
                    + "');";
                rs.Open(sql, cn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText);

                //rs.Close();
                cn.Close();
            }
            catch (System.Exception ex)
            {
                Logger.Error("InsertOrder 出错：" + ex.Message);
                return false;
            }
            return true;

        }

       
        public static ArrayList GetTodayOrder(string db_file_name = "../../Data/Simcpa.mdb", string table_name = "Simcpa")
        {
            ArrayList orders = new ArrayList();
            try
            {
                ADODB.Connection cn = new ADODB.Connection();
                cn.Open("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + db_file_name, null, null, -1);
                ADODB.Recordset rs;
                rs = new ADODB.Recordset();
                string sql = "select * from " + table_name;
                rs.Open(sql, cn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText);
                //如果记录集为空，输出一个错误信息
                if (rs.BOF || rs.EOF)
                {
                    Console.WriteLine("没有找到任何记录，请检查你的sqlserver的表！");
                    Logger.Debug("数据库为空");
                }
                while (!rs.EOF)
                {
                    OrderDetail detail = new OrderDetail();
                    detail.id = rs.Fields["id"].Value.ToString();
                    detail.order_id = rs.Fields["order_id"].Value.ToString();
                    detail.date = rs.Fields["created_at"].Value.ToString();
                    detail.status = rs.Fields["status"].Value.ToString();
                    detail.message = rs.Fields["detail"].Value.ToString();
                    //Logger.Debug(rs.Fields["order_id"].Value.ToString());
                    orders.Add(detail);
                    rs.MoveNext();
                }

                //rs.Close();
                cn.Close();
            }
            catch (System.Exception ex)
            {
                Logger.Error("获取订单数据 出错：" + ex.Message);
            }
            

            return orders;
        }

        public static bool Clear(string db_file_name = "../../Data/Simcpa.mdb", string table_name = "Simcpa")
        {
            
            try
            {
                ADODB.Connection cn = new ADODB.Connection();
                cn.Open("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + db_file_name, null, null, -1);
               
                string sql = "delete * from " + table_name;
                object dummy;
                cn.Execute(sql, out dummy, -1);
                
                cn.Close();
            }
            catch (System.Exception ex)
            {
                Logger.Error("获取订单数据 出错：" + ex.Message);
                return false;
            }


            return true;
        }
    }
}
