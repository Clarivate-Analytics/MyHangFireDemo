using Amazon.DynamoDBv2.DataModel;
using MyHangFireDemo.Models;
using MyHangFireDemo.Utilitities;
using MyHangFireDemo.Utility;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyHangFireDemo.Services
{
    public class JobService : IJobService
    {
        private readonly IXMLUtility _XMLUtility;
        private readonly ISQLBookUtility _SQLBookUtility;
        private readonly IDynamoBookUtility _DynamoBookUtility;
        private readonly SQLBooksModel _bookModel = new SQLBooksModel();

        public JobService(ISQLBookUtility SQLBookUtility, IDynamoBookUtility DynamoBookUtility, IXMLUtility XMLUtility)
        {
            _XMLUtility = XMLUtility;
            _SQLBookUtility = SQLBookUtility;
            _DynamoBookUtility = DynamoBookUtility;
        }

        // Hangfire Job Implementation - Started
        public async Task CreateGetNewBookJob()
        {
            // List all the new Book available on SQL Server
            var newBook = await _SQLBookUtility.GetNewBookAsync();

            foreach (var item in newBook)
            {
                //Insert into DynamoDB each item from SQL having NewStatus=1
                await _DynamoBookUtility.InsertDataDynamo(item);

                // Update the Book as NewStatus = 0 in SQL Server, as this Book got processed
                await _SQLBookUtility.UpdateNewBookAsync(item.Id, _bookModel);

            }
        }

        public async Task CreateNewBookReportJob()
        {
            var data = await _DynamoBookUtility.GetDataFromDynamoDB();

            if (data.Any())
            {
                _XMLUtility.GenXML(data);
            }

            Console.WriteLine(data);
        }

        public void FireAndForgetJob()
        {
            Console.WriteLine("Hello from a Fire and Forget job!");
        }
        public void ReccuringJob()
        {
            Console.WriteLine("Hello from a Scheduled job!");
        }
        public void DelayedJob()
        {
            Console.WriteLine("Hello from a Delayed job!");
        }
        public void ContinuationJob()
        {
            Console.WriteLine("Hello from a Continuation job!");
        }
        // Hangfire Job Implementation - End
    }
}
