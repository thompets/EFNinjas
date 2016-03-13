﻿using NinjaDomain.Classes;
using NinjaDomain.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ConsoleApplication
{
	class Program
	{
		static void Main(string[] args)
		{
			// Keeps EF from going through its db initialization process
			Database.SetInitializer(new NullDatabaseInitializer<NinjaContext>());

			// InsertNinja();
			// InsertMultipleNinjas();
			// QueryAndUpdateNinja();
			// QueryAndUpdateNinjaDisconnected();
			// RetrieveDataWithFind();
			RetrieveDataWithStoredProc();
			// SimpleNinjaQueries();
		}		

		private static void InsertMultipleNinjas()
		{
			var ninja1 = new Ninja
			{
				Name = "Leonardo",
				ServedInOniwaban = false,
				DateOfBirth = new DateTime(1984, 1, 1),
				ClanId = 1
			};

			var ninja2 = new Ninja
			{
				Name = "Raphael",
				ServedInOniwaban = false,
				DateOfBirth = new DateTime(1985, 1, 1),
				ClanId = 1
			};

			using (var context = new NinjaContext())
			{
				context.Database.Log = Console.WriteLine;
				context.Ninjas.AddRange(new List<Ninja> { ninja1, ninja2 });
				context.SaveChanges();
			}
		}

		private static void InsertNinja()
		{
			var ninja = new Ninja
			{
				Name = "AttieSan",
				ServedInOniwaban = false,
				DateOfBirth = new DateTime(2014, 6, 1),
				ClanId = 1
			};

			using (var context = new NinjaContext())
			{
				context.Database.Log = Console.WriteLine;
				context.Ninjas.Add(ninja);
				context.SaveChanges();
			}
		}

		private static void QueryAndUpdateNinja()
		{
			using (var context = new NinjaContext())
			{
				context.Database.Log = Console.WriteLine;

				var ninja = context.Ninjas.FirstOrDefault();
				ninja.ServedInOniwaban = (!ninja.ServedInOniwaban);

				context.SaveChanges();
			}
		}

		private static void QueryAndUpdateNinjaDisconnected()
		{
			Ninja ninja;
			using (var context = new NinjaContext())
			{
				context.Database.Log = Console.WriteLine;
				ninja = context.Ninjas.FirstOrDefault();
			}

			ninja.ServedInOniwaban = (!ninja.ServedInOniwaban);

			using (var context = new NinjaContext())
			{
				context.Database.Log = Console.WriteLine;

				// this will send a different update to db.  
				// updates all fields instead only the one that changed.
				context.Entry(ninja).State = EntityState.Modified;
				context.SaveChanges();
			}
		}

		private static void RetrieveDataWithFind()
		{
			var keyval = 4;
			using (var context = new NinjaContext())
			{
				context.Database.Log = Console.WriteLine;
				var ninja = context.Ninjas.Find(keyval);
				Console.WriteLine("After Find#1: " + ninja.Name);

				var someNinja = context.Ninjas.Find(keyval);
				Console.WriteLine("After Find#2: " + ninja.Name);
				ninja = null;
			}
		}

		private static void RetrieveDataWithStoredProc()
		{
			using (var context = new NinjaContext())
			{
				context.Database.Log = Console.WriteLine;
				var ninjas = context.Ninjas.SqlQuery("exec GetOldNinjas").ToList();
				//foreach (var ninja in ninjas)
				//{
				//	Console.WriteLine(ninja.Name);
				//}
			}
		}

		private static void SimpleNinjaQueries()
		{
			using (var context = new NinjaContext())
			{
				var ninjas = context.Ninjas.ToList();
			}
		}
	}
}
