using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace EfCoreSample.Entity
{
	public class PurchaseOrderDetail
	{
		public int Id { get; set; }
		public Part ItemPart { get; set; }
		public int Qty { get; set; }
		public float Price { get; set; }
		public float TotalPrice { get; set; }
		public int PurchaseOrderId { get; set; }

		[JsonIgnore]
		[IgnoreDataMember]
		public virtual PurchaseOrder PurchaseOrder { get; set; }

	}
}
