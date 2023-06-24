using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

class Customer
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string CustomerName { get; set; }
}
