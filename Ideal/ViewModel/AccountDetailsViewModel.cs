using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ideal
{

    public class PaymentModel
    {
        public string ACC_ID { get; set; }
        public string PAY_DATE { get; set; }
        public decimal PAY_AMOUNT { get; set; }
        public Nullable<int> PAY_VISITS { get; set; }
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
        public ObservableCollection<Model> Totals { get; set; }
        public ObservableCollection<PaymentModel> Payments { get; set; }


        private List<PAYMENT_TBL> listPayments;

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
                    using (var db = new IdealContext())
                    {
                        try
                        {
                            db.Database.Connection.Open();
                            FillPayments(db, selectedItemAccountID);

                            FillDetails(selectedItemAccountID);
                            FillStatusValues(selectedItemAccountID);
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
            using (var db = new IdealContext())
            {
                try
                {
                    db.Database.Connection.Open();
                    FillAccounts(db);
                    selectedItemAccountID = AccountIDs.ElementAt(0);
                    FillPayments(db, selectedItemAccountID);
                    FillDetails(selectedItemAccountID);
                    FillStatusValues(selectedItemAccountID);
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
        public void FillPayments(IdealContext db, string accountId)
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
        /// Fill the account list and accountIDs list
        /// </summary>
        /// <param name="db"></param>
        private void FillAccounts(IdealContext db)
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


    }
}
