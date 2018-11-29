﻿using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace MainSystem
{
    public partial class BillCash : DevExpress.XtraReports.UI.XtraReport
    {
        public BillCash()
        {
            InitializeComponent();
        }

        public void InitData(DateTime dateNow, string transitionID, string branchName, int billNumber, string clientName, DateTime billDate, double paidMoney, string paymentMethod, string bank, string checkNumber, string payday, string visaType, string operationNumber, string description, string confirmEmp, string bankUserName, int qq200, int qq100, int qq50, int qq20, int qq10, int qq5, int qq1, int qqH, int qqQ)
        {
            TransitionID.Value = transitionID;
            DateNow.Value = dateNow;
            Branch_Name.Value = branchName;
            BillNumber.Value = billNumber;
            Client_Name.Value = clientName;
            BillDate.Value = billDate;
            PaidMoney.Value = paidMoney;
            Description.Value = description;
            PaymentMethod.Value = paymentMethod;
            Bank.Value = bank;
            CheckNumber.Value = checkNumber;
            //Payday.Value = payday.Date;
            Payday.Value = payday;
            VisaType.Value = visaType;
            OperationNumber.Value = operationNumber;
            ConfirmEmp.Value = confirmEmp;
            BankUserName.Value = bankUserName;
            q200.Value = qq200;
            q100.Value = qq100;
            q50.Value = qq50;
            q20.Value = qq20;
            q10.Value = qq10;
            q5.Value = qq5;
            q1.Value = qq1;
            qH.Value = qqH;
            qQ.Value = qqQ;
        }
    }
}
