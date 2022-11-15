using MyHangFireDemo.Helpers;
using MyHangFireDemo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace MyHangFireDemo.Utility
{
    public interface IXMLUtility
    {
        void GenXML(List<DynamoDBModel> newBook);
    }
    public class XMLUtility : IXMLUtility
    {

        //XML Service Implementation - Start
        public void GenXML(List<DynamoDBModel> newBook)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlElement bookDataNode = doc.CreateElement("NewBookData");
            bookDataNode.SetAttribute("xmlns:xsi", "https://www.proquest.com/");
            bookDataNode.SetAttribute("schemaLocation", "https://www.proquest.com/XMLSchema-instance", "https://www.proquest.com/ aaa.xsd");
            bookDataNode.SetAttribute("xmlns", "https://www.proquest.com/");
            doc.AppendChild(bookDataNode);

            XmlNode headerNode = doc.CreateElement("Header");
            bookDataNode.AppendChild(headerNode);

            DateTime today = DateTime.Now;
            string strDateTime = today.ToString("yyyy-MM-dd hh:mm:ss");

            XmlNode contentDateNode = doc.CreateElement("ContentDate");
            contentDateNode.AppendChild(doc.CreateTextNode(strDateTime));
            headerNode.AppendChild(contentDateNode);

            XmlNode bookRecordsNode = doc.CreateElement("BookRecords");
            doc.DocumentElement.AppendChild(bookRecordsNode);

            //Enter Each New Book Details in XML File
            foreach (var book in newBook)
            {
                XmlNode bookRecordNode = doc.CreateElement("BookRecord");
                bookRecordsNode.AppendChild(bookRecordNode);

                XmlNode bookNameNode = doc.CreateElement("BookName");
                bookNameNode.AppendChild(doc.CreateTextNode(book.Name));
                bookRecordNode.AppendChild(bookNameNode);

                string Price = ((double)book.Price).ToString();
                XmlNode bookPriceNode = doc.CreateElement("BookPrice");
                bookPriceNode.AppendChild(doc.CreateTextNode(Price));
                bookRecordNode.AppendChild(bookPriceNode);

                XmlNode bookCategoryNode = doc.CreateElement("BookCategory");
                bookCategoryNode.AppendChild(doc.CreateTextNode(book.Category));
                bookRecordNode.AppendChild(bookCategoryNode);

                XmlNode bookAuthorNode = doc.CreateElement("BookAuthor");
                bookAuthorNode.AppendChild(doc.CreateTextNode(book.Author));
                bookRecordNode.AppendChild(bookAuthorNode);
            }

            var basePath = Path.Combine(Environment.CurrentDirectory, @"XMLFiles\");
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            var newFileName = string.Format("{0}{1}", Guid.NewGuid().ToString("N"), ".xml");
            doc.Save(basePath + newFileName);
        }
        //XML Service Implementation - End
    }
}
