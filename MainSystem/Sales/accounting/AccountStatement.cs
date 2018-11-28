﻿using DevExpress.XtraTab;
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
    public partial class AccountStatement : Form
    {
        MySqlConnection dbconnection;
        MySqlConnection dbconnection1;
        MainForm saleMainForm;
        private string Customer_Type;
        private bool loaded = false;
        double safay = -1;
        int ClientID = -1;
        string ClientName = "";
        public AccountStatement(MainForm mainForm)
        {
            InitializeComponent();
            saleMainForm = mainForm;
            dbconnection = new MySqlConnection(connection.connectionString);
            dbconnection1 = new MySqlConnection(connection.connectionString);
            labelClient.Visible = false;
            labelEng.Visible = false;//label of مهندس/مقاول
            comClient.Visible = false;
            comEngCon.Visible = false;
            txtCustomerID.Visible = false;
            txtClientID.Visible = false;

        }
        //check type of customer if engineer,client or contract 
        private void radiotype_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radio = (RadioButton)sender;
            Customer_Type = radio.Text;
            comClient.Text = "";
            txtClientID.Text = "";
            comEngCon.Text = "";
            txtCustomerID.Text = "";

            loaded = false; //this is flag to prevent action of SelectedValueChanged event until datasource fill combobox
            try
            {
                if (Customer_Type == "عميل")
                {
                    labelEng.Visible = false;
                    comEngCon.Visible = false;
                    txtCustomerID.Visible = false;
                    labelClient.Visible = true;
                    comClient.Visible = true;
                    txtClientID.Visible = true;

                    string query = "select * from customer where Customer_Type='" + Customer_Type + "'";
                    MySqlDataAdapter da = new MySqlDataAdapter(query, dbconnection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    comClient.DataSource = dt;
                    comClient.DisplayMember = dt.Columns["Customer_Name"].ToString();
                    comClient.ValueMember = dt.Columns["Customer_ID"].ToString();
                    comClient.Text = "";
                    txtClientID.Text = "";
                }
                else
                {
                    labelEng.Visible = true;
                    comEngCon.Visible = true;
                    txtCustomerID.Visible = true;
                    labelClient.Visible = false;
                    comClient.Visible = false;
                    txtClientID.Visible = false;

                    string query = "select * from customer where Customer_Type='" + Customer_Type + "'";
                    MySqlDataAdapter da = new MySqlDataAdapter(query, dbconnection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    comEngCon.DataSource = dt;
                    comEngCon.DisplayMember = dt.Columns["Customer_Name"].ToString();
                    comEngCon.ValueMember = dt.Columns["Customer_ID"].ToString();
                    comEngCon.Text = "";
                    txtCustomerID.Text = "";
                }

                loaded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbconnection.Close();
        }
        private void comClient_SelectedValueChanged(object sender, EventArgs e)
        {
            if (loaded)
            {
                try
                {
                    txtClientID.Text = comClient.SelectedValue.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        //when select customer(مهندس,مقاول)display in comCustomer the all clients of th customer 
        private void comEngCon_SelectedValueChanged(object sender, EventArgs e)
        {
            if (loaded)
            {
                try
                {
                    loaded = false;
                    txtCustomerID.Text = comEngCon.SelectedValue.ToString();
                    labelClient.Visible = true;
                    comClient.Visible = true;
                    txtClientID.Visible = true;

                    string query = "select * from customer where Customer_ID in(select Client_ID from custmer_client where Customer_ID=" + comEngCon.SelectedValue + ")";
                    MySqlDataAdapter da = new MySqlDataAdapter(query, dbconnection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    comClient.DataSource = dt;
                    comClient.DisplayMember = dt.Columns["Customer_Name"].ToString();
                    comClient.ValueMember = dt.Columns["Customer_ID"].ToString();
                    comClient.Text = "";
                    loaded = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void txtBox_KeyDown(object sender, KeyEventArgs e)
        {

            TextBox txtBox = (TextBox)sender;
            string query;
            MySqlCommand com;
            string Name;
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    if (txtBox.Text != "")
                    {
                        dbconnection.Open();
                        switch (txtBox.Name)
                        {
                            case "txtCustomerID":
                                query = "select Customer_Name from customer where Customer_ID=" + txtCustomerID.Text + "";
                                com = new MySqlCommand(query, dbconnection);
                                if (com.ExecuteScalar() != null)
                                {
                                    Name = (string)com.ExecuteScalar();
                                    comEngCon.Text = Name;
                                    comEngCon.SelectedValue = txtCustomerID.Text;

                                }
                                else
                                {
                                    MessageBox.Show("there is no item with this id");
                                    dbconnection.Close();
                                    return;
                                }
                                break;
                            case "txtClientID":
                                query = "select Customer_Name from customer where Customer_ID=" + txtClientID.Text + "";
                                com = new MySqlCommand(query, dbconnection);
                                if (com.ExecuteScalar() != null)
                                {
                                    Name = (string)com.ExecuteScalar();
                                    comClient.Text = Name;
                                    comClient.SelectedValue = txtClientID.Text;
                                }
                                else
                                {
                                    MessageBox.Show("there is no item with this id");
                                    dbconnection.Close();
                                    return;
                                }
                                break;
                        }

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                dbconnection.Close();
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                dbconnection.Open();
                dbconnection1.Open();
                dataGridView1.Rows.Clear();
                dataGridView2.Rows.Clear();
                if (txtClientID.Text != "" || txtCustomerID.Text != "")
                {
                    int.TryParse(txtClientID.Text, out ClientID);
                    ClientName = comClient.Text;
                    displayBill();
                    displayReturnBill(dataGridView1);
                    displayPaidReturnBill();
                    displayPaidBill();
                    double totalBill = 0, TotalReturn = 0, rest = 0, rest2 = 0;

                    foreach (DataGridViewRow row1 in dataGridView1.Rows)
                    {
                        totalBill += Convert.ToDouble(row1.Cells[6].Value);
                        TotalReturn += Convert.ToDouble(row1.Cells[5].Value);
                        rest = totalBill - TotalReturn;
                    }

                    labTotalBillCost.Text = totalBill.ToString();
                    labTotalReturnCost.Text = TotalReturn.ToString();
                    labRest.Text = rest.ToString();

                    totalBill = 0; TotalReturn = 0;

                    foreach (DataGridViewRow row1 in dataGridView2.Rows)
                    {
                        totalBill += Convert.ToDouble(row1.Cells[6].Value);
                        TotalReturn += Convert.ToDouble(row1.Cells[5].Value);
                        rest2 = totalBill - TotalReturn;
                    }

                    labTotalPaid.Text = totalBill.ToString();
                    labTotalReturn2.Text = TotalReturn.ToString();
                    labRest2.Text = rest2.ToString();

                    if (rest2 > 0)
                    {
                        safay = rest - rest2;
                    }
                    else
                    {
                        safay = rest + rest2;
                    }
                    safay += Convert.ToDouble(labCustomerOpenAccount.Text);
                    labSafay.Text = safay.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbconnection.Close();
            dbconnection1.Close();
        }

        //function
        // display Customer bills
        public void displayBill()
        {
            DateTime date = dateTimeFrom.Value.Date;
            string d = date.ToString("yyyy-MM-dd");
            DateTime date2 = dateTimeTo.Value.Date;
            string d2 = date2.ToString("yyyy-MM-dd");
            string query = "";
            if (txtClientID.Text != "" && txtCustomerID.Text != "")
            {
                query = "select * from customer_bill where Client_ID=" + txtClientID.Text + " and Customer_ID='" + txtCustomerID.Text + "' and Bill_Date between '" + d + "' and '" + d2 + "'";
                string q = "select Customer_OpenAccount from customer where Customer_ID=" + txtClientID.Text;
                MySqlCommand c = new MySqlCommand(q, dbconnection);
                labCustomerOpenAccount.Text = c.ExecuteScalar().ToString();
            }
            else if (txtClientID.Text == "" && txtCustomerID.Text != "")
            {
                query = "select * from customer_bill inner join customer on customer_bill.Client_ID=customer.Customer_ID where  customer_bill.Customer_ID=" + txtCustomerID.Text + " and Bill_Date between '" + d + "' and '" + d2 + "'";
                MySqlCommand com1 = new MySqlCommand(query, dbconnection1);
                MySqlDataReader dr1 = com1.ExecuteReader();
                dataGridView1.Columns[3].Visible = true;
                dataGridView1.Columns[2].Visible = true;
                while (dr1.Read())
                {
                    int n = dataGridView1.Rows.Add();
                    dataGridView1.Rows[n].Cells[6].Value = dr1["Total_CostAD"].ToString();
                    dataGridView1.Rows[n].Cells[5].Value = "0.00";
                    dataGridView1.Rows[n].Cells[4].Value = dr1["CustomerBill_ID"].ToString();
                    dataGridView1.Rows[n].Cells[3].Value = dr1["Customer_Name"].ToString();
                    dataGridView1.Rows[n].Cells[2].Value = dr1["Client_ID"].ToString();
                    dataGridView1.Rows[n].Cells[1].Value = dr1["Type_Buy"].ToString();
                    dataGridView1.Rows[n].Cells[0].Value = dr1["Bill_Date"].ToString();
                }
                dr1.Close();

                string q = "select Customer_OpenAccount from customer where Customer_ID=" + txtCustomerID.Text;
                MySqlCommand c = new MySqlCommand(q, dbconnection);
                labCustomerOpenAccount.Text = c.ExecuteScalar().ToString();

                return;
            }
            else
            {
                query = "select * from customer_bill where Client_ID=" + txtClientID.Text + " and Customer_ID is null and Bill_Date between '" + d + "' and '" + d2 + "'";
                string q = "select Customer_OpenAccount from customer where Customer_ID=" + txtClientID.Text;
                MySqlCommand c = new MySqlCommand(q, dbconnection);
                labCustomerOpenAccount.Text = c.ExecuteScalar().ToString();
            }
            MySqlCommand com = new MySqlCommand(query, dbconnection1);
            MySqlDataReader dr = com.ExecuteReader();

            while (dr.Read())
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[6].Value = dr["Total_CostAD"].ToString();
                dataGridView1.Rows[n].Cells[5].Value = "0.00";
                dataGridView1.Rows[n].Cells[4].Value = dr["CustomerBill_ID"].ToString();
                dataGridView1.Rows[n].Cells[1].Value = dr["Type_Buy"].ToString();
                dataGridView1.Rows[n].Cells[0].Value = dr["Bill_Date"].ToString();
            }
            dr.Close();
            dataGridView1.Columns[3].Visible = false;
            dataGridView1.Columns[2].Visible = false;
        }
        // display Customer return bills
        public void displayReturnBill(DataGridView datagridview)
        {
            DateTime date = dateTimeFrom.Value.Date;
            string d = date.ToString("yyyy-MM-dd");
            DateTime date2 = dateTimeTo.Value.Date;
            string d2 = date2.ToString("yyyy-MM-dd");
            string query = "";
            if (txtClientID.Text != "" && txtCustomerID.Text != "")
            {
                query = "select * from customer_return_bill where Client_ID=" + txtClientID.Text + " and Customer_ID='" + txtCustomerID.Text + "' and Date between '" + d + "' and '" + d2 + "'";
            }
            else if (txtClientID.Text == "" && txtCustomerID.Text != "")
            {
                query = "select * from customer_return_bill inner join customer on customer_return_bill.Client_ID=customer.Customer_ID where  customer_return_bill.Customer_ID=" + txtCustomerID.Text + " and Date between '" + d + "' and '" + d2 + "'";
                MySqlCommand com1 = new MySqlCommand(query, dbconnection1);
                MySqlDataReader dr1 = com1.ExecuteReader();
                datagridview.Columns[3].Visible = true;
                datagridview.Columns[2].Visible = true;
                while (dr1.Read())
                {
                    int n = datagridview.Rows.Add();
                    datagridview.Rows[n].Cells[6].Value = "0.00";
                    datagridview.Rows[n].Cells[5].Value = dr1["TotalCostAD"].ToString();
                    datagridview.Rows[n].Cells[4].Value = dr1["CustomerReturnBill_ID"].ToString();
                    datagridview.Rows[n].Cells[3].Value = dr1["Customer_Name"].ToString();
                    datagridview.Rows[n].Cells[2].Value = dr1["Client_ID"].ToString();
                    datagridview.Rows[n].Cells[1].Value = dr1["Type_Buy"].ToString();
                    datagridview.Rows[n].Cells[0].Value = dr1["Date"].ToString();
                }
                dr1.Close();
                return;
            }
            else
            {
                query = "select * from customer_return_bill where Client_ID=" + txtClientID.Text + " and Customer_ID is null and Date between '" + d + "' and '" + d2 + "'";
            }
            MySqlCommand com = new MySqlCommand(query, dbconnection1);
            MySqlDataReader dr = com.ExecuteReader();

            while (dr.Read())
            {
                int n = datagridview.Rows.Add();
                datagridview.Rows[n].Cells[6].Value = "0.00";
                datagridview.Rows[n].Cells[5].Value = dr["TotalCostAD"].ToString();
                datagridview.Rows[n].Cells[4].Value = dr["CustomerReturnBill_ID"].ToString();
                datagridview.Rows[n].Cells[1].Value = dr["Type_Buy"].ToString();
                datagridview.Rows[n].Cells[0].Value = dr["Date"].ToString();
            }
            dr.Close();
            datagridview.Columns[3].Visible = false;
            datagridview.Columns[2].Visible = false;
        }
        // display Customer Paid bills
        public void displayPaidBill()
        {
            DateTime date = dateTimeFrom.Value.Date;
            string d = date.ToString("yyyy-MM-dd");
            DateTime date2 = dateTimeTo.Value.Date;
            string d2 = date2.ToString("yyyy-MM-dd");
            string query = "", query1 = "";
            string Name = "";
            if (txtClientID.Text != "" && txtCustomerID.Text != "")
            {
                query = "select * from transitions  where Client_ID=" + txtClientID.Text + " and Customer_ID='" + txtCustomerID.Text + "'  and Date between '" + d + "' and '" + d2 + "' and Transition='ايداع'";
                query1 = "select * from customer_taswaya where Client_ID=" + txtClientID.Text + " and Customer_ID='" + txtCustomerID.Text + "' and Date between '" + d + "' and '" + d2 + "' and Taswaya_Type='اضافة'";
                Name = comClient.Text;
            }
            else if (txtClientID.Text == "" && txtCustomerID.Text != "")
            {
                dataGridView2.Columns[3].Visible = true;
                query = "select * from transitions inner join customer on transitions.Client_ID=customer.Customer_ID  where transitions.Customer_ID=" + txtCustomerID.Text + "  and Date between '" + d + "' and '" + d2 + "' and Transition='ايداع'";
                query1 = "select * from customer_taswaya inner join customer on customer_taswaya.Client_ID=customer.Customer_ID where  customer_taswaya.Customer_ID=" + txtCustomerID.Text + " and Date between '" + d + "' and '" + d2 + "'  and Taswaya_Type='اضافة'";
                MySqlCommand com1 = new MySqlCommand(query, dbconnection1);
                MySqlDataReader dr1 = com1.ExecuteReader();

                while (dr1.Read())
                {
                    int n = dataGridView2.Rows.Add();
                    dataGridView2.Rows[n].Cells[6].Value = dr1["Amount"].ToString();
                    dataGridView2.Rows[n].Cells[5].Value = "0.00";
                    dataGridView2.Rows[n].Cells[4].Value = dr1["Transition_ID"].ToString();
                    dataGridView2.Rows[n].Cells[3].Value = dr1["Customer_Name"].ToString();
                    dataGridView2.Rows[n].Cells[2].Value = dr1["Client_ID"].ToString();
                    dataGridView2.Rows[n].Cells[1].Value = dr1["Type"].ToString();
                    dataGridView2.Rows[n].Cells[0].Value = dr1["Date"].ToString();
                }
                dr1.Close();

                com1 = new MySqlCommand(query1, dbconnection1);
                dr1 = com1.ExecuteReader();

                while (dr1.Read())
                {
                    int n = dataGridView2.Rows.Add();
                    dataGridView2.Rows[n].Cells[6].Value = dr1["Money_Paid"].ToString();
                    dataGridView2.Rows[n].Cells[5].Value = "0.00";
                    dataGridView2.Rows[n].Cells[4].Value = dr1["CustomerTaswaya_ID"].ToString();
                    dataGridView2.Rows[n].Cells[3].Value = dr1["Customer_Name"].ToString();
                    dataGridView2.Rows[n].Cells[2].Value = dr1["Client_ID"].ToString();
                    dataGridView2.Rows[n].Cells[1].Value = "تسوية";
                    dataGridView2.Rows[n].Cells[0].Value = dr1["Date"].ToString();
                }
                dr1.Close();
                return;
            }
            else
            {
                query = "select * from transitions where Client_ID=" + txtClientID.Text + "  and Date between '" + d + "' and '" + d2 + "' and Transition='ايداع'";
                query1 = "select * from customer_taswaya where Client_ID=" + txtClientID.Text + " and Customer_ID is null and Date between '" + d + "' and '" + d2 + "'  and Taswaya_Type='اضافة'";
                Name = comClient.Text;
            }
            MySqlCommand com = new MySqlCommand(query, dbconnection1);
            MySqlDataReader dr = com.ExecuteReader();

            while (dr.Read())
            {
                int n = dataGridView2.Rows.Add();
                dataGridView2.Rows[n].Cells[6].Value = dr["Amount"].ToString();
                dataGridView2.Rows[n].Cells[5].Value = "0.00";
                dataGridView2.Rows[n].Cells[4].Value = dr["Transition_ID"].ToString();
                dataGridView2.Rows[n].Cells[1].Value = dr["Type"].ToString();
                dataGridView2.Rows[n].Cells[0].Value = dr["Date"].ToString();
            }
            dr.Close();

            com = new MySqlCommand(query1, dbconnection1);
            dr = com.ExecuteReader();

            while (dr.Read())
            {
                int n = dataGridView2.Rows.Add();
                dataGridView2.Rows[n].Cells[6].Value = dr["Money_Paid"].ToString();
                dataGridView2.Rows[n].Cells[5].Value = "0.00";
                dataGridView2.Rows[n].Cells[4].Value = dr["CustomerTaswaya_ID"].ToString();
                dataGridView2.Rows[n].Cells[1].Value = "تسوية";
                dataGridView2.Rows[n].Cells[0].Value = dr["Date"].ToString();
            }
            dr.Close();
            dataGridView2.Columns[3].Visible = false;
            dataGridView2.Columns[2].Visible = false;
        }
        // display Customer Paid Return bills
        public void displayPaidReturnBill()
        {
            DateTime date = dateTimeFrom.Value.Date;
            string d = date.ToString("yyyy-MM-dd");
            DateTime date2 = dateTimeTo.Value.Date;
            string d2 = date2.ToString("yyyy-MM-dd");
            string query = "", query1 = "";
            if (txtClientID.Text != "" && txtCustomerID.Text != "")
            {
                query = "select * from transitions where Client_ID=" + txtClientID.Text + " and Customer_ID='" + txtCustomerID.Text + "'  and Date between '" + d + "' and '" + d2 + "' and Transition='سحب'";
                query1 = "select * from customer_taswaya where Client_ID=" + txtClientID.Text + " and Customer_ID='" + txtCustomerID.Text + "' and Date between '" + d + "' and '" + d2 + "' and Taswaya_Type='خصم'";
                Name = comClient.Text;
            }
            else if (txtClientID.Text == "" && txtCustomerID.Text != "")
            {
                query = "select * from transitions inner join customer on transitions.Client_ID=customer.Customer_ID where transitions.Customer_ID=" + txtCustomerID.Text + "  and Date between '" + d + "' and '" + d2 + "' and Transition='سحب'";
                query1 = "select * from customer_taswaya inner join customer on customer_taswaya.Client_ID=customer.Customer_ID where  customer_taswaya.Customer_ID=" + txtCustomerID.Text + " and Date between '" + d + "' and '" + d2 + "'  and Taswaya_Type='خصم'";
                MySqlCommand com1 = new MySqlCommand(query, dbconnection1);
                MySqlDataReader dr1 = com1.ExecuteReader();
                dataGridView2.Columns[3].Visible = true;
                dataGridView2.Columns[2].Visible = true;
                while (dr1.Read())
                {
                    int n = dataGridView2.Rows.Add();
                    dataGridView2.Rows[n].Cells[6].Value =  "0.00";
                    dataGridView2.Rows[n].Cells[5].Value = dr1["Amount"].ToString();
                    dataGridView2.Rows[n].Cells[4].Value = dr1["Transition_ID"].ToString();
                    dataGridView2.Rows[n].Cells[3].Value = dr1["Customer_Name"].ToString();
                    dataGridView2.Rows[n].Cells[2].Value = dr1["Client_ID"].ToString();
                    dataGridView2.Rows[n].Cells[1].Value = dr1["Type"].ToString();
                    dataGridView2.Rows[n].Cells[0].Value = dr1["Date"].ToString();
                }
                dr1.Close();

                com1 = new MySqlCommand(query1, dbconnection1);
                dr1 = com1.ExecuteReader();

                while (dr1.Read())
                {
                    int n = dataGridView2.Rows.Add();
                    dataGridView2.Rows[n].Cells[6].Value = "0.00"; 
                    dataGridView2.Rows[n].Cells[5].Value = dr1["Money_Paid"].ToString();
                    dataGridView2.Rows[n].Cells[4].Value = dr1["CustomerTaswaya_ID"].ToString();
                    dataGridView2.Rows[n].Cells[3].Value = dr1["Customer_Name"].ToString();
                    dataGridView2.Rows[n].Cells[2].Value = dr1["Client_ID"].ToString();
                    dataGridView2.Rows[n].Cells[1].Value = "تسوية";
                    dataGridView2.Rows[n].Cells[0].Value = dr1["Date"].ToString();
                }
                dr1.Close();
                return;
            }
            else
            {
                query = "select * from transitions where Client_ID=" + txtClientID.Text + "  and Date between '" + d + "' and '" + d2 + "' and Transition='سحب'";
                query1 = "select * from customer_taswaya where Client_ID=" + txtClientID.Text + " and Customer_ID is null and Date between '" + d + "' and '" + d2 + "'  and Taswaya_Type='خصم'";
                Name = comClient.Text;
            }
            MySqlCommand com = new MySqlCommand(query, dbconnection1);
            MySqlDataReader dr = com.ExecuteReader();

            while (dr.Read())
            {
                int n = dataGridView2.Rows.Add();
                dataGridView2.Rows[n].Cells[6].Value = "0.00";
                dataGridView2.Rows[n].Cells[5].Value = dr["Amount"].ToString();
                dataGridView2.Rows[n].Cells[4].Value = dr["Transition_ID"].ToString();
                dataGridView2.Rows[n].Cells[1].Value = dr["Type"].ToString();
                dataGridView2.Rows[n].Cells[0].Value = dr["Date"].ToString();
            }
            dr.Close();
            com = new MySqlCommand(query1, dbconnection1);
            dr = com.ExecuteReader();

            while (dr.Read())
            {
                int n = dataGridView2.Rows.Add();
                dataGridView2.Rows[n].Cells[0].Value = "0.00";
                dataGridView2.Rows[n].Cells[1].Value = dr["Money_Paid"].ToString();
                dataGridView2.Rows[n].Cells[2].Value = dr["CustomerTaswaya_ID"].ToString(); ;
                dataGridView2.Rows[n].Cells[5].Value = "تسوية";
                dataGridView2.Rows[n].Cells[6].Value = dr["Date"].ToString();
            }
            dr.Close();
            dataGridView2.Columns[3].Visible = false;
            dataGridView2.Columns[2].Visible = false;
        }
        private void AccountStatement_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
        private void btnReport_Click(object sender, EventArgs e)
        {
            string Name = "";
            DataTable dt = new DataTable();
            for (int i = 0; i < dataGridView2.Columns.Count; i++)
            {
                dt.Columns.Add(dataGridView2.Columns[i].Name.ToString());
            }
            for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < dataGridView2.Columns.Count; j++)
                {
                    dr[dataGridView2.Columns[j].Name.ToString()] = dataGridView2.Rows[i].Cells[j].Value;
                }

                dt.Rows.Add(dr);
            }

            if (comClient.Text != "")
            {
                Name = comClient.Text;
            }
            else if (comEngCon.Text != "")
            {
                Name = comEngCon.Text;
            }

        }
        private void btnTaswaya_Click(object sender, EventArgs e)
        {
            try
            {
                saleMainForm.bindTaswayaCustomersForm(Customer_Type, txtCustomerID.Text, txtClientID.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
