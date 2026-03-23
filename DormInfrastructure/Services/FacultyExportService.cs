using ClosedXML.Excel;
using DormDomain.Model;
using Microsoft.EntityFrameworkCore;

namespace DormInfrastructure.Services
{
    public class FacultyExportService : IExportService<Faculty>
    {
        private readonly Do2Context _context;

        private static readonly IReadOnlyList<string> HeaderNames =
            new string[]
            {
                "ПІБ Студента",
                "Кафедра",
                "Телефон",
                "Ступінь"
            };

        public FacultyExportService(Do2Context context)
        {
            _context = context;
        }

        private static void WriteHeader(IXLWorksheet worksheet)
        {
            for (int columnIndex = 0; columnIndex < HeaderNames.Count; columnIndex++)
            {
                worksheet.Cell(1, columnIndex + 1).Value = HeaderNames[columnIndex];
            }
            worksheet.Row(1).Style.Font.Bold = true;
        }

        private void WriteStudent(IXLWorksheet worksheet, Student student, string departmentName, int rowIndex)
        {
            var columnIndex = 1;
           
            worksheet.Cell(rowIndex, columnIndex++).Value = $"{student.LastName} {student.FirstName} {student.MiddleName}".Trim();
           
            worksheet.Cell(rowIndex, columnIndex++).Value = departmentName;
            worksheet.Cell(rowIndex, 3).Value = student.PhoneNumber ?? "Не вказано";

            worksheet.Cell(rowIndex, 4).Value = student.Degree?.DegreeName ?? "Не вказано";
        }

        private void WriteFacultyData(IXLWorksheet worksheet, Faculty faculty)
        {
            WriteHeader(worksheet);
            int rowIndex = 2;

            foreach (var dept in faculty.Departments)
            {
                foreach (var student in dept.Students)
                {
                    WriteStudent(worksheet, student, dept.Name, rowIndex);
                    rowIndex++;
                }
            }
            worksheet.Columns().AdjustToContents();
        }

        public async Task WriteToAsync(Stream stream, CancellationToken cancellationToken)
        {
            if (!stream.CanWrite)
            {
                throw new ArgumentException("Input stream is not writable");
            }

            var faculties = await _context.Faculties
         .Include(f => f.Departments)           
             .ThenInclude(d => d.Students)       
                 .ThenInclude(s => s.Degree)    
         .ToListAsync(cancellationToken);
            var workbook = new XLWorkbook();

            foreach (var faculty in faculties)
            {
                if (faculty is not null)
                { 
                    var worksheet = workbook.Worksheets.Add(faculty.Name);
                    WriteFacultyData(worksheet, faculty);
                }
            }

            workbook.SaveAs(stream);
        }
    }
}
