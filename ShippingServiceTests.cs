using DeliveryCalculator.Enums;
using DeliveryCalculator.Models;
using DeliveryCalculator.Services;

namespace DeliveryCalculator.Tests;

public class ShippingServiceTests
{
    // 🔹 БАЗОВЫЕ СЦЕНАРИИ

    [Fact]
    public void Standard_NoExtras_ShouldBe300()
    {
        var service = new ShippingService();

        var result = service.Calculate(new ShippingRequest
        {
            DeliveryType = DeliveryType.Standard,
            Weight = 0.8m,
            Distance = 8
        });

        Assert.Equal(300, result.Total);
    }

    [Fact]
    public void Express_WithUrgent_ShouldIncreasePrice()
    {
        var service = new ShippingService();

        var result = service.Calculate(new ShippingRequest
        {
            DeliveryType = DeliveryType.Express,
            Weight = 2,
            Distance = 20,
            IsUrgent = true
        });

        Assert.True(result.UrgentFee > 0);
    }

    [Fact]
    public void Cargo_WithInsurance_ShouldIncreasePrice()
    {
        var service = new ShippingService();

        var result = service.Calculate(new ShippingRequest
        {
            DeliveryType = DeliveryType.Cargo,
            Weight = 10,
            Distance = 100,
            HasInsurance = true
        });

        Assert.True(result.InsuranceFee > 0);
    }

    // 🔹 ВЕС

    [Fact]
    public void Weight_LessThan1_ShouldHaveZeroFee()
    {
        var service = new ShippingService();

        var result = service.Calculate(new ShippingRequest
        {
            DeliveryType = DeliveryType.Standard,
            Weight = 0.5m,
            Distance = 10
        });

        Assert.Equal(0, result.WeightFee);
    }

    [Fact]
    public void Weight_Between1And5_ShouldBe100()
    {
        var service = new ShippingService();

        var result = service.Calculate(new ShippingRequest
        {
            DeliveryType = DeliveryType.Standard,
            Weight = 3,
            Distance = 10
        });

        Assert.Equal(100, result.WeightFee);
    }

    // 🔹 РАССТОЯНИЕ

    [Fact]
    public void Distance_LessThan10_ShouldBe100()
    {
        var service = new ShippingService();

        var result = service.Calculate(new ShippingRequest
        {
            DeliveryType = DeliveryType.Standard,
            Weight = 1,
            Distance = 5
        });

        Assert.Equal(100, result.DistanceFee);
    }

    [Fact]
    public void Distance_MoreThan200_ShouldBe1500()
    {
        var service = new ShippingService();

        var result = service.Calculate(new ShippingRequest
        {
            DeliveryType = DeliveryType.Standard,
            Weight = 1,
            Distance = 300
        });

        Assert.Equal(1500, result.DistanceFee);
    }

    // 🔹 ПРОМОКОДЫ

    [Fact]
    public void Promo_SALE5_ShouldApplyDiscount()
    {
        var service = new ShippingService();

        var result = service.Calculate(new ShippingRequest
        {
            DeliveryType = DeliveryType.Standard,
            Weight = 2,
            Distance = 20,
            PromoCode = "SALE5"
        });

        Assert.True(result.Discount > 0);
    }

    [Fact]
    public void Promo_SAVE300_ShouldApplyDiscount()
    {
        var service = new ShippingService();

        var result = service.Calculate(new ShippingRequest
        {
            DeliveryType = DeliveryType.Standard,
            Weight = 2,
            Distance = 20,
            PromoCode = "SAVE300"
        });

        Assert.Equal(300, result.Discount);
    }

    [Fact]
    public void Discount_ShouldNotExceedTotal()
    {
        var service = new ShippingService();

        var result = service.Calculate(new ShippingRequest
        {
            DeliveryType = DeliveryType.Standard,
            Weight = 1,
            Distance = 5,
            PromoCode = "SAVE300"
        });

        Assert.True(result.Total >= 0);
    }

    // 🔹 ВАЛИДАЦИЯ (ОШИБКИ)

    [Fact]
    public void Weight_Zero_ShouldThrow()
    {
        var service = new ShippingService();

        Assert.Throws<ArgumentException>(() =>
            service.Calculate(new ShippingRequest
            {
                DeliveryType = DeliveryType.Standard,
                Weight = 0,
                Distance = 10
            }));
    }

    [Fact]
    public void Distance_Zero_ShouldThrow()
    {
        var service = new ShippingService();

        Assert.Throws<ArgumentException>(() =>
            service.Calculate(new ShippingRequest
            {
                DeliveryType = DeliveryType.Standard,
                Weight = 1,
                Distance = 0
            }));
    }

    [Fact]
    public void Invalid_Promo_ShouldThrow()
    {
        var service = new ShippingService();

        Assert.Throws<ArgumentException>(() =>
            service.Calculate(new ShippingRequest
            {
                DeliveryType = DeliveryType.Standard,
                Weight = 1,
                Distance = 10,
                PromoCode = "BADCODE"
            }));
    }
}