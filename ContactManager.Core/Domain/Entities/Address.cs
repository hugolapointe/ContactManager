using System.ComponentModel.DataAnnotations.Schema;

namespace ContactManager.Core.Domain.Entities {
    public class Address {

        public Guid Id { get; set; }
        public Guid ContactId { get; set; }

        public int StreetNumber { get; set; }
        public string StreetName { get; set; }
        public string CityName { get; set; }
        public string PostalCode { get; set; }


        // Navigation Properties
        [ForeignKey(nameof(ContactId))]
        public virtual Contact? Contact { get; set; }
    }
}