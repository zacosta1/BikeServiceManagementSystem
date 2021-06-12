namespace BSMSData.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Job
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Job()
        {
            JobDetails = new HashSet<JobDetail>();
            Sales = new HashSet<Sale>();
        }

        [Required]
        public int JobID { get; set; }

        public DateTime JobDateIn { get; set; }

        public DateTime? JobDateStarted { get; set; }

        public DateTime? JobDateDone { get; set; }

        public DateTime? JobDateOut { get; set; }

        [Required]
        public int CustomerID { get; set; }

        public int EmployeeID { get; set; }

        public decimal ShopRate { get; set; }

        [Required]
        [StringLength(1)]
        public string StatusCode { get; set; }

        [Required]
        [StringLength(50,ErrorMessage = "The provided vehicle identitfication number or model description exceeded the 50-character limit.")]
        public string VehicleIdentification { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Employee Employee { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobDetail> JobDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sales { get; set; }
    }
}
