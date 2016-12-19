#region Copyright
/*
 * Copyright (C) 2016 Angel Garcia
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
using Ideal.DBModel;

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
    /// Class to represent Month and Week comparations
    /// </summary>
    public class ItemModel
    {
        public string Item { get; set; }
        public int? Expected { get; set; }
        public int? Collected { get; set; }
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
        public ObservableCollection<ACCOUNT_CLIENT_SCHEDULED_PAYMENTS_VIEW> Accounts { get; set; }
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

        // Private list to save content of tables
        private List<SCHEDULED_PAYMENT_TBL> listScheduledPay;
        private List<PAYMENT_TBL> listPayments;


        public RunningAccountsViewModel()
        {
            using (var db = new IdealContext())
            {
                try
                {
                    db.Database.Connection.Open();
                    FillAccountSeries(db);
                    FillPaymentLists(db);
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
        private void FillAccountSeries(IdealContext db)
        {
            var query = from account in db.ACCOUNT_CLIENT_SCHEDULED_PAYMENTS_VIEW
                        orderby account.ACC_ID
                        select account;
            if (!query.Any())
            {
                return; // No results 
            }
            Accounts = new ObservableCollection<ACCOUNT_CLIENT_SCHEDULED_PAYMENTS_VIEW>();
            CostComparations = new ObservableCollection<CostComparationModel>();
            foreach (var acc in query)
            {
                Accounts.Add(acc);     //Create the list of accounts
                CostComparations.Add(  // Create a list of CostComparations
                    new CostComparationModel()
                    {
                        AccountID = acc.ACC_ID,
                        ItemPrice = (double)acc.ITEM_BUY_PRICE,
                        Collected = (double)acc.PAYMENTS,
                        Expected = (double)acc.SUM_SCHEDULED_PAYMENTS
                    });
            }
        }

        /// <summary>
        /// Store tables in temporal Lists to allow LINQ grouping queries.
        /// </summary>
        /// <param name="db"></param>
        private void FillPaymentLists(IdealContext db)
        {
            // Get Scheduled Payments table
            listScheduledPay = new List<SCHEDULED_PAYMENT_TBL>();
            var queryScheduledPay = from p in db.SCHEDULED_PAYMENT_TBL
                                    select p;
            foreach (var p in queryScheduledPay)
            {
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
        /// Query the lists and fill the series.
        /// NOTE: DataEntity didn't generate all views from db (3 views missing).
        /// </summary>
        private void FillMonthSeries()
        {
            List<ItemModel> listMonth = new List<ItemModel>();
            CalculateScheduledMonthPayments(listMonth);
            CalculateMonthPayments(listMonth);

            // Add to the Observable list
            Months = new ObservableCollection<ItemModel>();
            foreach (var item in listMonth)
                Months.Add(item);
        }

        private void CalculateScheduledMonthPayments(List<ItemModel> listMonth)
        {
            var result = from k in listScheduledPay // work around: group by doest work with table
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

        private void CalculateMonthPayments(List<ItemModel> listMonth)
        {
            var result = from r in listPayments  // work around: group by doest work with table
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

        private void FillTotalProgress()
        {
            TotalProgress = new ObservableCollection<Model>();
            TotalProgress.Add(new Model()
            {
                Item = "Balance",
                Units = (int)Accounts.Sum(x => x.CALCULATED_CURRENT_BALANCE)
            });
            TotalProgress.Add(new Model()
            {
                Item = "Collected",
                Units = (int)Accounts.Sum(x => x.PAYMENTS)
            });
        }

        /// <summary>
        /// Fill the lag values for the running accounts
        /// </summary>
        /// <param name="db"></param>
        private void FillTotalLag(IdealContext db)
        {
            TotalLag = new ObservableCollection<Model>();
            TotalLag.Add(new Model()
            {
                Item = "Expected",
                Units = (Accounts.Sum(x => x.SUM_SCHEDULED_PAYMENTS) - Accounts.Sum(x => x.PAYMENTS) > 0) ?
                            (int)(Accounts.Sum(x => x.SUM_SCHEDULED_PAYMENTS) - Accounts.Sum(x => x.PAYMENTS)) : 0
            });
            TotalLag.Add(new Model()
            {
                Item = "Collected",
                Units = (int)Accounts.Sum(x => x.PAYMENTS)
            });
        }

        private void FillStatusValues()
        {
            PositiveAmount = (int)Accounts.Sum(x => x.PAYMENTS); // Collected
            NegativeAmount = (Accounts.Sum(x => x.SUM_SCHEDULED_PAYMENTS) - Accounts.Sum(x => x.PAYMENTS) > 0) ?
                                (int)(Accounts.Sum(x => x.SUM_SCHEDULED_PAYMENTS) - Accounts.Sum(x => x.PAYMENTS)) : 0; //Lag
            BalanceAmount = (int)Accounts.Sum(x => x.CALCULATED_CURRENT_BALANCE); //Balance
        }

        /// <summary>
        /// Fill the totals values for all accounts
        /// </summary>
        /// <param name="db"></param>
        private void FillTotals(IdealContext db)
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
            CalculateScheduledWeekPayments(listWeek);
            CalculateWeekPayments(listWeek);

            // Add to the Observable list
            Weeks = new ObservableCollection<ItemModel>();
            foreach (var item in listWeek)
                Weeks.Add(item);
        }

        private void CalculateScheduledWeekPayments(List<ItemModel> listWeek)
        {
            var result = from k in listScheduledPay // work around: group by doest work with table
                         group k by new
                         {
                             y = k.SCHPAY_DATE.Year,
                             w = WeekOfYearISO8601(k.SCHPAY_DATE)
                         } into g
                         select new
                         {
                             Year = g.Key.y,
                             Week = g.Key.w,
                             FirstWeekDay = FirstDateOfWeek(g.Key.y, g.Key.w, CultureInfo.CurrentCulture),
                             //LastWeekDay = FirstDateOfWeek(g.Key.y, g.Key.w, CultureInfo.CurrentCulture).AddDays(6),
                             Sum = g.Sum(p => p.SCHPAY_AMOUNT)
                         };

            // DateTime firstDayOfWeek= FirstDateOfWeek(2013, thisWeekNumber, CultureInfo.CurrentCulture);

            foreach (var p in result)
            {
                listWeek.Add(new ItemModel() // Add all items
                {
                    //Item = p.Year.ToString() + "-" + p.Week.ToString() +  p.FirstWeekDay,
                    Item = String.Format("{0} {1:MMMdd}", p.Year, p.FirstWeekDay),
                    Expected = (int)p.Sum,
                    Collected = null
                });
            }
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



        // Source http://stackoverflow.com/questions/19901666/get-date-of-first-and-last-day-of-week-knowing-week-number
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


        private void CalculateWeekPayments(List<ItemModel> listWeek)
        {
            var result = from r in listPayments  // work around: group by doest work with table
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
                             //LastWeekDay = FirstDateOfWeek(g.Key.y, g.Key.w, CultureInfo.CurrentCulture).AddDays(6),
                             Sum = g.Sum(p => p.PAY_AMOUNT)
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
                        Collected = (int)p.Sum
                    });
                }
            }
        }


    }
}