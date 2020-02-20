#region Copyright
/*
 * Copyright (C) 2017 Angel Newton
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
using System.Globalization;
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
    /// <summary>
    /// Used to store data for displaying in CostComparations chart.
    /// NOTE: Parameter names are used in chart
    /// </summary>
    public class CostComparationModel
    {
        public string AccountID { get; set; }
        public double? ItemPrice { get; set; }
        public double? Collected { get; set; }
        public double? Expected { get; set; }
    }

    /// <summary>
    /// Represents an account item on the Running Accounts page. 
    /// </summary>
    public class AccountModel
    {
        public string AccountID { get; set; }
        public string ClientName { get; set; }
        public string Item { get; set; }
        public string PaymentID { get; set; }
        public string Date { get; set; }
        public double? AccountPrice { get; set; }
        public double? Hitch { get; set; }
        public double? Balance { get; set; }
        public double? ItemPrice { get; set; }
        public double? Payments { get; set; }
        public double? ScheduledPayments { get; set; }
        public double? DiffScheduled { get; set; }
    }

    /// <summary>
    /// Class to represent Month and Week comparations
    /// </summary>
    public class ItemModel
    {
        public string Item { get; set; }
        public int? Expected { get; set; }
        public int? Collected { get; set; }
        public int? Order { get; set; } //Order of sorting in the list
    }


    /// <summary>
    /// Model for Total costs
    /// </summary>
    public class Model
    {
        public int? Units { get; set; }
        public string Item { get; set; }
    }


    /// <summary>
    ///  Series of data for the charts in *.xaml file
    /// </summary>
    class RunningAccountsViewModel
    {

        public ObservableCollection<CostComparationModel> CostComparations { get; set; }
        public ObservableCollection<AccountModel> Accounts { get; set; }
        public ObservableCollection<ItemModel> Months { get; set; }
        public ObservableCollection<Model> TotalProgress { get; set; }
        public ObservableCollection<Model> TotalLag { get; set; }
        public ObservableCollection<Model> Totals { get; set; } // Represent the running account totals
        public ObservableCollection<ItemModel> Weeks { get; set; }

        private double _positiveAmount;
        public double PositiveAmount
        {
            get { return _positiveAmount; }
            set { _positiveAmount = value; }
        }

        private double _negativeAmount;
        public double NegativeAmount
        {
            get { return _negativeAmount; }
            set { _negativeAmount = value; }
        }

        private double _balanceAmount;
        public double BalanceAmount
        {
            get { return _balanceAmount; }
            set { _balanceAmount = value; }
        }

        // Helper for making calculations
        RunningAccountsHelper util = new RunningAccountsHelper();

        /// <summary>
        /// Constructor.
        /// </summary>
        public RunningAccountsViewModel()
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
                    FillAccountSeries(db);
                    util.FillPaymentLists(db);
                    FillMonthSeries();
                    FillWeekSeries();
                    FillTotalProgress();
                    FillTotalLag(db);
                    FillStatusValues();
                    FillTotals(db);
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    MessageBox.Show(e.Message, "Database connection error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        /// <summary>
        /// Query the Accounts view and fill Accounts and CostComparations series.
        /// </summary>
#if USE_SAMPLE_DB
        private void FillAccountSeries(SampleModel db)
#else
        private void FillAccountSeries(IdealContext db)
#endif
        {
            var query = from a in db.ACCOUNT_CLIENT_SCHEDULED_PAYMENTS_VIEW
                        orderby a.ACC_ID
                        select new { a.ACC_ID, a.NAME, a.ITEM_NAME, a.PAYMD_ID, a.ACC_DATE, a.ACC_PRICE,
                                     a.ACC_HITCH, a.CALCULATED_CURRENT_BALANCE, a.ITEM_BUY_PRICE, a.PAYMENTS,
                                     a.SUM_SCHEDULED_PAYMENTS, a.DIFF_SCHEDULED};
            if (!query.Any())
            {
                return; // No results 
            }
            Accounts = new ObservableCollection<AccountModel>();
            CostComparations = new ObservableCollection<CostComparationModel>();
            foreach (var acc in query)
            {
                Accounts.Add( //Create the list of accounts
                    new AccountModel()
                    {
                        AccountID = acc.ACC_ID,
                        ClientName = acc.NAME,
                        Item = acc.ITEM_NAME,
                        PaymentID = acc.PAYMD_ID,
                        Date = acc.ACC_DATE.ToString("MMM dd yyyy"),
                        AccountPrice = (double) acc.ACC_PRICE,
                        Hitch = (double) acc.ACC_HITCH,
                        Balance = (double) acc.CALCULATED_CURRENT_BALANCE,
                        ItemPrice = (double) acc.ITEM_BUY_PRICE,
                        Payments = (double) acc.PAYMENTS,
                        ScheduledPayments = (double) acc.SUM_SCHEDULED_PAYMENTS,
                        DiffScheduled = (double) acc.DIFF_SCHEDULED
                    });
                CostComparations.Add(  // Create a list of CostComparations
                    new CostComparationModel()
                    {
                        AccountID = acc.ACC_ID,
                        ItemPrice = (double) acc.ITEM_BUY_PRICE,
                        Collected = (double) acc.PAYMENTS,
                        Expected = (double) acc.SUM_SCHEDULED_PAYMENTS
                    });
            }
        }


        /// <summary>
        /// Query the lists and fill the series.
        /// NOTE: DataEntity didn't generate all views from db (3 views missing).
        /// </summary>
        private void FillMonthSeries()
        {
            List<ItemModel> listMonth = new List<ItemModel>();
            util.CalculateScheduledMonthPayments(listMonth);
            util.CalculateMonthPayments(listMonth);

            // Add to the Observable list
            Months = new ObservableCollection<ItemModel>();
            foreach (var item in listMonth)
                Months.Add(item);
        }

        
        private void FillTotalProgress()
        {
            TotalProgress = new ObservableCollection<Model>();
            TotalProgress.Add(new Model()
            {
                Item = "Balance",
                Units = (int)Accounts.Sum(x => x.Balance)
            });
            TotalProgress.Add(new Model()
            {
                Item = "Collected",
                Units = (int)Accounts.Sum(x => x.Payments)
            });
        }

        /// <summary>
        /// Fill the lag values for the running accounts
        /// </summary>
        /// <param name="db"></param>
#if USE_SAMPLE_DB
        private void FillTotalLag(SampleModel db)
#else
        private void FillTotalLag(IdealContext db)
#endif
        {
            TotalLag = new ObservableCollection<Model>();
            TotalLag.Add(new Model()
            {
                Item = "Expected",
                Units = (Accounts.Sum(x => x.ScheduledPayments) - Accounts.Sum(x => x.Payments) > 0) ?
                            (int)(Accounts.Sum(x => x.ScheduledPayments) - Accounts.Sum(x => x.Payments)) : 0
            });
            TotalLag.Add(new Model()
            {
                Item = "Collected",
                Units = (int)Accounts.Sum(x => x.Payments)
            });
        }

        private void FillStatusValues()
        {
            PositiveAmount = (int)Accounts.Sum(x => x.Payments); // Collected
            NegativeAmount = (Accounts.Sum(x => x.ScheduledPayments) - Accounts.Sum(x => x.Payments) > 0) ?
                                (int)(Accounts.Sum(x => x.ScheduledPayments) - Accounts.Sum(x => x.Payments)) : 0; //Lag
            BalanceAmount = (int)Accounts.Sum(x => x.Balance); //Balance
        }

        /// <summary>
        /// Fill the totals values for all accounts
        /// </summary>
        /// <param name="db"></param>
#if USE_SAMPLE_DB
        private void FillTotals(SampleModel db)
#else
        private void FillTotals(IdealContext db)
#endif
        {
            Totals = new ObservableCollection<Model>();
            var result = from r in db.TOTAL_INVESTED_VS_RECOVERED_VS_SCHEDULED_VIEW
                         select r;
            foreach (var t in result)
            {
                string description = null;
                if (t.DESCRIPTION.Contains("Scheduled")) { description = "Expected"; }
                else if (t.DESCRIPTION.Contains("ITEM")) { description = "Invested"; }
                else { description = "Collected"; }

                Totals.Add(new Model() { Item = description, Units = (int)t.VALUE });
            }
        }

        /// <summary>
        /// Query the lists and fill the Week series.
        /// NOTE: DataEntity didn't generate all views from db (3 views missing).
        /// </summary>
        private void FillWeekSeries()
        {
            List<ItemModel> listWeek = new List<ItemModel>();
            util.CalculateScheduledWeekPayments(listWeek);
            util.CalculateWeekPayments(listWeek);
            // Order by date number
            listWeek = listWeek.OrderBy(o => o.Order).ToList();

            // Add to the Observable list
            Weeks = new ObservableCollection<ItemModel>();
            foreach (var item in listWeek)
                Weeks.Add(item);
        }
    }



    /// <summary>
    /// Helper class for the RunningAccountsViewModel
    /// </summary>
    public class RunningAccountsHelper
    {

        // Private list to save content of tables
        private List<SCHEDULED_PAYMENT_TBL> listScheduledPay;
        private List<PAYMENT_TBL> listPayments;


        /// <summary>
        /// Store tables in temporal Lists to allow LINQ grouping queries.
        /// </summary>
        /// <param name="db"></param>
#if USE_SAMPLE_DB
        public void FillPaymentLists(SampleModel db)
#else
        public void FillPaymentLists(IdealContext db)
#endif
        {
            var accounts = new List<String>();
            var queryAccounts = from a in db.ACCOUNT_CLIENT_SCHEDULED_PAYMENTS_VIEW
                                select a.ACC_ID;
            foreach (var a in queryAccounts)
                accounts.Add(a);

            // Get Scheduled Payments table
            listScheduledPay = new List<SCHEDULED_PAYMENT_TBL>();
            var queryScheduledPay = from p in db.SCHEDULED_PAYMENT_TBL
                                    select p;
            foreach (var p in queryScheduledPay)
            {
               //if (accounts.Any(p.ACC_ID.Contains))
                    listScheduledPay.Add(p);
            }

            // Get Payments table
            listPayments = new List<PAYMENT_TBL>();
            var queryPayments = from p in db.PAYMENT_TBL
                                select p;
            foreach (var p in queryPayments)
            {
                listPayments.Add(p);
            }
        }

        /// <summary>
        /// Generates a list of scheduled month payments.
        /// </summary>
        /// <param name="listMonth"></param> The generated list
        public void CalculateScheduledMonthPayments(List<ItemModel> listMonth)
        {
            var result = from k in listScheduledPay // work around: group by doesn't work with table
                         group k by new
                         {
                             y = k.SCHPAY_DATE.Year,
                             m = k.SCHPAY_DATE.Month
                         } into g
                         select new
                         {
                             Month = new DateTime(g.Key.y, g.Key.m, 1),
                             Sum = g.Sum(p => p.SCHPAY_AMOUNT)
                         };
            foreach (var p in result)
            {
                listMonth.Add(new ItemModel() // Add all items
                {
                    Item = p.Month.ToString("yyyy MMM"),
                    Expected = (int)p.Sum,
                    Collected = null
                });
            }
        }

        /// <summary>
        /// Generates a list of the month payments.
        /// </summary>
        /// <param name="listMonth"></param>
        public void CalculateMonthPayments(List<ItemModel> listMonth)
        {
            var result = from r in listPayments  // work around: group by doesn't work with table
                         group r by new
                         {
                             y = r.PAY_DATE.Year,
                             m = r.PAY_DATE.Month
                         } into g
                         select new
                         {
                             Month = new DateTime(g.Key.y, g.Key.m, 1),
                             Sum = g.Sum(p => p.PAY_AMOUNT)
                         };
            foreach (var p in result)
            {
                string str = p.Month.ToString("yyyy MMM");
                ItemModel month = listMonth.Find(x => x.Item.Equals(str));
                if (month != null)
                {
                    month.Collected = (int)p.Sum;
                }
                else
                {
                    listMonth.Add(new ItemModel() // Add item
                    {
                        Item = str,
                        Expected = null,
                        Collected = (int)p.Sum
                    });
                }
            }
        }

        /// <summary>
        /// Generates a list of the week payments.
        /// </summary>
        /// <param name="listWeek"></param>
        public void CalculateWeekPayments(List<ItemModel> listWeek)
        {
            var result = from r in listPayments  // work around: group by doesn't work with table
                         group r by new
                         {
                             y = r.PAY_DATE.Year,
                             w = WeekOfYearISO8601(r.PAY_DATE)
                         } into g
                         select new
                         {
                             Year = g.Key.y,
                             Week = g.Key.w,
                             FirstWeekDay = FirstDateOfWeek(g.Key.y, g.Key.w, CultureInfo.CurrentCulture),
                             Sum = g.Sum(p => p.PAY_AMOUNT),
                             Order = (g.Key.y * 1000) + g.Key.w  // sorting order
                         };
            foreach (var p in result)
            {
                string str = String.Format("{0} {1:MMMdd}", p.Year, p.FirstWeekDay);
                ItemModel month = listWeek.Find(x => x.Item.Equals(str));
                if (month != null)
                {
                    month.Collected = (int)p.Sum;
                }
                else
                {
                    listWeek.Add(new ItemModel() // Add item
                    {
                        Item = str,
                        Expected = null,
                        Collected = (int)p.Sum,
                        Order = p.Order
                    });
                }
            }
        }

        /// <summary>
        /// Generates a list of the scheduled week payments.
        /// </summary>
        /// <param name="listWeek"></param>
        public void CalculateScheduledWeekPayments(List<ItemModel> listWeek)
        {
            var result = from k in listScheduledPay // work around: group by doesn't work with table
                         group k by new
                         {
                             y = k.SCHPAY_DATE.Year,
                             w = WeekOfYearISO8601(k.SCHPAY_DATE),
                         } into g
                         select new
                         {
                             Year = g.Key.y,
                             Week = g.Key.w,
                             FirstWeekDay = FirstDateOfWeek(g.Key.y, g.Key.w, CultureInfo.CurrentCulture),
                             Sum = g.Sum(p => p.SCHPAY_AMOUNT),
                             Order = (g.Key.y * 1000) + g.Key.w  // sorting order
                         };

            foreach (var p in result) // Add all items
            {
                ItemModel item = new ItemModel() // Create item
                {
                    Item = String.Format("{0} {1:MMMdd}", p.Year, p.FirstWeekDay),
                    Expected = (int)p.Sum,
                    Collected = null,
                    Order = p.Order
                };
                listWeek.Add(item);
            }
        }

        /// <summary>
        /// Get date of first and last day of week knowing week number.
        /// Source http://stackoverflow.com/questions/19901666/get-date-of-first-and-last-day-of-week-knowing-week-number
        /// </summary>
        /// <param name="year"></param>
        /// <param name="weekOfYear"></param>
        /// <param name="ci"></param>
        /// <returns></returns>
        public static DateTime FirstDateOfWeek(int year, int weekOfYear, System.Globalization.CultureInfo ci)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = (int)ci.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;
            DateTime firstWeekDay = jan1.AddDays(daysOffset);
            int firstWeek = ci.Calendar.GetWeekOfYear(jan1, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
            if ((firstWeek <= 1 || firstWeek >= 52) && daysOffset >= -3)
            {
                weekOfYear -= 1;
            }
            return firstWeekDay.AddDays(weekOfYear * 7);
        }

        /// <summary>
        /// Calculate the week number based on a given date.
        /// Source: http://stackoverflow.com/questions/1497586/how-can-i-calculate-find-the-week-number-of-a-given-date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int WeekOfYearISO8601(DateTime date)
        {
            var day = (int)CultureInfo.CurrentCulture.Calendar.GetDayOfWeek(date);
            return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date.AddDays(4 - (day == 0 ? 7 : day)),
                CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
    }

}