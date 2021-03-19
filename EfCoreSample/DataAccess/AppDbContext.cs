using EfCoreSample.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EfCoreSample.DataAccess
{
	public class AppDbContext : DbContext
	{
		public DbSet<Part> Parts { get; set; }
		public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
		public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocaldb;Database=EfCoreSample;Trusted_Connection=True;");
		}
	}
}
