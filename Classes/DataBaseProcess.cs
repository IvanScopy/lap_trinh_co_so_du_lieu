using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using Oracle.ManagedDataAccess.Client;


namespace lap_trinh_co_so_du_lieu.Classes
{
    public class DataBaseProcess
    {
        // Chuỗi kết nối đến Oracle Database. Bạn cần thay thế các thông tin kết nối phù hợp.
        string strConnect = "Data Source=localhost:1521/ORCL;User Id=Project_user;Password=abc123";

        OracleConnection oracleConnect = null;

        // Phương thức mở kết nối
        void OpenConnect()
        {
            oracleConnect = new OracleConnection(strConnect);
            if (oracleConnect.State != ConnectionState.Open)
            {
                oracleConnect.Open();
            }
        }

        // Phương thức đóng kết nối
        void CloseConnect()
        {
            if (oracleConnect.State != ConnectionState.Closed)
            {
                oracleConnect.Close();
                oracleConnect.Dispose();
            }
        }

        // Phương thức thực thi câu lệnh Select, trả về một DataTable
        public DataTable DataReader(string sqlSelect)
        {
            DataTable tblData = new DataTable();
            OpenConnect();

            // Sử dụng OracleDataAdapter để thực thi câu lệnh SQL và điền dữ liệu vào DataTable
            OracleDataAdapter oracleDataAdapter = new OracleDataAdapter(sqlSelect, oracleConnect);
            oracleDataAdapter.Fill(tblData);

            CloseConnect();
            return tblData;
        }

        // Phương thức thực hiện câu lệnh dạng insert, update, delete
        public void DataChange(string sql)
        {
            OpenConnect();

            // Sử dụng OracleCommand để thực thi câu lệnh
            OracleCommand oracleCommand = new OracleCommand();
            oracleCommand.Connection = oracleConnect;
            oracleCommand.CommandText = sql;
            oracleCommand.ExecuteNonQuery();

            CloseConnect();
        }
    }
}
