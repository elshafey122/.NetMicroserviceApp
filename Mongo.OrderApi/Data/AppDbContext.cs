﻿using Microsoft.EntityFrameworkCore;
using Mongo.OrderApi.Model;
using System.Collections.Generic;

namespace Mongo.OrderApi.Data
{
	public class AppDbContext:DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}

		public DbSet<OrderHeader> OrderHeaders { get; set; }
		public DbSet<OrderDetails> OrderDetails { get; set; }
	}
}
