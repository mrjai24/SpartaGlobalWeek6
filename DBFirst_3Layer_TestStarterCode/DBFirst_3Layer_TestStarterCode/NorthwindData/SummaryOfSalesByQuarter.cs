﻿using System;

#nullable disable

namespace NorthwindData;

public partial class SummaryOfSalesByQuarter
{
    public DateTime? ShippedDate { get; set; }
    public int OrderId { get; set; }
    public decimal? Subtotal { get; set; }
}