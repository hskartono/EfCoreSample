using EfCoreSample.DataAccess;
using EfCoreSample.Entity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EfCoreSample
{
	class Program
	{
		static void Main(string[] args)
		{
			using(var context = new AppDbContext())
			{
				#region Add Data Part
				var p1 = new Part()
				{
					PartNumber = "P1",
					PartName = "PART 1"
				};
				var p2 = new Part()
				{
					PartNumber = "P2",
					PartName = "PART 2"
				};
				var p3 = new Part()
				{
					PartNumber = "P3",
					PartName = "PART 3"
				};
				if (context.Parts.Where(e=>e.PartNumber == p1.PartNumber).Any())
				{
					p1 = context.Parts.Where(e => e.PartNumber == p1.PartNumber).FirstOrDefault();
					p2 = context.Parts.Where(e => e.PartNumber == p2.PartNumber).FirstOrDefault();
					p3 = context.Parts.Where(e => e.PartNumber == p3.PartNumber).FirstOrDefault();
				} else
				{
					context.Parts.Add(p1);
					context.Parts.Add(p2);
					context.Parts.Add(p3);
					context.SaveChanges();
				}
				#endregion

				#region Add Data PO
				var po1 = new PurchaseOrder()
				{
					PoDate = DateTime.Now.Date,
					PoNumber = "PO1",
					Remarks = "Remarks1",
				};
				po1.PurchaseOrderDetails.Add(new PurchaseOrderDetail()
				{
					PurchaseOrder = po1,
					Price = 1000,
					Qty = 5,
					TotalPrice = 5000,
					ItemPart = p1
				});
				po1.PurchaseOrderDetails.Add(new PurchaseOrderDetail()
				{
					PurchaseOrder = po1,
					Price = 2000,
					Qty = 10,
					TotalPrice = 20000,
					ItemPart = p2
				});

				var po2 = new PurchaseOrder()
				{
					PoDate = DateTime.Now.Date,
					PoNumber = "PO2",
					Remarks = "Remarks2",
				};
				po1.PurchaseOrderDetails.Add(new PurchaseOrderDetail()
				{
					PurchaseOrder = po2,
					Price = 3000,
					Qty = 5,
					TotalPrice = 15000,
					ItemPart = p3
				});
				po1.PurchaseOrderDetails.Add(new PurchaseOrderDetail()
				{
					PurchaseOrder = po2,
					Price = 4000,
					Qty = 10,
					TotalPrice = 40000,
					ItemPart = p2
				});

				context.PurchaseOrders.Add(po1);
				context.PurchaseOrders.Add(po2);
				context.SaveChanges();
				#endregion

				
				List<PurchaseOrder> datas = null;

				// mencari po berdasarkan po_number (Equal)
				datas = context.PurchaseOrders.Where(e => e.PoNumber == "PO1").ToList();
				string json = JsonConvert.SerializeObject(datas, Formatting.Indented);
				Console.WriteLine("// mencari po berdasarkan po_number (Equal)");
				Console.WriteLine(json);
				Console.WriteLine("----------------------------------------");

				// mencari po berdasarkan po_number (Like)
				datas = context.PurchaseOrders.Where(e => e.PoNumber.Contains("PO")).ToList();
				json = JsonConvert.SerializeObject(datas, Formatting.Indented);
				Console.WriteLine("// mencari po berdasarkan po_number (Like)");
				Console.WriteLine(json);
				Console.WriteLine("----------------------------------------");

				// mencari po berdasarkan list of po number (equal)
				List<string> poNumbers = new List<string>();
				poNumbers.Add("PO1");
				poNumbers.Add("PO2");

				datas = context.PurchaseOrders.Where(e => poNumbers.Contains(e.PoNumber)).ToList();
				json = JsonConvert.SerializeObject(datas, Formatting.Indented);
				Console.WriteLine("// mencari po berdasarkan list of po number (equal)");
				Console.WriteLine(json);
				Console.WriteLine("----------------------------------------");

				// mencari po berdasarkan list of po number (like)
				poNumbers = new List<string>();
				poNumbers.Add("O1");
				poNumbers.Add("P");
				poNumbers.Add("2");

				var predicate = PredicateBuilder.False<PurchaseOrder>();
				foreach(var item in poNumbers)
				{
					predicate = predicate.Or(p => p.PoNumber.Contains(item));
				}
				datas = context.PurchaseOrders.Where(predicate).ToList();
				json = JsonConvert.SerializeObject(datas, Formatting.Indented);
				Console.WriteLine("// mencari po berdasarkan list of po number (like)");
				Console.WriteLine(json);
				Console.WriteLine("----------------------------------------");

				// mencari po berdasarkan partnumber (equal)
				datas = context.PurchaseOrders.Where(e => e.PurchaseOrderDetails.Any(
					d => d.ItemPart.PartNumber == "P1")
				).ToList();
				json = JsonConvert.SerializeObject(datas, Formatting.Indented);
				Console.WriteLine("// mencari po berdasarkan partnumber (equal)");
				Console.WriteLine(json);
				Console.WriteLine("----------------------------------------");

				// mencari po berdasarkan partnumber (like)
				datas = context.PurchaseOrders.Where(e => e.PurchaseOrderDetails.Any(
					d => d.ItemPart.PartNumber.Contains("P1"))
				).ToList();
				json = JsonConvert.SerializeObject(datas, Formatting.Indented);
				Console.WriteLine("// mencari po berdasarkan partnumber (like)");
				Console.WriteLine(json);
				Console.WriteLine("----------------------------------------");

				// mencari po berdasarkan list of partnumber (equal)
				List<string> partNumbers = new List<string>();
				partNumbers.Add("P1");
				partNumbers.Add("P2");

				datas = context.PurchaseOrders.Where(e => e.PurchaseOrderDetails.Any(
					d => partNumbers.Contains(d.ItemPart.PartNumber))
				).ToList();
				json = JsonConvert.SerializeObject(datas, Formatting.Indented);
				Console.WriteLine("// mencari po berdasarkan list of partnumber (equal)");
				Console.WriteLine(json);
				Console.WriteLine("----------------------------------------");

				// mencari po berdasarkan list of partnumber (like)
				partNumbers = new List<string>();
				partNumbers.Add("P");
				partNumbers.Add("2");

				var predicatePart = PredicateBuilder.False<PurchaseOrderDetail>();
				foreach (var item in partNumbers)
				{
					predicatePart = predicatePart.Or(p => p.ItemPart.PartNumber.Contains(item));
				}

				var query = context.PurchaseOrders.Where(e => e.PurchaseOrderDetails.Any(
					d=>d.ItemPart.PartNumber.Contains(partNumbers[0]) || d.ItemPart.PartNumber.Contains(partNumbers[1])
					// predicatePart.Compile()
					)
				);
				datas = query.ToList();
				json = JsonConvert.SerializeObject(datas, Formatting.Indented);
				Console.WriteLine("// mencari po berdasarkan list of partnumber (like)");
				Console.WriteLine(query.ToQueryString());
				Console.WriteLine(json);
				Console.WriteLine("----------------------------------------");
				Console.WriteLine("Press any key to exit");
				Console.ReadKey();
			}
		}
	}
}
