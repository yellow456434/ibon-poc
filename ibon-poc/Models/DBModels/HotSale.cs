using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ibon_poc.Models.DBModels
{
    public class HotSale
    {
        [Key]
        public int id { set; get; }
        [Column(TypeName="nvarchar(10)"), MaxLength(10), Required]
        public string Name { set; get; }
        public int Price { set; get; }
    }
}
