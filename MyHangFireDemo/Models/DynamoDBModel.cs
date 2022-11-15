using Amazon.DynamoDBv2.DataModel;

namespace MyHangFireDemo.Models
{
    [DynamoDBTable("Table2")]
    public class DynamoDBModel
    {
        [DynamoDBHashKey("id")]
        public int Id { get; set; }


        [DynamoDBProperty("name")]
        public string Name { get; set; }


        [DynamoDBProperty("price")]
        public decimal Price { get; set; }


        [DynamoDBProperty("category")]
        public string Category { get; set; }


        [DynamoDBProperty("author")]
        public string Author { get; set; }


        [DynamoDBProperty("new_status")]
        public int NewStatus { get; set; }


        [DynamoDBProperty("book_origin")]
        public string BookOrigin { get; set; }

    }
}
