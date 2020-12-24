using System.Collections.Generic;

namespace SelfReferencingSample.Entities
{
    public class Customer
    {
        public long Id { get; set; }

        public string FullName { get; set; }

        public long ParentId { get; set; }

        public Customer Parent { get; set; }

        public ICollection<Customer> Children { get; set; }
    }
}
