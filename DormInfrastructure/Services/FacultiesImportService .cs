using ClosedXML.Excel;

using DormDomain.Model;
using Microsoft.EntityFrameworkCore;


namespace DormInfrastructure.Services
{

    public class FacultiesImportService : IImportService<Faculty>
    {
        private readonly Do2Context _context;
       

        public FacultiesImportService(Do2Context context)
        {
            _context = context;
        }

        public async Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken)
        {
            if (!stream.CanRead)
            {
                throw new ArgumentException("Дані не можуть бути прочитані", nameof(stream));
            }

            using (XLWorkbook workBook = new XLWorkbook(stream))
            {
             
                foreach (IXLWorksheet worksheet in workBook.Worksheets)
                {
                   

                    var facultyName = worksheet.Name;
                    var faculty = await _context.Faculties.FirstOrDefaultAsync(f => f.Name == facultyName, cancellationToken);
                    if (faculty == null)
                    {

                        faculty = new Faculty(); 
                        faculty.Name = facultyName; 
                        faculty.Address = "From Excel"; 
                        _context.Faculties.Add(faculty); 
                        await _context.SaveChangesAsync(cancellationToken);
                    }
                                      
                    foreach (var row in worksheet.RowsUsed().Skip(1))
                    
                    {
                        await AddDepartmentAsync(row, cancellationToken, faculty);
                    }
                }
            }
            await _context.SaveChangesAsync(cancellationToken);
        }



        private async Task AddDepartmentAsync(IXLRow row, CancellationToken cancellationToken, Faculty faculty)
        {

            Department department = new Department();

            department.Name = GetDepartmentName(row);


            department.Faculty = faculty;

            _context.Departments.Add(department);

            await _context.SaveChangesAsync(cancellationToken);

            await GetStudentsAsync(row, department, cancellationToken);
        }

        private static string GetDepartmentName(IXLRow row)
        {
            return row.Cell(2).Value.ToString();
        }


        private async Task GetStudentsAsync(IXLRow row, Department department, CancellationToken cancellationToken)
        {
            var fullString = row.Cell(1).Value.ToString().Trim();

            if (!string.IsNullOrEmpty(fullString))
            {

                var parts = fullString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var lastName = parts.Length > 0 ? parts[0] : "Невідомо";
                var firstName = parts.Length > 1 ? parts[1] : "Невідомо";
                var middleName = parts.Length > 2 ? parts[2] : "";
                var studentExists = await _context.Students
                    .AnyAsync(s => s.LastName == lastName && s.FirstName == firstName && s.DepartmentId == department.Id, cancellationToken);

                if (!studentExists)
                {
                    var student = new Student();

                    student.LastName = lastName;
                    student.FirstName = firstName;
                    student.MiddleName = middleName;

                    student.DepartmentId = department.Id;

                    _context.Students.Add(student);
                }



                var studentName = row.Cell(1).Value.ToString().Trim();

            }
        }
    }
}

