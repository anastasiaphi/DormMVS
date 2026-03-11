using DormDomain.Model;

namespace DormInfrastructure.Services
{
    public class FacultyServiceFactory
        : IDataPortServiceFactory<Faculty>
    {
        private readonly Do2Context _context;
        public FacultyServiceFactory(Do2Context context)
        {
            _context = context;
        }
        public IImportService<Faculty> GetImportService(string contentType)
        {
            if (contentType is "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return new FacultiesImportService(_context);
            }
            throw new NotImplementedException($"No import service implemented for movies with content type {contentType}");
        }
        public IExportService<Faculty> GetExportService(string contentType)
        {
            if (contentType is "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
              //  return new FacultyExportService(_context);
            }
            throw new NotImplementedException($"No export service implemented for movies with content type {contentType}");
        }
    }

}
