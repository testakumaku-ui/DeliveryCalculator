using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeliveryCalculator.Enums;

namespace DeliveryCalculator.Models;

public class ShippingRequest
{
    public DeliveryType DeliveryType { get; set; }
    public decimal Weight { get; set; }
    public int Distance { get; set; }
    public bool IsUrgent { get; set; }
    public bool HasInsurance { get; set; }
    public string? PromoCode { get; set; }
}
