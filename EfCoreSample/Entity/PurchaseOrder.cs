using System;
using System.Collections.Generic;
using System.Text;

namespace EfCoreSample.Entity
{
	public class PurchaseOrder
	{
		public int Id { get; set; }
		public string PoNumber { get; set; }
		public DateTime PoDate { get; set; }
		public string Remarks { get; set; }
		public List<PurchaseOrderDetail> PurchaseOrderDetails { get; set; } = new List<PurchaseOrderDetail>();

	}
}
