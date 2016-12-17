namespace Ideal
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class IdealContext : DbContext
    {
        public IdealContext()
            : base("name=IdealContext")
        {
        }

        public virtual DbSet<ACCOUNT_TBL> ACCOUNT_TBL { get; set; }
        public virtual DbSet<ADDRESS_TBL> ADDRESS_TBL { get; set; }
        public virtual DbSet<CLIENT_TBL> CLIENT_TBL { get; set; }
        public virtual DbSet<EMPLOYEE_TBL> EMPLOYEE_TBL { get; set; }
        public virtual DbSet<ITEM_TBL> ITEM_TBL { get; set; }
        public virtual DbSet<PAYMENT_MODE_TBL> PAYMENT_MODE_TBL { get; set; }
        public virtual DbSet<PAYMENT_TBL> PAYMENT_TBL { get; set; }
        public virtual DbSet<PROVIDER_TBL> PROVIDER_TBL { get; set; }
        public virtual DbSet<REFERENCE_TBL> REFERENCE_TBL { get; set; }
        public virtual DbSet<SCHEDULED_PAYMENT_TBL> SCHEDULED_PAYMENT_TBL { get; set; }
        public virtual DbSet<ACCOUNT_CLIENT_SCHEDULED_PAYMENTS_VIEW> ACCOUNT_CLIENT_SCHEDULED_PAYMENTS_VIEW { get; set; }
        public virtual DbSet<PAYMENTS_VIEW> PAYMENTS_VIEW { get; set; }
        public virtual DbSet<TOTAL_INVESTED_VS_RECOVERED_VS_SCHEDULED_VIEW> TOTAL_INVESTED_VS_RECOVERED_VS_SCHEDULED_VIEW { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ACCOUNT_TBL>()
                .Property(e => e.ACC_ID)
                .IsUnicode(false);

            modelBuilder.Entity<ACCOUNT_TBL>()
                .Property(e => e.CLI_ID)
                .HasPrecision(10, 0);

            modelBuilder.Entity<ACCOUNT_TBL>()
                .Property(e => e.ITEM_ID)
                .HasPrecision(10, 0);

            modelBuilder.Entity<ACCOUNT_TBL>()
                .Property(e => e.PAYMD_ID)
                .IsUnicode(false);

            modelBuilder.Entity<ACCOUNT_TBL>()
                .Property(e => e.ACC_STATUS)
                .IsUnicode(false);

            modelBuilder.Entity<ACCOUNT_TBL>()
                .Property(e => e.ACC_PRICE)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ACCOUNT_TBL>()
                .Property(e => e.ACC_HITCH)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ACCOUNT_TBL>()
                .Property(e => e.ACC_INITIAL_BALANCE)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ACCOUNT_TBL>()
                .Property(e => e.ACC_CURRENT_BALANCE)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ACCOUNT_TBL>()
                .Property(e => e.ACC_DISTANCE)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ACCOUNT_TBL>()
                .Property(e => e.ACC_SELLER_ID)
                .IsUnicode(false);

            modelBuilder.Entity<ACCOUNT_TBL>()
                .Property(e => e.ACC_NOTES)
                .IsUnicode(false);

            modelBuilder.Entity<ACCOUNT_TBL>()
                .HasMany(e => e.PAYMENT_TBL)
                .WithRequired(e => e.ACCOUNT_TBL)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ACCOUNT_TBL>()
                .HasMany(e => e.SCHEDULED_PAYMENT_TBL)
                .WithRequired(e => e.ACCOUNT_TBL)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ADDRESS_TBL>()
                .Property(e => e.ADDR_ID)
                .HasPrecision(10, 0);

            modelBuilder.Entity<ADDRESS_TBL>()
                .Property(e => e.ADDR_STREET_NUMBER)
                .IsUnicode(false);

            modelBuilder.Entity<ADDRESS_TBL>()
                .Property(e => e.ADDR_NEIGHBORHOOD)
                .IsUnicode(false);

            modelBuilder.Entity<ADDRESS_TBL>()
                .Property(e => e.ADDR_CITY)
                .IsUnicode(false);

            modelBuilder.Entity<ADDRESS_TBL>()
                .Property(e => e.ADDR_STATE)
                .IsUnicode(false);

            modelBuilder.Entity<ADDRESS_TBL>()
                .Property(e => e.ADDR_COUNTRY)
                .IsUnicode(false);

            modelBuilder.Entity<ADDRESS_TBL>()
                .HasMany(e => e.CLIENT_TBL)
                .WithOptional(e => e.ADDRESS_TBL)
                .HasForeignKey(e => e.CLI_HOME_ADDR_ID);

            modelBuilder.Entity<ADDRESS_TBL>()
                .HasMany(e => e.CLIENT_TBL1)
                .WithOptional(e => e.ADDRESS_TBL1)
                .HasForeignKey(e => e.CLI_WORK_ADDR_ID);

            modelBuilder.Entity<ADDRESS_TBL>()
                .HasMany(e => e.PROVIDER_TBL)
                .WithOptional(e => e.ADDRESS_TBL)
                .HasForeignKey(e => e.PROV_ADDR_ID);

            modelBuilder.Entity<ADDRESS_TBL>()
                .HasMany(e => e.REFERENCE_TBL)
                .WithOptional(e => e.ADDRESS_TBL)
                .HasForeignKey(e => e.REF_ADDR_ID);

            modelBuilder.Entity<CLIENT_TBL>()
                .Property(e => e.CLI_ID)
                .HasPrecision(10, 0);

            modelBuilder.Entity<CLIENT_TBL>()
                .Property(e => e.CLI_NAMES)
                .IsUnicode(false);

            modelBuilder.Entity<CLIENT_TBL>()
                .Property(e => e.CLI_FAMILY_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<CLIENT_TBL>()
                .Property(e => e.CLI_HOME_PHONE)
                .IsUnicode(false);

            modelBuilder.Entity<CLIENT_TBL>()
                .Property(e => e.CLI_WORK_PHONE)
                .IsUnicode(false);

            modelBuilder.Entity<CLIENT_TBL>()
                .Property(e => e.CLI_HOME_ADDR_ID)
                .HasPrecision(10, 0);

            modelBuilder.Entity<CLIENT_TBL>()
                .Property(e => e.CLI_WORK_ADDR_ID)
                .HasPrecision(10, 0);

            modelBuilder.Entity<CLIENT_TBL>()
                .Property(e => e.CLI_OCUPATION)
                .IsUnicode(false);

            modelBuilder.Entity<CLIENT_TBL>()
                .Property(e => e.CLI_NOTES)
                .IsUnicode(false);

            modelBuilder.Entity<CLIENT_TBL>()
                .Property(e => e.CLI_PARTNER_NAMES)
                .IsUnicode(false);

            modelBuilder.Entity<CLIENT_TBL>()
                .Property(e => e.CLI_PARTNER_FAMILY_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<CLIENT_TBL>()
                .Property(e => e.CLI_REFERENCE_ID1)
                .HasPrecision(10, 0);

            modelBuilder.Entity<CLIENT_TBL>()
                .Property(e => e.CLI_REFERENCE_ID2)
                .HasPrecision(10, 0);

            modelBuilder.Entity<CLIENT_TBL>()
                .Property(e => e.CLI_SCORE)
                .IsUnicode(false);

            modelBuilder.Entity<CLIENT_TBL>()
                .HasMany(e => e.ACCOUNT_TBL)
                .WithRequired(e => e.CLIENT_TBL)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EMPLOYEE_TBL>()
                .Property(e => e.EMP_ID)
                .IsUnicode(false);

            modelBuilder.Entity<EMPLOYEE_TBL>()
                .Property(e => e.EMP_TYPE)
                .IsUnicode(false);

            modelBuilder.Entity<EMPLOYEE_TBL>()
                .Property(e => e.EMP_NAMES)
                .IsUnicode(false);

            modelBuilder.Entity<EMPLOYEE_TBL>()
                .Property(e => e.EMP_FAMILY_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<EMPLOYEE_TBL>()
                .HasMany(e => e.ACCOUNT_TBL)
                .WithRequired(e => e.EMPLOYEE_TBL)
                .HasForeignKey(e => e.ACC_SELLER_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EMPLOYEE_TBL>()
                .HasMany(e => e.PAYMENT_TBL)
                .WithRequired(e => e.EMPLOYEE_TBL)
                .HasForeignKey(e => e.PAY_COLLECTOR)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EMPLOYEE_TBL>()
                .HasMany(e => e.PAYMENT_TBL1)
                .WithRequired(e => e.EMPLOYEE_TBL1)
                .HasForeignKey(e => e.PAY_INSPECTOR)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ITEM_TBL>()
                .Property(e => e.ITEM_ID)
                .HasPrecision(10, 0);

            modelBuilder.Entity<ITEM_TBL>()
                .Property(e => e.PROV_ID)
                .IsUnicode(false);

            modelBuilder.Entity<ITEM_TBL>()
                .Property(e => e.ITEM_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<ITEM_TBL>()
                .Property(e => e.ITEM_BUY_PRICE)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ITEM_TBL>()
                .HasMany(e => e.ACCOUNT_TBL)
                .WithRequired(e => e.ITEM_TBL)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PAYMENT_MODE_TBL>()
                .Property(e => e.PAYMD_ID)
                .IsUnicode(false);

            modelBuilder.Entity<PAYMENT_MODE_TBL>()
                .Property(e => e.PAYMD_AMOUNT)
                .HasPrecision(10, 2);

            modelBuilder.Entity<PAYMENT_MODE_TBL>()
                .Property(e => e.PAYMD_FREQUENCY)
                .IsUnicode(false);

            modelBuilder.Entity<PAYMENT_MODE_TBL>()
                .Property(e => e.PAYMD_DAY)
                .IsUnicode(false);

            modelBuilder.Entity<PAYMENT_MODE_TBL>()
                .HasMany(e => e.ACCOUNT_TBL)
                .WithRequired(e => e.PAYMENT_MODE_TBL)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PAYMENT_TBL>()
                .Property(e => e.PAY_ID)
                .HasPrecision(20, 0);

            modelBuilder.Entity<PAYMENT_TBL>()
                .Property(e => e.ACC_ID)
                .IsUnicode(false);

            modelBuilder.Entity<PAYMENT_TBL>()
                .Property(e => e.PAY_AMOUNT)
                .HasPrecision(10, 2);

            modelBuilder.Entity<PAYMENT_TBL>()
                .Property(e => e.PAY_COLLECTOR)
                .IsUnicode(false);

            modelBuilder.Entity<PAYMENT_TBL>()
                .Property(e => e.PAY_INSPECTOR)
                .IsUnicode(false);

            modelBuilder.Entity<PROVIDER_TBL>()
                .Property(e => e.PROV_ID)
                .IsUnicode(false);

            modelBuilder.Entity<PROVIDER_TBL>()
                .Property(e => e.PROV_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<PROVIDER_TBL>()
                .Property(e => e.PROV_ADDR_ID)
                .HasPrecision(10, 0);

            modelBuilder.Entity<PROVIDER_TBL>()
                .Property(e => e.PROV_PHONE)
                .IsUnicode(false);

            modelBuilder.Entity<PROVIDER_TBL>()
                .Property(e => e.PROV_WEBSITE)
                .IsUnicode(false);

            modelBuilder.Entity<PROVIDER_TBL>()
                .Property(e => e.PROV_NOTES)
                .IsUnicode(false);

            modelBuilder.Entity<PROVIDER_TBL>()
                .HasMany(e => e.ITEM_TBL)
                .WithRequired(e => e.PROVIDER_TBL)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<REFERENCE_TBL>()
                .Property(e => e.REF_ID)
                .HasPrecision(10, 0);

            modelBuilder.Entity<REFERENCE_TBL>()
                .Property(e => e.REF_NAMES)
                .IsUnicode(false);

            modelBuilder.Entity<REFERENCE_TBL>()
                .Property(e => e.REF_FAMILY_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<REFERENCE_TBL>()
                .Property(e => e.REF_PHONE)
                .IsUnicode(false);

            modelBuilder.Entity<REFERENCE_TBL>()
                .Property(e => e.REF_ADDR_ID)
                .HasPrecision(10, 0);

            modelBuilder.Entity<REFERENCE_TBL>()
                .Property(e => e.REF_RELATION)
                .IsUnicode(false);

            modelBuilder.Entity<REFERENCE_TBL>()
                .HasMany(e => e.CLIENT_TBL)
                .WithOptional(e => e.REFERENCE_TBL)
                .HasForeignKey(e => e.CLI_REFERENCE_ID1);

            modelBuilder.Entity<REFERENCE_TBL>()
                .HasMany(e => e.CLIENT_TBL1)
                .WithOptional(e => e.REFERENCE_TBL1)
                .HasForeignKey(e => e.CLI_REFERENCE_ID2);

            modelBuilder.Entity<SCHEDULED_PAYMENT_TBL>()
                .Property(e => e.SCHPAY_ID)
                .HasPrecision(20, 0);

            modelBuilder.Entity<SCHEDULED_PAYMENT_TBL>()
                .Property(e => e.ACC_ID)
                .IsUnicode(false);

            modelBuilder.Entity<SCHEDULED_PAYMENT_TBL>()
                .Property(e => e.SCHPAY_AMOUNT)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ACCOUNT_CLIENT_SCHEDULED_PAYMENTS_VIEW>()
                .Property(e => e.ACC_ID)
                .IsUnicode(false);

            modelBuilder.Entity<ACCOUNT_CLIENT_SCHEDULED_PAYMENTS_VIEW>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<ACCOUNT_CLIENT_SCHEDULED_PAYMENTS_VIEW>()
                .Property(e => e.ITEM_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<ACCOUNT_CLIENT_SCHEDULED_PAYMENTS_VIEW>()
                .Property(e => e.PAYMD_ID)
                .IsUnicode(false);

            modelBuilder.Entity<ACCOUNT_CLIENT_SCHEDULED_PAYMENTS_VIEW>()
                .Property(e => e.ACC_DISTANCE)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ACCOUNT_CLIENT_SCHEDULED_PAYMENTS_VIEW>()
                .Property(e => e.ACC_PRICE)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ACCOUNT_CLIENT_SCHEDULED_PAYMENTS_VIEW>()
                .Property(e => e.ACC_HITCH)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ACCOUNT_CLIENT_SCHEDULED_PAYMENTS_VIEW>()
                .Property(e => e.ACC_INITIAL_BALANCE)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ACCOUNT_CLIENT_SCHEDULED_PAYMENTS_VIEW>()
                .Property(e => e.CALCULATED_CURRENT_BALANCE)
                .HasPrecision(38, 2);

            modelBuilder.Entity<ACCOUNT_CLIENT_SCHEDULED_PAYMENTS_VIEW>()
                .Property(e => e.CALCULATED_GAIN)
                .HasPrecision(11, 2);

            modelBuilder.Entity<ACCOUNT_CLIENT_SCHEDULED_PAYMENTS_VIEW>()
                .Property(e => e.ITEM_BUY_PRICE)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ACCOUNT_CLIENT_SCHEDULED_PAYMENTS_VIEW>()
                .Property(e => e.PAYMENTS)
                .HasPrecision(38, 2);

            modelBuilder.Entity<ACCOUNT_CLIENT_SCHEDULED_PAYMENTS_VIEW>()
                .Property(e => e.SUM_SCHEDULED_PAYMENTS)
                .HasPrecision(38, 2);

            modelBuilder.Entity<ACCOUNT_CLIENT_SCHEDULED_PAYMENTS_VIEW>()
                .Property(e => e.DIFF_SCHEDULED)
                .HasPrecision(38, 2);

            modelBuilder.Entity<PAYMENTS_VIEW>()
                .Property(e => e.ACC_ID)
                .IsUnicode(false);

            modelBuilder.Entity<PAYMENTS_VIEW>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<PAYMENTS_VIEW>()
                .Property(e => e.ITEM_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<PAYMENTS_VIEW>()
                .Property(e => e.PAY_AMOUNT)
                .HasPrecision(10, 2);

            modelBuilder.Entity<PAYMENTS_VIEW>()
                .Property(e => e.PAY_COLLECTOR)
                .IsUnicode(false);

            modelBuilder.Entity<PAYMENTS_VIEW>()
                .Property(e => e.PAY_INSPECTOR)
                .IsUnicode(false);

            modelBuilder.Entity<TOTAL_INVESTED_VS_RECOVERED_VS_SCHEDULED_VIEW>()
                .Property(e => e.DESCRIPTION)
                .IsUnicode(false);

            modelBuilder.Entity<TOTAL_INVESTED_VS_RECOVERED_VS_SCHEDULED_VIEW>()
                .Property(e => e.VALUE)
                .HasPrecision(38, 2);
        }
    }
}
