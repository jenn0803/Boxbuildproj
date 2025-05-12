using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BoxBuildproj.Models;
using Microsoft.AspNetCore.Identity;

public class Reviews
{
    [Key]
    public int ReviewId { get; set; }

    [ForeignKey("AspNetUsers")]
    public string Id { get; set; }

    [ForeignKey("Product")]
    public int ProductId { get; set; }

    public int Rating { get; set; }  // Changed from TEXT to INT
    public string ReviewText { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual IdentityUser User { get; set; }
  //  public virtual Product Product { get; set; }
}
