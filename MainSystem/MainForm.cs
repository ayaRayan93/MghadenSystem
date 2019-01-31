﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using DevExpress.XtraGrid;
using DevExpress.XtraTab.ViewInfo;
using DevExpress.XtraNavBar;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace MainSystem
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        MySqlConnection dbconnection;
        XtraTabPage StoreTP;
        XtraTabPage SalesTP;
        XtraTabPage HRTP;
        XtraTabPage CarsTP;
        XtraTabPage POSTP;
        XtraTabPage BankTP;
        XtraTabPage ReceptionTP;
        XtraTabPage ShippingTP;
        XtraTabPage AccountingTP;
        XtraTabPage CodingTP;
        int index = 1;
        
        public MainForm()
        {
            try
            {
                dbconnection = new MySqlConnection(connection.connectionString);
                InitializeComponent();
                //bankSystem
                if (UserControl.userType == 5)
                {
                    POSSystem();
                }
                else
                {
                    initialize();
                    ReceptionSystem();
                    SalesMainForm();
                    ShippingForm();
                    POSSystem();
                    initializeBranch();
                }

                StoreTP = xtraTabPageStores;
                SalesTP =xtraTabPageSales;
                HRTP = xtraTabPageHR;
                CarsTP = xtraTabPageCars;
                POSTP = xtraTabPagePOS;
                BankTP = xtraTabPageBank;
                ReceptionTP = xtraTabPageReception;
                ShippingTP = xtraTabPageShipping;
                AccountingTP = xtraTabPageAccounting;
                CodingTP = xtraTabPageCoding;
                xtraTabControlMainContainer.TabPages.Remove(xtraTabPageStores);
                xtraTabControlMainContainer.TabPages.Remove(xtraTabPageSales);
                xtraTabControlMainContainer.TabPages.Remove(xtraTabPageHR);
                xtraTabControlMainContainer.TabPages.Remove(xtraTabPageCars);
                xtraTabControlMainContainer.TabPages.Remove(xtraTabPagePOS);
                xtraTabControlMainContainer.TabPages.Remove(xtraTabPageBank);
                xtraTabControlMainContainer.TabPages.Remove(xtraTabPageReception);
                xtraTabControlMainContainer.TabPages.Remove(xtraTabPageShipping);
                xtraTabControlMainContainer.TabPages.Remove(xtraTabPageAccounting);
                xtraTabControlMainContainer.TabPages.Remove(xtraTabPageCoding);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (UserControl.userType == 1)
            {
                btnCars.Enabled = true;
                btnCars.Checked = true;
                btnStores.Enabled = true;
                btnStores.Checked = true;
                btnBank.Enabled = true;
                btnBank.Checked = true;
                btnReception.Enabled = true;
                btnReception.Checked = true;
                btnPOS.Enabled = true;
                btnPOS.Checked = true;
                btnSales.Enabled = true;
                btnSales.Checked = true;
                TIElsha7n.Enabled = true;
                TIElsha7n.Checked = true;
                AccountingSystem.Enabled = true;
                AccountingSystem.Checked = true;
                btnBuying.Enabled = true;
                btnBuying.Checked = true;
                btnHR.Enabled = true;
                btnHR.Checked = true;
                btnCustomerService.Enabled = true;
                btnCustomerService.Checked = true;
                btnCoding.Enabled = true;
                btnCoding.Checked = true;
                btnReports.Enabled = true;
                btnReports.Checked = true;
            }
            else if (UserControl.userType == 2)
            {
                btnStores.Enabled = true;
                btnStores.Checked = true;
            }
            else if (UserControl.userType == 3)
            {
                btnBank.Enabled = true;
                btnBank.Checked = true;
            }
            else if (UserControl.userType == 4)
            {
                btnReception.Enabled = true;
                btnReception.Checked = true;
            }
            else if (UserControl.userType == 5)
            {
                btnPOS.Enabled = true;
                btnPOS.Checked = true;
            }
            else if (UserControl.userType == 6 || UserControl.userType == 7)
            {
                btnSales.Enabled = true;
                btnSales.Checked = true;
            }
            else if (UserControl.userType == 8)
            {
                TIElsha7n.Enabled = true;
                TIElsha7n.Checked = true;
            }
            else if (UserControl.userType == 9)
            {
                AccountingSystem.Enabled = true;
                AccountingSystem.Checked = true;
            }
            else if (UserControl.userType == 10)
            {
                btnBuying.Enabled = true;
                btnBuying.Checked = true;
            }
            else if (UserControl.userType == 11)
            {
                btnHR.Enabled = true;
                btnHR.Checked = true;
            }
            else if (UserControl.userType == 12)
            {
                btnCustomerService.Enabled = true;
                btnCustomerService.Checked = true;
            }
            else if (UserControl.userType == 13)
            {
                btnCoding.Enabled = true;
                btnCoding.Checked = true;
            }
            else if (UserControl.userType == 14)
            {
                btnCars.Enabled = true;
                btnCars.Checked = true;
            }
        }

        //events
        private void btnStores_ItemClick(object sender, TileItemEventArgs e)
        {
            try
            {
                if (!xtraTabControlMainContainer.TabPages.Contains(xtraTabPageStores))
                {
                    xtraTabControlMainContainer.TabPages.Insert(index, StoreTP);
                    index++;
                }
                xtraTabControlMainContainer.SelectedTabPage = xtraTabControlMainContainer.TabPages[xtraTabControlMainContainer.TabPages.IndexOf(xtraTabPageStores)];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSales_ItemClick(object sender, TileItemEventArgs e)
        {
            try
            {
                if (!xtraTabControlMainContainer.TabPages.Contains(xtraTabPageSales))
                {
                    xtraTabControlMainContainer.TabPages.Insert(index, SalesTP);
                    index++;
                }
                xtraTabControlMainContainer.SelectedTabPage = xtraTabControlMainContainer.TabPages[xtraTabControlMainContainer.TabPages.IndexOf(xtraTabPageSales)];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnHR_ItemClick(object sender, TileItemEventArgs e)
        {
            try
            {
                if (!xtraTabControlMainContainer.TabPages.Contains(xtraTabPageHR))
                {
                    xtraTabControlMainContainer.TabPages.Insert(index, HRTP);
                    index++;
                }
                xtraTabControlMainContainer.SelectedTabPage = xtraTabControlMainContainer.TabPages[xtraTabControlMainContainer.TabPages.IndexOf(xtraTabPageHR)];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCars_ItemClick(object sender, TileItemEventArgs e)
        {
            try
            {
                if (!xtraTabControlMainContainer.TabPages.Contains(xtraTabPageCars))
                {
                    xtraTabControlMainContainer.TabPages.Insert(index, CarsTP);
                    index++;
                }
                xtraTabControlMainContainer.SelectedTabPage = xtraTabControlMainContainer.TabPages[xtraTabControlMainContainer.TabPages.IndexOf(xtraTabPageCars)];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnPOS_ItemClick(object sender, TileItemEventArgs e)
        {
            try
            {
                if (!xtraTabControlMainContainer.TabPages.Contains(xtraTabPagePOS))
                {
                    xtraTabControlMainContainer.TabPages.Insert(index, POSTP);
                    index++;
                }
                xtraTabControlMainContainer.SelectedTabPage = xtraTabControlMainContainer.TabPages[xtraTabControlMainContainer.TabPages.IndexOf(xtraTabPagePOS)];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnBank_ItemClick(object sender, TileItemEventArgs e)
        {
            try
            {
                if (!xtraTabControlMainContainer.TabPages.Contains(xtraTabPageBank))
                {
                    xtraTabControlMainContainer.TabPages.Insert(index, BankTP);
                    index++;
                }
                xtraTabControlMainContainer.SelectedTabPage = xtraTabControlMainContainer.TabPages[xtraTabControlMainContainer.TabPages.IndexOf(xtraTabPageBank)];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnReception_ItemClick(object sender, TileItemEventArgs e)
        {
            try
            {
                if (!xtraTabControlMainContainer.TabPages.Contains(xtraTabPageReception))
                {
                    xtraTabControlMainContainer.TabPages.Insert(index, ReceptionTP);
                    index++;
                }
                xtraTabControlMainContainer.SelectedTabPage = xtraTabControlMainContainer.TabPages[xtraTabControlMainContainer.TabPages.IndexOf(xtraTabPageReception)];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TIElsha7n_ItemClick(object sender, TileItemEventArgs e)
        {
            try
            {
                if (!xtraTabControlMainContainer.TabPages.Contains(xtraTabPageShipping))
                {
                    xtraTabControlMainContainer.TabPages.Insert(index, ShippingTP);
                    index++;
                }
                xtraTabControlMainContainer.SelectedTabPage = xtraTabControlMainContainer.TabPages[xtraTabControlMainContainer.TabPages.IndexOf(xtraTabPageShipping)];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AccountingSystem_ItemClick(object sender, TileItemEventArgs e)
        {
            try
            {
                if (!xtraTabControlMainContainer.TabPages.Contains(xtraTabPageAccounting))
                {
                    xtraTabControlMainContainer.TabPages.Insert(index, AccountingTP);
                    index++;
                }
                xtraTabControlMainContainer.SelectedTabPage = xtraTabControlMainContainer.TabPages[xtraTabControlMainContainer.TabPages.IndexOf(xtraTabPageAccounting)];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCoding_ItemClick(object sender, TileItemEventArgs e)
        {
            try
            {
                if (!xtraTabControlMainContainer.TabPages.Contains(xtraTabPageCoding))
                {
                    xtraTabControlMainContainer.TabPages.Insert(index, CodingTP);
                    index++;
                }
                xtraTabControlMainContainer.SelectedTabPage = xtraTabControlMainContainer.TabPages[xtraTabControlMainContainer.TabPages.IndexOf(xtraTabPageCoding)];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (!IsTabPageSave())
                {
                    DialogResult dialogResult = MessageBox.Show("There are unsave Pages To you wound close anyway?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Application.Exit();
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        e.Cancel = (dialogResult == DialogResult.No);
                    }
                }
                else
                {
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void xtraTabControlMainContainer_CloseButtonClick(object sender, EventArgs e)
        {
            try
            {
                ClosePageButtonEventArgs arg = e as ClosePageButtonEventArgs;
                XtraTabPage xtraTabPage = (XtraTabPage)arg.Page;
                if (!IsTabPageSave())
                {
                    DialogResult dialogResult = MessageBox.Show("There are unsave Pages To you wound close anyway?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        xtraTabControlMainContainer.TabPages.Remove(arg.Page as XtraTabPage);

                    }
                    else if (dialogResult == DialogResult.No)
                    {

                    }
                }
                else
                {
                    xtraTabControlMainContainer.TabPages.Remove(arg.Page as XtraTabPage);
                    index--;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void xtraTabControlContent_Click(object sender, EventArgs e)
        {
            try
            {
                ClosePageButtonEventArgs arg = e as ClosePageButtonEventArgs;
                XtraTabPage xtraTabPage = (XtraTabPage)arg.Page;
                XtraTabControl XtraTabControl = (XtraTabControl)sender;
                if (xtraTabPage.ImageOptions.Image != null)
                {
                    DialogResult dialogResult = MessageBox.Show("Are you sure you want to Close this page without save?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        XtraTabControl.TabPages.Remove(arg.Page as XtraTabPage);
                    }
                    else if (dialogResult == DialogResult.No)
                    {

                    }
                }
                else
                {
                   XtraTabControl tabControl=(XtraTabControl) xtraTabPage.Parent;
                    tabControl.TabPages.Remove(arg.Page as XtraTabPage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //functions
        public XtraTabPage getTabPage(XtraTabControl tabControl,string text)
        {
            for (int i = 0; i < tabControl.TabPages.Count; i++)
                if (tabControl.TabPages[i].Text == text)
                {
                    return tabControl.TabPages[i];
                }
            return null;
        }

        public bool IsTabPageSave()
        {
            for (int i = 0; i < xtraTabControlMainContainer.TabPages.Count; i++)
            {
                foreach (Control item in xtraTabControlMainContainer.TabPages[i].Controls)
                {
                    if (item is XtraTabControl)
                    {
                        XtraTabControl item1 = (XtraTabControl)item;
                        for (int j = 0; j < item1.TabPages.Count; j++)
                            if (item1.TabPages[j].ImageOptions.Image != null)
                            {
                                return false;
                            }
                    }
                }
            }
            return true;
        }

        public void restForeColorOfNavBarItem()
        {
            foreach (XtraTabPage tabPage in xtraTabControlMainContainer.TabPages)
            {
                foreach (Control item in tabPage.Controls)
                {
                    if (item is NavBarControl)
                    {
                        foreach (NavBarItem navBar in navBarControl1.Items)
                        {
                            navBar.Appearance.ForeColor = Color.Black;
                            navBar.Appearance.BackColor = Color.LightGray;
                        }
                    }
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Form form = this as Form;
                form.FormClosing -= MainForm_FormClosing;
                Application.Exit();
                //form.FormClosing += MainForm_FormClosing;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pictureBoxLogout_Click(object sender, EventArgs e)
        {
            try
            {
                Login_Admin loginForm = new Login_Admin();
                loginForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pictureBoxProfile_Click(object sender, EventArgs e)
        {
            try
            {
                UserUpdate form = new UserUpdate(this);
                form.ShowDialog();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }

    public static class connection
    {
        public static string connectionString = "SERVER=192.168.1.200;DATABASE=test;user=Devccc;PASSWORD=rootroot;CHARSET=utf8;SslMode=none";
        // public static string connectionString = "SERVER=localhost;DATABASE=testcoding;user=root;PASSWORD=root;CHARSET=utf8";
    }
}