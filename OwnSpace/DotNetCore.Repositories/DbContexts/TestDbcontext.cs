using DotNetCore.Models;
using DotNetCore.Models.Team;
using Microsoft.EntityFrameworkCore;

namespace DotNetCore.Repositories.DbContexts
{
    public class TestDbcontext : DbContext
    {
        public TestDbcontext(DbContextOptions<TestDbcontext> options) : base(options)
        {
        }

        //public DbSet<User> Users { get; set; }

	    public DbSet<Transaction> Transactions { get; set; }

        //会员基础信息
        public DbSet<BasicInfomation> BasicInfomation { get; set; }

        /// <summary>
        /// 球队
        /// </summary>
        public DbSet<Team> Team { get; set; }
    }
}
