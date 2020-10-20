using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;

namespace WpfApp1
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }
        private int pageSize = 7;
        private int pageIndex = 1;
        private int recordCount;
        private int pageCount;
        private void LoadData()
        {
            string constr = "Data Source=PC-181115SD;Initial Catalog=Departments;Integrated Security=True";
            DataTable dt = new DataTable();
            using (SqlDataAdapter adapter=new SqlDataAdapter("usp_getPage",constr))
            {
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@pagesize",SqlDbType.Int){Value=pageSize},
                    new SqlParameter("@pageindex",SqlDbType.Int){Value=pageIndex},
                    new SqlParameter("@recordcount",SqlDbType.Int){Direction=ParameterDirection.Output},
                    new SqlParameter("@pagecount",SqlDbType.Int){Direction=ParameterDirection.Output},
                };
                adapter.SelectCommand.Parameters.AddRange(parameters);
                adapter.Fill(dt);
                lb1.Content = "总数：" + parameters[2].Value.ToString();
                lb2.Content = "总页数：" + parameters[3].Value.ToString();
                lb3.Content = "当前页：" + pageIndex;
                this.dg1.ItemsSource = dt.AsDataView();
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            pageIndex++;
            LoadData();
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            pageIndex++;
            LoadData();
        }
    }
}
