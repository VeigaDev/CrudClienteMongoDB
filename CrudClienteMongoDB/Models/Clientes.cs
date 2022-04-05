using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CrudClienteMongoDB.Models
{
    public class Clientes
    {
        [BsonElement("_id")]
        public Guid CustomerId { get; set; }
        public string FantasyName { get; set; }
        public string CompanyName { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime LastUpdate { get; set; }
        public string CPF_CNPJ { get; set; }
        public string PhoneNumber { get; set; }
        public bool Customer { get; set; }
        public bool Supplier { get; set; }
    }
}
