using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DotNetCore.Repositories.DbContexts
{

	public class DbConnectionString
	{
		//建表时，把这里的数据库链接字符串改成要连的数据库
		public static string TestDb = @"Data Source=.\SQLEXPRESS;Initial Catalog=OWNDB;User ID=sa;Password=123456;";
	}

	public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TestDbcontext>
	{
		public TestDbcontext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<TestDbcontext>();
			optionsBuilder.UseSqlServer(DbConnectionString.TestDb);

			return new TestDbcontext(optionsBuilder.Options);
		}
	}
}
