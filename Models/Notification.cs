using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BoxBuildproj.Models;
using Microsoft.AspNetCore.Identity;
public class Notification
{
    [Key]
    public int NotificationId { get; set; }

    [ForeignKey("AspNetUsers")]
    public string Id { get; set; }

    [ForeignKey("Orders")]
    public int OrderId { get; set; }

    public string Message { get; set; }
    public bool IsRead { get; set; }
    public string Type { get; set; }

    public virtual IdentityUser User { get; set; }
    public virtual Orders Order { get; set; }
}