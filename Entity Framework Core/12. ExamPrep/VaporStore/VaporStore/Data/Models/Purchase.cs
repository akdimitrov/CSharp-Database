using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VaporStore.Data.Models.Enums;

namespace VaporStore.Data.Models
{
    public class Purchase
    {
public int Id { get; set; }

        public PurchaseType Type { get; set; }

        [Required]
        [MaxLength(14)]
        public string ProductKey { get; set; }

        public DateTime Date { get; set; }

        public int CardId { get; set; }

        public virtual Card Card { get; set; }

        public int GameId { get; set; }

        public virtual Game Game { get; set; }
    }
}