using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BoxBuildproj.Models;
using Microsoft.AspNetCore.Identity;

public class Offers
{
    [Key]
    public int OfferId { get; set; }

    public string OfferTitle { get; set; }
    public string OfferDescription { get; set; }
    public decimal DiscountPercentage { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
}