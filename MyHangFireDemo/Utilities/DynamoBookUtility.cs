using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using MyHangFireDemo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyHangFireDemo.Utilitities
{
    public interface IDynamoBookUtility
    {
        public Task InsertDataDynamo(SQLBooksModel bookModel);

        public Task<List<DynamoDBModel>> GetDataFromDynamoDB();

    }

    public class DynamoBookUtility : IDynamoBookUtility
    {
        private readonly IDynamoDBContext _context;

        public DynamoBookUtility(IDynamoDBContext context)
        {
            AmazonDynamoDBConfig config = new AmazonDynamoDBConfig();
            config.ServiceURL = "https://dynamodb.ap-south-1.amazonaws.com";
            _context = context;
        }

        public async Task InsertDataDynamo(SQLBooksModel item)
        {
            DynamoDBModel model = new DynamoDBModel();

            model.Id = item.Id;
            model.Name = item.Name;
            model.Price = item.Price;
            model.Category = item.Category;
            model.Author = item.Author;
            model.NewStatus = item.NewStatus;
            model.BookOrigin = item.BookOrigin;

            await _context.SaveAsync(model);
        }

        public async Task<List<DynamoDBModel>> GetDataFromDynamoDB()
        {
            var condition = new List<ScanCondition> {
               new ScanCondition("NewStatus", ScanOperator.Equal, 1)};

            AsyncSearch<DynamoDBModel> search = _context.ScanAsync<DynamoDBModel>(condition);

            var data = await search.GetRemainingAsync();

            for (int i = 0; i < data.Count; i++)
            {
                var item = data[i];
                item.NewStatus = 0;
                await _context.SaveAsync(item);
            }

            return data;
        }

    }
}
