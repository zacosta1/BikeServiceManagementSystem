using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

#region AdditionalNamespaces
using BSMSData.Entities;
#endregion

namespace BSMSSystem.DAL
{
    public partial class BSMSContext : DbContext
    {
        public BSMSContext()
            : base("name=BSMSDB")
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Coupon> Coupons { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<JobDetailPart> JobDetailParts { get; set; }
        public virtual DbSet<JobDetail> JobDetails { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<OnlineCustomer> OnlineCustomers { get; set; }
        public virtual DbSet<Part> Parts { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public virtual DbSet<ReceiveOrderDetail> ReceiveOrderDetails { get; set; }
        public virtual DbSet<ReceiveOrder> ReceiveOrders { get; set; }
        public virtual DbSet<ReturnedOrderDetail> ReturnedOrderDetails { get; set; }
        public virtual DbSet<SaleDetail> SaleDetails { get; set; }
        public virtual DbSet<SaleRefundDetail> SaleRefundDetails { get; set; }
        public virtual DbSet<SaleRefund> SaleRefunds { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public virtual DbSet<StandardJobPart> StandardJobParts { get; set; }
        public virtual DbSet<StandardJob> StandardJobs { get; set; }
        public virtual DbSet<UnorderedPurchaseItemCart> UnorderedPurchaseItemCarts { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.Parts)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.Province)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.PostalCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.ContactPhone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.EmailAddress)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Jobs)
                .WithRequired(e => e.Customer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.SocialInsuranceNumber)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Province)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.PostalCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.HomePhone)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.EmailAddress)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Jobs)
                .WithRequired(e => e.Employee)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.PurchaseOrders)
                .WithRequired(e => e.Employee)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.SaleRefunds)
                .WithRequired(e => e.Employee)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Sales)
                .WithRequired(e => e.Employee)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<JobDetailPart>()
                .Property(e => e.SellingPrice)
                .HasPrecision(10, 4);

            modelBuilder.Entity<JobDetail>()
                .Property(e => e.JobHours)
                .HasPrecision(5, 2);

            modelBuilder.Entity<JobDetail>()
                .HasMany(e => e.JobDetailParts)
                .WithRequired(e => e.JobDetail)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Job>()
                .Property(e => e.ShopRate)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Job>()
                .Property(e => e.StatusCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Job>()
                .HasMany(e => e.JobDetails)
                .WithRequired(e => e.Job)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OnlineCustomer>()
                .HasMany(e => e.ShoppingCarts)
                .WithRequired(e => e.OnlineCustomer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Part>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Part>()
                .Property(e => e.PurchasePrice)
                .HasPrecision(10, 4);

            modelBuilder.Entity<Part>()
                .Property(e => e.SellingPrice)
                .HasPrecision(10, 4);

            modelBuilder.Entity<Part>()
                .Property(e => e.Refundable)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Part>()
                .HasMany(e => e.JobDetailParts)
                .WithRequired(e => e.Part)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Part>()
                .HasMany(e => e.PurchaseOrderDetails)
                .WithRequired(e => e.Part)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Part>()
                .HasMany(e => e.SaleDetails)
                .WithRequired(e => e.Part)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Part>()
                .HasMany(e => e.SaleRefundDetails)
                .WithRequired(e => e.Part)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Part>()
                .HasMany(e => e.ShoppingCartItems)
                .WithRequired(e => e.Part)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Part>()
                .HasMany(e => e.StandardJobParts)
                .WithRequired(e => e.Part)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Position>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Position>()
                .HasMany(e => e.Employees)
                .WithRequired(e => e.Position)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PurchaseOrderDetail>()
                .Property(e => e.PurchasePrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PurchaseOrderDetail>()
                .HasMany(e => e.ReceiveOrderDetails)
                .WithRequired(e => e.PurchaseOrderDetail)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PurchaseOrder>()
                .Property(e => e.TaxAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PurchaseOrder>()
                .Property(e => e.SubTotal)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PurchaseOrder>()
                .HasMany(e => e.PurchaseOrderDetails)
                .WithRequired(e => e.PurchaseOrder)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PurchaseOrder>()
                .HasMany(e => e.ReceiveOrders)
                .WithRequired(e => e.PurchaseOrder)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReceiveOrder>()
                .HasMany(e => e.ReceiveOrderDetails)
                .WithRequired(e => e.ReceiveOrder)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReceiveOrder>()
                .HasMany(e => e.ReturnedOrderDetails)
                .WithRequired(e => e.ReceiveOrder)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SaleDetail>()
                .Property(e => e.SellingPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<SaleRefundDetail>()
                .Property(e => e.SellingPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<SaleRefund>()
                .Property(e => e.TaxAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<SaleRefund>()
                .Property(e => e.SubTotal)
                .HasPrecision(19, 4);

            modelBuilder.Entity<SaleRefund>()
                .HasMany(e => e.SaleRefundDetails)
                .WithRequired(e => e.SaleRefund)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sale>()
                .Property(e => e.TaxAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Sale>()
                .Property(e => e.SubTotal)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Sale>()
                .Property(e => e.PaymentType)
                .IsFixedLength();

            modelBuilder.Entity<Sale>()
                .HasMany(e => e.SaleDetails)
                .WithRequired(e => e.Sale)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sale>()
                .HasMany(e => e.SaleRefunds)
                .WithRequired(e => e.Sale)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ShoppingCart>()
                .HasMany(e => e.ShoppingCartItems)
                .WithRequired(e => e.ShoppingCart)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<StandardJob>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<StandardJob>()
                .Property(e => e.StandardHours)
                .HasPrecision(5, 2);

            modelBuilder.Entity<StandardJob>()
                .HasMany(e => e.StandardJobParts)
                .WithRequired(e => e.StandardJob)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Vendor>()
                .Property(e => e.ProvinceID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Vendor>()
                .Property(e => e.PostalCode)
                .IsFixedLength();

            modelBuilder.Entity<Vendor>()
                .HasMany(e => e.Parts)
                .WithRequired(e => e.Vendor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Vendor>()
                .HasMany(e => e.PurchaseOrders)
                .WithRequired(e => e.Vendor)
                .WillCascadeOnDelete(false);
        }
    }
}
