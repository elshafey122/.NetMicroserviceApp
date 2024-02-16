using Microsoft.EntityFrameworkCore;
using Mongo.EmailApi.Model;

namespace Mongo.EmailApi.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) 
        {
                
        }
        public DbSet<EmailLogger> EmailLoggers { get; set; }
    }
}