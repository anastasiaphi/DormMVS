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
                    // 1. Факультет = Назва вкладки
                    var facultyName = worksheet.Name.Trim();
                    var faculty = await _context.Faculties
                        .FirstOrDefaultAsync(f => f.Name == facultyName, cancellationToken);

                    if (faculty == null)
                    {
                        faculty = new Faculty();
                        faculty.Name = facultyName;
                        faculty.Address = "Імпортовано з Excel"; 
                        faculty.Email = "n/a";
                        _context.Faculties.Add(faculty);

                        await _context.SaveChangesAsync(cancellationToken);
                    }

                    foreach (var row in worksheet.RowsUsed().Skip(1))
                    {
                        if (row.Cell(1).IsEmpty()) continue;
                        await AddDepartmentAsync(row, cancellationToken, faculty);
                    }
                }
            }
            await _context.SaveChangesAsync(cancellationToken);
        }


        private async Task<Degree> AddDegreeAsync(IXLRow row, CancellationToken cancellationToken)
        {
            var degreeName = GetDegreeName(row);

            var degree = await _context.Degrees
                .FirstOrDefaultAsync(d => d.DegreeName == degreeName, cancellationToken);

            if (degree == null)
            {
                degree = new Degree { DegreeName = degreeName };
                _context.Degrees.Add(degree);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return degree;
        }
        private async Task AddDepartmentAsync(IXLRow row, CancellationToken cancellationToken, Faculty faculty)
        {
          

            var deptName = GetDepartmentName(row);

            var department = await _context.Departments
         .FirstOrDefaultAsync(d => d.Name == deptName && d.FacultyId == faculty.Id, cancellationToken);

            if (department == null)
            {
                department = new Department
                {
                    Name = deptName,
                    FacultyId = faculty.Id
                };
                _context.Departments.Add(department);
                await _context.SaveChangesAsync(cancellationToken);
            }
            var degree = await AddDegreeAsync(row, cancellationToken);

            await GetStudentsAsync(row, department, degree, cancellationToken);
        }

        private static string GetDepartmentName(IXLRow row)
        {
            return row.Cell(2).Value.ToString();
        }
        private static string GetPhoneNumber(IXLRow row)
        {
            return row.Cell(3).Value.ToString().Trim();
        }

        private static string GetDegreeName(IXLRow row)
        {
            return row.Cell(4).Value.ToString().Trim();
        }

        private async Task GetStudentsAsync(IXLRow row, Department department, Degree degree, CancellationToken cancellationToken)
        {
            var fullString = row.Cell(1).Value.ToString().Trim();
            if (string.IsNullOrWhiteSpace(fullString)) return;

            var parts = fullString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2) return;

            var phoneNumber = GetPhoneNumber(row);

            var studentExists = await _context.Students
                .AnyAsync(s => s.LastName == parts[0] && s.FirstName == parts[1] && s.DepartmentId == department.Id, cancellationToken);

            if (!studentExists)
            {
                var student = new Student
                {
                    LastName = parts[0],
                    FirstName = parts[1],
                    MiddleName = parts.Length > 2 ? parts[2] : "",
                    DepartmentId = department.Id,
                    DegreeId = degree.Id, 
                    PhoneNumber = phoneNumber
                };

                _context.Students.Add(student);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}

