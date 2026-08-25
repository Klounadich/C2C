using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catalog.Models
{
    [Table("item_aliases")]
    public class ItemAlias
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("canonical_term")]
        public string CanonicalTerm { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Column("alias")]
        public string Alias { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}