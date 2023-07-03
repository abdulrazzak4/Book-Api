using book.Data.Models;

namespace book.Data.Services
{
    public class LogsService
    {
        public AppDbContext _context { get; set; }
        public LogsService(AppDbContext context) 
        {
            _context = context;
        }
        public List<Log> GetAllLogsFromDB() => _context.Logs.ToList();
    }
}
