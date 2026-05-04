using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeliveryCalculator.Enums;
using DeliveryCalculator.Models;

namespace DeliveryCalculator.Services;

public class ShippingService
{
    public ShippingResult Calculate(ShippingRequest request)
    {
        Validate(request);

        var result = new ShippingResult();

        result.BasePrice = GetBasePrice(request.DeliveryType);
        result.WeightFee = GetWeightFee(request.Weight);
        result.DistanceFee = GetDistanceFee(request.Distance);
        result.UrgentFee = GetUrgentFee(request);
        result.InsuranceFee = GetInsuranceFee(request);

        var total = result.BasePrice +
                    result.WeightFee +
                    result.DistanceFee +
                    result.UrgentFee +
                    result.InsuranceFee;

        result.Discount = GetDiscount(request.PromoCode, total);

        if (result.Discount > total)
            result.Discount = total;

        result.Total = total - result.Discount;

        return result;
    }

    private void Validate(ShippingRequest request)  
    {
        if (request.Weight <= 0)
            throw new ArgumentException("Weight must be > 0");

        if (request.Distance <= 0)
            throw new ArgumentException("Distance must be > 0");
    }

    private decimal GetBasePrice(DeliveryType type)
    {
        return type switch
        {
            DeliveryType.Standard => 200,
            DeliveryType.Express => 500,
            DeliveryType.Cargo => 1000,
            _ => throw new ArgumentException("Invalid type")
        };
    }

    private decimal GetWeightFee(decimal weight)
    {
        if (weight <= 1) return 0;
        if (weight <= 5) return 100;
        if (weight <= 20) return 300;
        return 800;
    }

    private decimal GetDistanceFee(int distance)
    {
        if (distance <= 10) return 100;
        if (distance <= 50) return 300;
        if (distance <= 200) return 800;
        return 1500;
    }

    private decimal GetUrgentFee(ShippingRequest r)
    {
        if (!r.IsUrgent) return 0;

        return r.DeliveryType switch
        {
            DeliveryType.Standard => 300,
            DeliveryType.Express => 500,
            DeliveryType.Cargo => 1000,
            _ => 0
        };
    }

    private decimal GetInsuranceFee(ShippingRequest r)
    {
        if (!r.HasInsurance) return 0;

        return r.DeliveryType switch
        {
            DeliveryType.Standard => 100,
            DeliveryType.Express => 200,
            DeliveryType.Cargo => 500,
            _ => 0
        };
    }

    private decimal GetDiscount(string? promo, decimal total)
    {
        if (string.IsNullOrEmpty(promo)) return 0;

        return promo switch
        {
            "SALE5" => total * 0.05m,
            "SAVE300" => 300,
            _ => throw new ArgumentException("Invalid promo")
        };
    }
}
