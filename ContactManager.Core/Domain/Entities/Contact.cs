using System.ComponentModel.DataAnnotations.Schema;

namespace ContactManager.Core.Domain.Entities {
    public class Contact {
        private const int DAYS_PER_YEAR = 365;

        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public int Age => (DateTime.Today - DateOfBirth).Days / DAYS_PER_YEAR;

        // Navigation Properties
        [ForeignKey(nameof(OwnerId))]
        public virtual User Owner { get; set; }
        public List<Address> Addresses { get; } = new();
    }
}
