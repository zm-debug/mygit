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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {           
            LoadData();         
        }

        private void LoadData()
        {
            List<Class1> list = new List<Class1>();
            string constr = "Data Source=PC-181115SD;Initial Catalog=FirstDataBase;Integrated Security=True";
            string sql = "select * from Departments";
            //using (SqlConnection con=new SqlConnection(constr))
            //{
            //    string sql = "select * from Departments";
            //    using (SqlCommand com=new SqlCommand(sql,con))
            //    {
            //        con.Open();
            //        using (SqlDataReader reader=com.ExecuteReader())
            //        {
            //            if(reader.HasRows)
            //            {
            //                while (reader.Read())
            //                {
            //                    Class1 class1 = new Class1()
            //                    {
            //                        DepartmentID = reader.GetInt32(0),
            //                        DepartmentName = reader.GetString(1),
            //                        DepartmentDesc = reader.GetString(2)
            //                    };
            //                    list.Add(class1);

            //                }
            //            }
            //        }
            //    }
            //}
            using (SqlDataReader reader = SqlHelper.ExecuteReader(sql,System.Data.CommandType.Text,null))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Class1 class1 = new Class1()
                        {
                            DepartmentID = reader.GetInt32(0),
                            DepartmentName = reader.GetString(1),
                            DepartmentDesc = reader.GetString(2)
                        };
                        list.Add(class1);

                    }
                }
            }
            dg1.ItemsSource = list;
            //dg1.DataContext = list;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string DName = this.tb1.Text.Trim();
            string DDesc = this.tb2.Text.Trim();
            string constr = "Data Source=PC-181115SD;Initial Catalog=FirstDataBase;Integrated Security=True";
            //using (SqlConnection con = new SqlConnection(constr))
            //{
            //    string sql = string.Format("insert into Departments values('{0}','{1}')", DName, DDesc);
            //    using (SqlCommand com = new SqlCommand(sql, con))
            //    {
            //        con.Open();
            //        int r = com.ExecuteNonQuery();
            //        if(r>0)
            //        {
            //            this.Title = "插入成功！！";
            //            LoadData();
            //        }
            //    }
            //}
            string sql = "insert into Departments values( @dname, @ddesc)";
            SqlParameter[] parameters = new SqlParameter[] 
            {
                new SqlParameter("@dname",SqlDbType.NVarChar,50){Value=DName},
                new SqlParameter("@ddesc",SqlDbType.NVarChar,50 ){Value=DDesc}
            };
            int r = SqlHelper.ExecuteNonQuery(sql, CommandType.Text, parameters);
            if (r > 0)
            {
                this.Title = "插入成功！！";
                LoadData();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Class1 cl = new Class1()
            {
                DepartmentID = Convert.ToInt16(this.lb1.Content.ToString()),
                DepartmentName = this.tb3.Text.Trim(),
                DepartmentDesc = this.tb4.Text.Trim()
            };
            //string constr = "Data Source=PC-181115SD;Initial Catalog=FirstDataBase;Integrated Security=True";
            //using (SqlConnection con = new SqlConnection(constr))
            //{
            //    string sql = string.Format("update Departments set DepartmentName='{0}',DepartmentDesc='{1}' where DepartmentID={2}", cl.DepartmentName, cl.DepartmentDesc,cl.DepartmentID);
            //    using (SqlCommand com = new SqlCommand(sql, con))
            //    {
            //        con.Open();
            //        int r = com.ExecuteNonQuery();
            //        if (r > 0)
            //        {
            //            this.Title = "更新成功！！";
            //            LoadData();
            //        }
            //    }
            //}
            string sql = "update Departments set DepartmentName=@dname,DepartmentDesc=@ddesc where DepartmentID=@ddid";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@dname",SqlDbType.NVarChar,50){Value=cl.DepartmentName},
                new SqlParameter("@ddesc",SqlDbType.NVarChar,50 ){Value=cl.DepartmentDesc},
                new SqlParameter("@ddid",SqlDbType.Int){Value=cl.DepartmentID}
            };
            int r = SqlHelper.ExecuteNonQuery(sql, CommandType.Text, parameters);
            if (r > 0)
            {
                this.Title = "更新成功！！";
                LoadData();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result= System.Windows.MessageBox.Show("确定要删除吗？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if(result== MessageBoxResult.OK)
            {
                if (dg1.SelectedItem!=null)
                {
                    int id = Convert.ToInt16(this.lb1.Content.ToString());
                    //string constr = "Data Source=PC-181115SD;Initial Catalog=FirstDataBase;Integrated Security=True";
                    //using (SqlConnection con = new SqlConnection(constr))
                    //{
                    //    string sql = string.Format("delete from Departments where DepartmentID={0}", id);
                    //    using (SqlCommand com = new SqlCommand(sql, con))
                    //    {
                    //        con.Open();
                    //        int r = com.ExecuteNonQuery();
                    //        if (r > 0)
                    //        {
                    //            this.Title = "删除成功！！";
                    //            LoadData();
                    //        }
                    //    }
                    //}
                    string sql = "delete from Departments where DepartmentID= @ddid";
                    SqlParameter[] parameters = new SqlParameter[]
                    {
                       new SqlParameter("@ddid",SqlDbType.Int){Value=id}
                    };
                    int r = SqlHelper.ExecuteNonQuery(sql, CommandType.Text, parameters);
                    if (r > 0)
                    {
                        this.Title = "删除成功！！";
                        LoadData();
                    }
                }
                else
                {
                    this.Title = "选择要删除的项！！";
                }
                
            }
           
        }
    }
}
