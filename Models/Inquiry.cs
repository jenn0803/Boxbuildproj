using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BoxBuildproj.Models;
using Microsoft.AspNetCore.Identity;

public class Inquiry
{
    [Key]
    public int InquiryId { get; set; }

    [ForeignKey("AspNetUsers")]
    public string Id { get; set; }

    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }

    public virtual IdentityUser User { get; set; }
}