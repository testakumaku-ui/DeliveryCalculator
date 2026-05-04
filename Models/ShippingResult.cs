using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCalculator.Models;

public class ShippingResult
{
    public decimal Total { get; set; }
    public decimal Discount { get; set; }
    public decimal BasePrice { get; set; }
    public decimal WeightFee { get; set; }
    public decimal DistanceFee { get; set; }
    public decimal UrgentFee { get; set; }
    public decimal InsuranceFee { get; set; }
}
