﻿using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;

namespace MainSystem
{
    public partial class Print_ProductsBillsFactories : DevExpress.XtraReports.UI.XtraReport
    {
        public Print_ProductsBillsFactories()
        {
            InitializeComponent();
        }

        public void InitData(string factoryName, DateTime fromDate, DateTime toDate, List<Items_Bills> Bill_Items)
        {
            Factory_Name.Value = factoryName;
            FromDate.Value = fromDate;
            ToDate.Value = toDate;
            DateNow.Value = DateTime.Now;
            objectDataSource1.DataSource = Bill_Items;
        }
    }
}
