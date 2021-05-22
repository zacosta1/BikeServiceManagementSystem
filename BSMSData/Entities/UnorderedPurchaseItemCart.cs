namespace BSMSData.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UnorderedPurchaseItemCart")]
    public partial class UnorderedPurchaseItemCart
    {
        [Key]
        public int CartID { get; set; }

        public int PurchaseOrderNumber { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public string VendorPartNumber { get; set; }

        public int Quantity { get; set; }
    }
}
