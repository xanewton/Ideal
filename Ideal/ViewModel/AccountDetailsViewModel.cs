#region Copyright
/*
 * Copyright (C) 2017 Angel Garcia
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

#if USE_SAMPLE_DB
using Ideal.SampleDBModel;
#else
using Ideal.DBModel;
#endif

namespace Ideal
{

    public class PaymentModel
    {
        public string ACC_ID { get; set; }
        public string PAY_DATE { get; set; }
        public decimal PAY_AMOUNT { get; set; }
        public Nullable<int> PAY_VISITS { get; set; }
    }

    public class ScheduledPaymentModel
    {
        public string ACC_ID { get; set; }
        public string SCHPAY_DATE { get; set; }
        public decimal SCHPAY_AMOUNT { get; set; }
    }

    public class DetailModel
    {
        public string Item { get; set; }
        public string Value { get; set; }
    }


    /// <summary>
    /// Model that represents the AccountDetailsPage
    /// </summary>
    class AccountDetailsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ACCOUNT_CLIENT_SCHEDULED_PAYMENTS_VIEW> Accounts { get; set; }
        public ObservableCollection<String> AccountIDs { get; private set; }
        public ObservableCollection<DetailModel> Details { get; set; }
        public ObservableCollection<Model> Totals { get; set; } // Total chart
        public ObservableCollection<PaymentModel> Payments { get; set; }
        public ObservableCollection<ScheduledPaymentModel> SchPayments { get; set; }
        public ObservableCollection<Model> TotalProgress { get; set; } // Progress chart
        public ObservableCollection<Model> TotalLag { get; set; }  //Lag chart
        public ObservableCollection<ItemModel> PaymentComparation { get; set; } // Payment comparation chart


        private List<PAYMENT_TBL> listPayments;
        private List<SCHEDULED_PAYMENT_TBL> listSchPayments;

        private double _positiveAmount;
        public double PositiveAmount
        {
            get { return _positiveAmount; }
            set
            {
                _positiveAmount = value;
                RaisePropertyChanged("PositiveAmount");
            }
        }

        private double _negativeAmount;
        public double NegativeAmount
        {
            get { return _negativeAmount; }
            set
            {
                _negativeAmount = value;
                RaisePropertyChanged("NegativeAmount");
            }
        }

        private double _balanceAmount;
        public double BalanceAmount
        {
            get { return _balanceAmount; }
            set
            {
                _balanceAmount = value;
                RaisePropertyChanged("BalanceAmount");
            }
        }

        private int _noPositiveTransactions;
        public int NoPositiveTransactions
        {
            get { return _noPositiveTransactions; }
            set
            {
                _noPositiveTransactions = value;
                RaisePropertyChanged("NoPositiveTransactions");
            }
        }

        private int _noNegativeTransactions;
        public int NoNegativeTransactions
        {
            get { return _noNegativeTransactions; }
            set
            {
                _noNegativeTransactions = value;
                RaisePropertyChanged("NoNegativeTransactions");
            }
        }

        void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // Represents the selected account id in the combobox
        private string selectedItemAccountID = null;
        public string SelectedItemAccountID
        {
            get { return selectedItemAccountID; }
            set
            {
                if (selectedItemAccountID != value)
                {
                    selectedItemAccountID = value;
                    RaisePropertyChanged("SelectedItemAccountID");
#if USE_SAMPLE_DB
                    using (var db = new SampleModel())
#else
                    using (var db = new IdealContext())
#endif
                    {
                        try
                        {
                            db.Database.Connection.Open();
                            FillPayments(db, selectedItemAccountID);
                            FillSchPayments(db, selectedItemAccountID);
                            FillDetails(selectedItemAccountID);
                            FillStatusValues(selectedItemAccountID);
                            FillTotalProgress(selectedItemAccountID);
                            FillTotalLag(selectedItemAccountID);
                            FillTotals(selectedItemAccountID);
                            FillPaymentComparationSeries(selectedItemAccountID);
                        }
                        catch (System.Data.SqlClient.SqlException e)
                        {
                            MessageBox.Show(e.Message, "Database connection error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }



        /// <summary>
        /// Constructor
        /// </summary>
        public AccountDetailsViewModel()
        {
#if USE_SAMPLE_DB
            using (var db = new SampleModel())
#else
            using (var db = new IdealContext())
#endif
            {
                try
                {
                    db.Database.Connection.Open();
                    FillAccounts(db);
                    selectedItemAccountID = AccountIDs.ElementAt(0);
                    FillPayments(db, selectedItemAccountID);
                    FillSchPayments(db, selectedItemAccountID);
                    FillDetails(selectedItemAccountID);
                    FillStatusValues(selectedItemAccountID);
                    FillTotalProgress(selectedItemAccountID);
                    FillTotalLag(selectedItemAccountID);
                    FillTotals(selectedItemAccountID);
                    FillPaymentComparationSeries(selectedItemAccountID);
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    MessageBox.Show(e.Message, "Database connection error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Fill the Payments list with the accountID element.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="accountId"></param>
#if USE_SAMPLE_DB
        public void FillPayments(SampleModel db, string accountId)
#else
        public void FillPayments(IdealContext db, string accountId)
#endif
        {
            var query = from pay in db.PAYMENT_TBL
                        where pay.ACC_ID == accountId
                        orderby pay.PAY_DATE
                        select pay;
            if (!query.Any()) { return; }
            listPayments = new List<PAYMENT_TBL>();
            Payments = new ObservableCollection<PaymentModel>();
            foreach (var r in query)
            {
                listPayments.Add(r);
                Payments.Add(new PaymentModel()
                {
                    ACC_ID = r.ACC_ID,
                    PAY_DATE = String.Format("{0:ddd MMM dd, yyyy}", r.PAY_DATE),
                    PAY_AMOUNT = r.PAY_AMOUNT,
                    PAY_VISITS = r.PAY_VISITS
                });
            }
            RaisePropertyChanged("Payments");
        }

        /// <summary>
        /// Fill SchPayments with the accountID element.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="accountId"></param>
#if USE_SAMPLE_DB
        private void FillSchPayments(SampleModel db, string accountId)
#else
        private void FillSchPayments(IdealContext db, string accountId)
#endif
        {
            var query = from pay in db.SCHEDULED_PAYMENT_TBL
                        where pay.ACC_ID == accountId
                        orderby pay.SCHPAY_DATE
                        select pay;
            if (!query.Any()) { return; }
            listSchPayments = new List<SCHEDULED_PAYMENT_TBL>();
            SchPayments = new ObservableCollection<ScheduledPaymentModel>();
            foreach (var r in query)
            {
                listSchPayments.Add(r);
                SchPayments.Add(new ScheduledPaymentModel()
                {
                    ACC_ID = r.ACC_ID,
                    SCHPAY_DATE = String.Format("{0:ddd MMM dd, yyyy}", r.SCHPAY_DATE),
                    SCHPAY_AMOUNT = r.SCHPAY_AMOUNT
                });
            }
            RaisePropertyChanged("SchPayments");
        }

        /// <summary>
        /// Fill the account list and accountIDs list
        /// </summary>
        /// <param name="db"></param>
#if USE_SAMPLE_DB
        private void FillAccounts(SampleModel db)
#else
        private void FillAccounts(IdealContext db)
#endif
        {
            var query = from account in db.ACCOUNT_CLIENT_SCHEDULED_PAYMENTS_VIEW
                        orderby account.ACC_ID
                        select account;
            if (!query.Any()) { return; }
            Accounts = new ObservableCollection<ACCOUNT_CLIENT_SCHEDULED_PAYMENTS_VIEW>();
            AccountIDs = new ObservableCollection<string>();
            foreach (var r in query)
            {
                Accounts.Add(r);
                AccountIDs.Add(r.ACC_ID);
            }
        }

        private void FillDetails(string accountId)
        {
            var acc = Accounts.Where(x => x.ACC_ID == accountId).First();
            Details = new ObservableCollection<DetailModel>();
            Details.Add(new DetailModel() { Item = "ACC_ID", Value = acc.ACC_ID });
            Details.Add(new DetailModel() { Item = "NAME", Value = acc.NAME });
            Details.Add(new DetailModel() { Item = "ITEM_NAME", Value = acc.ITEM_NAME });
            Details.Add(new DetailModel() { Item = "PAYMD_ID", Value = acc.PAYMD_ID });
            Details.Add(new DetailModel() { Item = "ACC_DATE", Value = String.Format("{0:ddd MMM dd, yyyy}", acc.ACC_DATE) });
            Details.Add(new DetailModel() { Item = "ACC_DISTANCE", Value = acc.ACC_DISTANCE.ToString() });
            Details.Add(new DetailModel() { Item = "CALCULATED_X_TIMES_ITEM_BUY_PRICE", Value = acc.CALCULATED_X_TIMES_ITEM_BUY_PRICE.ToString() });
            Details.Add(new DetailModel() { Item = "ACC_PRICE", Value = acc.ACC_PRICE.ToString() });
            Details.Add(new DetailModel() { Item = "ACC_HITCH", Value = acc.ACC_HITCH.ToString() });
            Details.Add(new DetailModel() { Item = "ACC_INITIAL_BALANCE", Value = acc.ACC_INITIAL_BALANCE.ToString() });
            Details.Add(new DetailModel() { Item = "CALCULATED_CURRENT_BALANCE", Value = acc.CALCULATED_CURRENT_BALANCE.ToString() });
            Details.Add(new DetailModel() { Item = "CALCULATED_GAIN", Value = acc.CALCULATED_GAIN.ToString() });
            Details.Add(new DetailModel() { Item = "ITEM_BUY_PRICE", Value = acc.ITEM_BUY_PRICE.ToString() });
            Details.Add(new DetailModel() { Item = "PAYMENTS", Value = acc.PAYMENTS.ToString() });
            Details.Add(new DetailModel() { Item = "SUM_SCHEDULED_PAYMENTS", Value = acc.SUM_SCHEDULED_PAYMENTS.ToString() });
            Details.Add(new DetailModel() { Item = "DIFF_SCHEDULED", Value = acc.DIFF_SCHEDULED.ToString() });
            RaisePropertyChanged("Details");
        }

        private void FillStatusValues(string accountId)
        {
            //Find the account
            var acc = Accounts.Where(x => x.ACC_ID == accountId).First();
            if (acc == null) { return; }
            PositiveAmount = (double)acc.PAYMENTS; // Collected
            NegativeAmount = (acc.SUM_SCHEDULED_PAYMENTS - acc.PAYMENTS > 0) ?
                            (double)(acc.SUM_SCHEDULED_PAYMENTS - acc.PAYMENTS) : 0; //Lag
            BalanceAmount = (double)acc.CALCULATED_CURRENT_BALANCE; //Balance

            int l = acc.PAYMD_ID.IndexOf("x");
            string str = (l > 0) ? acc.PAYMD_ID.Substring(0, l) : "";
            NoNegativeTransactions = (int)(NegativeAmount / Int32.Parse(str));
            NoPositiveTransactions = listPayments.Count;
        }


        private void FillTotalProgress(string accountId)
        {
            //Calculate account information
            var acc = Accounts.Where(x => x.ACC_ID == accountId).First();
            if (acc == null) { return; }
            TotalProgress = new ObservableCollection<Model>();
            TotalProgress.Add(new Model()
            {
                Item = "Balance",
                Units = (int)acc.CALCULATED_CURRENT_BALANCE
            });
            TotalProgress.Add(new Model() { Item = "Collected", Units = (int)acc.PAYMENTS });
            RaisePropertyChanged("TotalProgress");
        }

        private void FillTotalLag(string accountId)
        {
            //Calculate account information
            var acc = Accounts.Where(x => x.ACC_ID == accountId).First();
            if (acc == null) { return; }
            TotalLag = new ObservableCollection<Model>();
            TotalLag.Add(new Model()
            {
                Item = "Expected",
                Units = (acc.SUM_SCHEDULED_PAYMENTS - acc.PAYMENTS > 0) ?
                            (int)(acc.SUM_SCHEDULED_PAYMENTS - acc.PAYMENTS) : 0
            });
            TotalLag.Add(new Model() { Item = "Collected", Units = (int)acc.PAYMENTS });
            RaisePropertyChanged("TotalLag");
        }

        /// <summary>
        /// Fill the total values using the accountId.
        /// Reads from the Accounts list
        /// </summary>
        /// <param name="accountId"></param>
        private void FillTotals(string accountId)
        {
            //Find the account
            var acc = Accounts.Where(x => x.ACC_ID == accountId).First();
            if (acc == null) { return; }
            Totals = new ObservableCollection<Model>();
            Totals.Add(new Model() { Item = "Expected", Units = (int)acc.SUM_SCHEDULED_PAYMENTS });
            Totals.Add(new Model() { Item = "Invested", Units = (int)acc.ITEM_BUY_PRICE });
            Totals.Add(new Model() { Item = "Collected", Units = (int)acc.PAYMENTS });
            RaisePropertyChanged("Totals");
        }

        private void FillPaymentComparationSeries(string accountId)
        {
            PaymentComparation = new ObservableCollection<ItemModel>();
            // Iterate in the payments list and add the sum of payments to the collected section
            TimeSpan period = listSchPayments.ElementAt(1).SCHPAY_DATE - listSchPayments.ElementAt(0).SCHPAY_DATE;
            for (int i = 0; i < listSchPayments.Count; i++)
            {
                DateTime minDate = listSchPayments.ElementAt(i).SCHPAY_DATE;
                DateTime maxDate = minDate + period;
                if (i == 0)
                    minDate = minDate - period - period;
                var sumPay = from p in listPayments
                             where (p.PAY_DATE < maxDate && p.PAY_DATE >= minDate)
                             group p by p.PAY_DATE into g
                             select g.Sum(p => p.PAY_AMOUNT);
                int? collected = null;
                if (sumPay != null && sumPay.Any())
                    collected = (int)sumPay.ElementAt(0);

                // Add item to the list
                PaymentComparation.Add(new ItemModel()
                {
                    Item = String.Format("{0:MMM dd, yyyy}", listSchPayments.ElementAt(i).SCHPAY_DATE),
                    Expected = (int)listSchPayments.ElementAt(i).SCHPAY_AMOUNT,
                    Collected = collected
                });
            }

            // Query the payments after the scheduled date
            var query = from p in listPayments
                        where (p.PAY_DATE > listSchPayments.Last().SCHPAY_DATE + period)
                        select p;
            foreach (var r in query)
            {
                PaymentComparation.Add(new ItemModel()
                {
                    Item = String.Format("{0:MMM dd, yyyy}", r.PAY_DATE),
                    Expected = null,
                    Collected = (int)r.PAY_AMOUNT
                });
            }
            RaisePropertyChanged("PaymentComparation");
        }
    }
}
