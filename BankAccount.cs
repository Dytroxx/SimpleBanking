using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

class BankAccount
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string AccountNumber { get; set; }
    public decimal Balance { get; set; }
    public Customer Owner { get; set; }
}
