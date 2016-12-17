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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ideal
{

    /// <summary>
    /// Representation of the Running Accounts page
    /// </summary>
    class RunningAccountsViewModel
    {

        public class CostComparationModel
        {
            public string AccountID { get; set; }
            public double? ItemPrice { get; set; }
            public double? Collected { get; set; }
            public double? Expected { get; set; }
        }


        public ObservableCollection<CostComparationModel> CostComparations { get; set; }
        public ObservableCollection<ACCOUNT_CLIENT_SCHEDULED_PAYMENTS_VIEW> Accounts { get; set; }

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




        public RunningAccountsViewModel()
        {
            using (var db = new IdealContext())
            {
                try
                {
                    db.Database.Connection.Open();
                    FillAccountSeries(db);
                    FillStatusValues();
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

        private void FillStatusValues()
        {
            PositiveAmount = (int)Accounts.Sum(x => x.PAYMENTS); // Collected
            NegativeAmount = (Accounts.Sum(x => x.SUM_SCHEDULED_PAYMENTS) - Accounts.Sum(x => x.PAYMENTS) > 0) ?
                                (int)(Accounts.Sum(x => x.SUM_SCHEDULED_PAYMENTS) - Accounts.Sum(x => x.PAYMENTS)) : 0; //Lag
            BalanceAmount = (int)Accounts.Sum(x => x.CALCULATED_CURRENT_BALANCE); //Balance
        }

    }
}