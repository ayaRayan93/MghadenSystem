﻿using DevExpress.XtraGrid.Views.Grid;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainSystem
{
    public partial class GetDelegateProfit : Form
    {
        private MySqlConnection dbconnection;
        public bool loaded = false;
        public GetDelegateProfit()
        {
            try
            {
                InitializeComponent();
                dbconnection = new MySqlConnection(connection.connectionString);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetDelegateProfit_Load(object sender, EventArgs e)
        {
            try
            {
                dbconnection.Open();

                string query = "select * from delegate ";

                MySqlDataAdapter da = new MySqlDataAdapter(query, dbconnection);
                DataTable dt = new DataTable();
            
                da.Fill(dt);
                comDelegate.DataSource = dt;
                comDelegate.DisplayMember = dt.Columns["Delegate_Name"].ToString();
                comDelegate.ValueMember = dt.Columns["Delegate_ID"].ToString();
                comDelegate.Text = "";
                txtDelegateID.Text = "";

                loaded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbconnection.Close();
        }
  
        private void comDelegate_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (loaded)
                {
                    try
                    {
                        txtDelegateID.Text = comDelegate.SelectedValue.ToString();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
 
        private void txtDelegateID_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    dbconnection.Open();
                    string query = "select Delegate_Name from delegate where Delegate_ID=" + txtDelegateID.Text ;
                    MySqlCommand com = new MySqlCommand(query, dbconnection);
                    if (com.ExecuteScalar() != null)
                    {
                        Name = (string)com.ExecuteScalar();
                        comDelegate.Text = Name;
                        query = "select Date_To from Delegates_Profit where Delegate_ID=" + txtDelegateID.Text ;
                        com = new MySqlCommand(query, dbconnection);
                        if (com.ExecuteScalar() != null)
                        {
                           // dateTimeFrom.Value.Date = (com.ExecuteScalar().ToString();
                        }
                    }
                    else
                    {
                        MessageBox.Show("there is no item with this id");
                        dbconnection.Close();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbconnection.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                dbconnection.Open();
                //peraper datasource
                DataTable _Table = peraperDataTable2();
                DataTable _Table2 = peraperDataTable();

                DateTime date = dateTimeFrom.Value;
                string d = date.ToString("yyyy-MM-dd HH:mm:ss");
                DateTime date2 = dateTimeTo.Value;
                string d2 = date2.ToString("yyyy-MM-dd HH:mm:ss");
                string itemName = "concat( product.Product_Name,' ',type.Type_Name,' ',factory.Factory_Name,' ',groupo.Group_Name,' ' ,COALESCE(color.Color_Name,''),' ',COALESCE(size.Size_Value,''),' ',COALESCE(sort.Sort_Value,''),' ',COALESCE(data.Classification,''),' ',COALESCE(data.Description,''))as 'البند'";
                string DataTableRelations = "INNER JOIN type ON type.Type_ID = data.Type_ID INNER JOIN product ON product.Product_ID = data.Product_ID INNER JOIN factory ON data.Factory_ID = factory.Factory_ID INNER JOIN groupo ON data.Group_ID = groupo.Group_ID LEFT outer JOIN color ON data.Color_ID = color.Color_ID LEFT outer  JOIN size ON data.Size_ID = size.Size_ID LEFT outer  JOIN sort ON data.Sort_ID = sort.Sort_ID";


                _Table = getTotalSoldProfit(_Table, DataTableRelations, d, d2);
                _Table = getTotalReturnedProfit(_Table, DataTableRelations, d, d2);

                gridControl2.DataSource = _Table;

                _Table2 = getSoldQuantity(_Table2, itemName, DataTableRelations, d, d2);
                _Table2 = getReturnedQuantity(_Table2, itemName, DataTableRelations, d, d2);

                gridControl1.DataSource = _Table2;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("حددالمندوب");
                txtDelegateID.Focus();
            }
            dbconnection.Close();
        }
        private void gridView2_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (e.Column.Name == "PercentageDelegate")
                {
                    GridView view = (GridView)sender;
                    DataRow dataRow = view.GetFocusedDataRow();
                    double oldValue = Convert.ToDouble(dataRow["ValueDelegate"].ToString());
                    double totalValue = Convert.ToDouble(labTotalDelegateProfit.Text);
                    totalValue -= oldValue;
                    double cellValue = Convert.ToDouble(e.Value);
                    double totalItemValue = Convert.ToDouble(dataRow["Quantity"].ToString());
                    double re = cellValue * totalItemValue;

                    view.SetRowCellValue(view.GetSelectedRows()[0], "ValueDelegate", re);

                    labTotalDelegateProfit.Text = (cellValue * totalItemValue) + totalValue + "";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void gridView1_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            try
            {
                GridView view = (GridView)sender;
                DataRow dataRow = view.GetFocusedDataRow();
                double total = 0.0;
                double result = Convert.ToDouble(labTotalDelegateProfit.Text);
                total = Convert.ToDouble(dataRow["ValueDelegate"].ToString());
                if (view.GetFocusedValue().ToString() == "True")
                {
                    result = result - total;
                }
                else
                {
                    result = result + total;
                }

                labTotalDelegateProfit.Text = result.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                dbconnection.Open();
                if (txtDelegateID.Text != "" && labTotalDelegateProfit.Text != "")
                {
                    string query = "INSERT INTO Delegates_Profit (Delegate_ID,Delegate_Profit, Date_From, Date_To) VALUES(@Delegate_ID,@Delegate_Profit, @Date_From, @Date_To)";
                    MySqlCommand command = new MySqlCommand(query, dbconnection);

                    command.Parameters.Add("@Delegate_ID", MySqlDbType.Int16);
                    command.Parameters["@Delegate_ID"].Value = txtDelegateID.Text;
                    command.Parameters.Add("@Delegate_Profit", MySqlDbType.Double);
                    command.Parameters["@Delegate_Profit"].Value = labTotalDelegateProfit.Text;
                    command.Parameters.Add("@Date_From", MySqlDbType.Date);
                    command.Parameters["@Date_From"].Value = dateTimeFrom.Value.Date;
                    command.Parameters.Add("@Date_To", MySqlDbType.Date);
                    command.Parameters["@Date_To"].Value = dateTimeTo.Value.Date;

                    command.ExecuteNonQuery();
                    reset();
                }
                else
                {
                    MessageBox.Show("ادخل البيانات بالكامل");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbconnection.Close();
        }
        private void newChoose_Click(object sender, EventArgs e)
        {
            try
            {
                txtDelegateID.Text = "";
                comDelegate.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //functions
        public DataTable getSoldQuantity(DataTable _Table, string itemName, string DataTableRelations, string dateFrom, string dateTo)
        {
            string query = "select product_bill.Data_ID," + itemName + ",sum(Quantity) as 'الكمية',PriceAD,PercentageDelegate from customer_bill inner join transitions on customer_bill.Branch_BillNumber=transitions.Bill_Number inner join product_bill on customer_bill.CustomerBill_ID=product_bill.CustomerBill_ID inner join sellprice on sellprice.Data_ID=product_bill.Data_ID inner join data on data.Data_ID=product_bill.Data_ID " + DataTableRelations + " inner join delegate on delegate.Delegate_ID=product_bill.Delegate_ID  where Paid_Status=1 and transitions.Date between '" + dateFrom + "' and '" + dateTo + "' and product_bill.Delegate_ID=" + txtDelegateID.Text + "  group by data.Code ";
            MySqlCommand com = new MySqlCommand(query, dbconnection);
            MySqlDataReader dr = com.ExecuteReader();

            while (dr.Read())
            {
                if (Convert.ToDouble(dr[4].ToString()) == 0)
                {
                    DataRow row = _Table.NewRow();

                    row["Data_ID"] = dr[0].ToString();
                    row["CodeName"] = dr[1].ToString();
                    row["PercentageDelegate"] = dr[4].ToString();
                    row["Quantity"] = dr[2].ToString();
                    row["Cost"] = Convert.ToDouble(dr[2].ToString()) * Convert.ToDouble(dr[3].ToString());
                    double str = Convert.ToDouble(dr[2].ToString()) * Convert.ToDouble(dr[3].ToString());

                    row["ValueDelegate"] = Convert.ToDouble(dr[4].ToString()) / 100 * str;
                    _Table.Rows.Add(row);
                }
            }
            dr.Close();

            return _Table;
        }
        public DataTable getReturnedQuantity(DataTable _Table, string itemName, string DataTableRelations, string dateFrom, string dateTo)
        {
            string query = "select customer_return_bill_details.Data_ID," + itemName + ",sum(TotalMeter) as 'الكمية',PriceAD,PercentageDelegate from customer_return_bill inner join customer_return_bill_details on customer_return_bill.CustomerReturnBill_ID=customer_return_bill_details.CustomerReturnBill_ID inner join sellprice on sellprice.Data_ID=customer_return_bill_details.Data_ID inner join data on data.Data_ID=customer_return_bill_details.Data_ID " + DataTableRelations + " inner join delegate on delegate.Delegate_ID=customer_return_bill_details.Delegate_ID  where customer_return_bill.Date between '" + dateFrom + "' and '" + dateTo + "' and customer_return_bill_details.Delegate_ID=" + txtDelegateID.Text + " group by data.Code ";
            MySqlCommand com = new MySqlCommand(query, dbconnection);
            MySqlDataReader dr = com.ExecuteReader();
            DataTable temp = peraperDataTable();
            bool flag = true;
            while (dr.Read())
            {
                if (Convert.ToDouble(dr[4].ToString()) == 0)
                {
                    foreach (DataRow item in _Table.Rows)
                    {
                        if (item[0].ToString() == dr[0].ToString())
                        {
                            item["Quantity"] = (Convert.ToDouble(item[2].ToString()) - Convert.ToDouble(dr[2].ToString())).ToString();
                            item["Cost"] = Convert.ToDouble(item["Quantity"].ToString()) * Convert.ToDouble(dr[3].ToString());
                            double str = Convert.ToDouble(item["Quantity"].ToString()) * Convert.ToDouble(dr[3].ToString());

                            item["ValueDelegate"] = Convert.ToDouble(dr[4].ToString()) / 100 * str;

                            flag = false;
                        }

                    }
                    if (flag)
                    {
                        DataRow row = temp.NewRow();

                        row["PercentageDelegate"] = dr[4].ToString();
                        row["Quantity"] = dr[2].ToString();
                        row["Data_ID"] = dr[0].ToString();
                        row["CodeName"] = dr[1].ToString();
                        row["Cost"] = Convert.ToDouble(dr[2].ToString()) * Convert.ToDouble(dr[3].ToString());
                        double str = Convert.ToDouble(dr[2].ToString()) * Convert.ToDouble(dr[3].ToString());
                        row["ValueDelegate"] = Convert.ToDouble(dr[4].ToString()) / 100 * str;

                        temp.Rows.Add(row);
                    }
                }

            }
            dr.Close();
            foreach (DataRow item in temp.Rows)
            {
                _Table.Rows.Add(item.ItemArray);
            }

            return _Table;
        }

        public DataTable peraperDataTable()
        {
            DataTable _Table = new DataTable("Table");

            _Table.Columns.Add(new DataColumn("Data_ID", typeof(int)));
            _Table.Columns.Add(new DataColumn("CodeName", typeof(string)));
            _Table.Columns.Add(new DataColumn("PercentageDelegate", typeof(string)));
            _Table.Columns.Add(new DataColumn("ValueDelegate", typeof(string)));
            _Table.Columns.Add(new DataColumn("Cost", typeof(string)));
            _Table.Columns.Add(new DataColumn("Quantity", typeof(string)));
            return _Table;
        }
        //total delegate profit for each company which its items have delegate_percentage 
        public DataTable peraperDataTable2()
        {
            DataTable _Table = new DataTable("Table2");

            _Table.Columns.Add(new DataColumn("FactoryID", typeof(int)));
            _Table.Columns.Add(new DataColumn("FactoryName", typeof(string)));
            _Table.Columns.Add(new DataColumn("PercentageDelegate", typeof(string)));
            _Table.Columns.Add(new DataColumn("ValueDelegate", typeof(string)));
            _Table.Columns.Add(new DataColumn("Cost", typeof(string)));
            _Table.Columns.Add(new DataColumn("Quantity", typeof(string)));
            return _Table;
        }
        public DataTable getTotalSoldProfit(DataTable _Table, string DataTableRelations, string dateFrom, string dateTo)
        {
            string query = "select data.Factory_ID,factory.Factory_Name,sum(Quantity) as 'الكمية',PriceAD,PercentageDelegate from customer_bill inner join transitions on customer_bill.Branch_BillNumber=transitions.Bill_Number inner join product_bill on customer_bill.CustomerBill_ID=product_bill.CustomerBill_ID inner join sellprice on sellprice.Data_ID=product_bill.Data_ID inner join data on data.Data_ID=product_bill.Data_ID " + DataTableRelations + " inner join delegate on delegate.Delegate_ID=product_bill.Delegate_ID  where Paid_Status=1 and transitions.Date between '" + dateFrom + "' and '" + dateTo + "' and product_bill.Delegate_ID=" + txtDelegateID.Text + "  group by data.Factory_ID ";
            MySqlCommand com = new MySqlCommand(query, dbconnection);
            MySqlDataReader dr = com.ExecuteReader();

            while (dr.Read())
            {
                if (Convert.ToDouble(dr[4].ToString()) == 0)
                {
                    DataRow row = _Table.NewRow();

                    row["FactoryID"] = dr[0].ToString();
                    row["FactoryName"] = dr[1].ToString();
                    row["PercentageDelegate"] = dr[4].ToString();
                    row["Quantity"] = dr[2].ToString();
                    row["Cost"] = Convert.ToDouble(dr[2].ToString()) * Convert.ToDouble(dr[3].ToString());
                    double str = Convert.ToDouble(dr[2].ToString()) * Convert.ToDouble(dr[3].ToString());

                    row["ValueDelegate"] = Convert.ToDouble(dr[4].ToString()) / 100 * str;
                    _Table.Rows.Add(row);
                }
            }
            dr.Close();

            return _Table;
        }
        public DataTable getTotalReturnedProfit(DataTable _Table, string DataTableRelations, string dateFrom, string dateTo)
        {
            string query = "select data.Factory_ID,factory.Factory_Name,sum(TotalMeter) as 'الكمية',PriceAD,PercentageDelegate from customer_return_bill inner join customer_return_bill_details on customer_return_bill.CustomerReturnBill_ID=customer_return_bill_details.CustomerReturnBill_ID inner join sellprice on sellprice.Data_ID=customer_return_bill_details.Data_ID inner join data on data.Data_ID=customer_return_bill_details.Data_ID " + DataTableRelations + " inner join delegate on delegate.Delegate_ID=customer_return_bill_details.Delegate_ID  where customer_return_bill.Date between '" + dateFrom + "' and '" + dateTo + "' and customer_return_bill_details.Delegate_ID=" + txtDelegateID.Text + " group by data.Factory_ID ";
            MySqlCommand com = new MySqlCommand(query, dbconnection);
            MySqlDataReader dr = com.ExecuteReader();
            DataTable temp = peraperDataTable2();
            bool flag = true;
            double result = 0.0;
            while (dr.Read())
            {
                if (Convert.ToDouble(dr[4].ToString()) > 0)
                {
                    foreach (DataRow item in _Table.Rows)
                    {
                        if (item[0].ToString() == dr[0].ToString())
                        {
                            item["Quantity"] = (Convert.ToDouble(item[2].ToString()) - Convert.ToDouble(dr[2].ToString())).ToString();
                            item["Cost"] = Convert.ToDouble(item["Quantity"].ToString()) * Convert.ToDouble(dr[3].ToString());
                            double str = Convert.ToDouble(item["Quantity"].ToString()) * Convert.ToDouble(dr[3].ToString());

                            item["ValueDelegate"] = Convert.ToDouble(dr[4].ToString()) / 100 * str;
                            result += Convert.ToDouble(dr[4].ToString()) / 100 * str;
                            flag = false;
                        }

                    }
                    if (flag)
                    {
                        DataRow row = temp.NewRow();

                        row["PercentageDelegate"] = dr[4].ToString();
                        row["Quantity"] = dr[2].ToString();
                        row["FactoryID"] = dr[0].ToString();
                        row["FactoryName"] = dr[1].ToString();
                        row["Cost"] = Convert.ToDouble(dr[2].ToString()) * Convert.ToDouble(dr[3].ToString());
                        double str = Convert.ToDouble(dr[2].ToString()) * Convert.ToDouble(dr[3].ToString());
                        row["ValueDelegate"] = Convert.ToDouble(dr[4].ToString()) / 100 * str;
                        result += Convert.ToDouble(dr[4].ToString()) / 100 * str;
                        temp.Rows.Add(row);
                    }
                }

            }
            dr.Close();
            foreach (DataRow item in temp.Rows)
            {
                _Table.Rows.Add(item.ItemArray);
            }
            labTotalDelegateProfit.Text = result.ToString();
            return _Table;
        }

        public void reset()
        {
            dateTimeFrom.Value = DateTime.Now;
            dateTimeTo.Value = DateTime.Now;
            txtDelegateID.Text = "";
            comDelegate.Text = "";
            gridControl1.DataSource = null;
            gridControl2.DataSource = null;
        }
    }
}
