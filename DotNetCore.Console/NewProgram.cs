using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml;

namespace DotNetCore.ConsoleApp
{
    class NewProgram
    {
        static void Main(string[] args)
        {

            TestLinq();

            Func<int, bool> isOdd = i => (i & 1) == 1;
            Expression<Func<int, bool>> isOddExpression = i => (i & 1) == 1;

            for (var i = 0; i < 10; i++)
            {
                if (isOdd(i))
                {
                    Console.WriteLine(i + " is odd");
                }
                else
                {
                    Console.WriteLine(i + " is even");
                }

            }
            Console.ReadLine();
        }

        private static void LinqToXml()
        {
            Book[] books = new Book[]
            {
                new Book{Title = "Java",Publisher = "manming",Year = 2018},
                new Book{Title = "C#",Publisher = "manming",Year = 2020},
                new Book{Title = "SQL",Publisher = "manming",Year = 2020},
            };
            XmlDocument doc = new XmlDocument();

            XmlElement root = doc.CreateElement("books");
            foreach (var book in books)
            {
                if (book.Year == 2020)
                {
                    XmlElement element = doc.CreateElement("book");
                    element.SetAttribute("title", book.Title);
                    XmlElement publisher = doc.CreateElement("publisher");
                    publisher.InnerText = book.Publisher;
                    element.AppendChild(publisher);
                    root.AppendChild(element);
                }

                doc.AppendChild(root);
                doc.Save(Console.Out);
            }
        }

        public static void Lambda()
        {
            //LINQ查询操作符表示方法
            var processes = Process.GetProcesses()//获取当前正在运行的进程列表
                .Where(process => process.WorkingSet64 > 20 * 1024 * 1024)
                .OrderByDescending(process => process.WorkingSet64)
                .Select(process => new { process.Id, Name = process.ProcessName });//保留ID和名称信息

            //LINQ查询表达式表示方法
            var processes2 = from process in Process.GetProcesses()
                             where process.WorkingSet64 > 20 * 1024 * 1024
                             orderby process.WorkingSet64 descending
                             select new { process.Id, Name = process.ProcessName };

        }

        public static void TestLinq()
        {
            var books = new List<Books>()
            {
                new Books
                {
                    Publisher = "出版社1",
                    Year = 2018,
                    Author = new List<Authors>
                    {
                        new Authors {Age = 15, LastName = "james"},
                        new Authors {Age = 18, LastName = "rondo"}
                    }
                },
                new Books
                {
                    Publisher = "出版社2",
                    Year = 2018,
                    Author = new List<Authors>
                    {
                        new Authors {Age = 15, LastName = "james2"},
                        new Authors {Age = 18, LastName = "rondo2"}
                    }
                },
                new Books
                {
                    Publisher = "出版社3",
                    Year = 2018,
                    Author = new List<Authors>
                    {
                        new Authors {Age = 15, LastName = "james"},
                        new Authors {Age = 18, LastName = "rondo"}
                    }
                }
            };

            var x = books.SelectMany(book => book.Author).ToList();
            var y = books.Select(t => t.Author).ToList();
            var z = from book in books 
                group book by book.Publisher into publisherBooks
                select new {Publisher = publisherBooks.Key,boos=publisherBooks};

            ArrayList arrayList = new ArrayList();
            //Cast操作符，让ArrayList与LINQ集成起来
            var a = arrayList.Cast<Books>().Where(t => t.Publisher == "");
            

        }
    }

    public class Books
    {
        public string Title { get; set; }
        public string Publisher { get; set; }
        public int Year { get; set; }

        public List<Authors> Author { get; set; }
    }

    public class Authors
    {
        public string LastName { get; set; }

        public int Age { get; set; }
    }

}
