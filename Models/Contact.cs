using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models{
    public class Contact{
        [Key]
        public int ID { get; set; }
        
        [Column(TypeName ="nvarchar")]
        [StringLength(50)]
        [Required(ErrorMessage ="Full name cannot null")]
        public string FullName { get; set; }
        [Required]
        [StringLength(100)]
        [EmailAddress(ErrorMessage ="Required a email")]
        public string Email { get; set; }
        public DateTime DateSent{set;get;}
        public string Message { get; set; }
        [StringLength(50)]
        [Phone(ErrorMessage ="Phone number invalid")]
        public string Phone { get; set; }
        public Contact(){}
    }
}