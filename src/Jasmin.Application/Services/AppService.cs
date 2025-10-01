using Jasmin.Common.Dto.Output;
using Jasmin.Db.DB;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Jasmin.Db.Entities;
using Jasmin.Common.Dto.Input;
using System.Collections.Generic;
using Jasmin.Common.Helpers;
using System;
using System.Linq;
using Aspose.Cells;
using System.IO;
using Npgsql;
using Jasmin.Common.Enums;

namespace Jasmin.Common.Services;

/// <summary>
/// Основной сервис
/// </summary>
public class AppService : IAppService
{
    private readonly IJasminDBContext _dbContext;
    private readonly ILogger<AppService> _logger;

    /// <summary>
    /// Конструктор
    /// </summary>
    public AppService(IJasminDBContext dbContext,
        ILogger<AppService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    #region IAppService

    #region Учебные года

    /// <inheritdoc/>
    public async Task<List<YearDto>> GetYears()
    {
        _logger.LogDebug("Получен запрос GetYears");
        var years = new List<YearDto>();

        foreach (var year in await _dbContext.Years.ToListAsync())
        {
            years.Add(new YearDto()
            {
                Id = year.Id,
                AcademicYear = year.AcademicYear
            });
        }
        return years;
    }

    /// <inheritdoc/>
    public async Task<bool> AddYear(YearInDto newOne)
    {
        var year = await GetYear(newOne, false);
        if (year != null)
        {
            _logger.LogWarning("Год уже существует");
            return false;
        }

        await _dbContext.Years.AddAsync(new Year()
        {
            AcademicYear = newOne.AcademicYear
        });

        await _dbContext.SaveChangesAsync();

        return true;
    }

    #endregion

    #region Преподаватели

    public async Task<bool> UpdatePassword(PasswordInDto passwordDto)
    {
        // Получите пользователя из базы данных по его логину
        var user = await _dbContext.Teachers.FirstOrDefaultAsync(t => t.Login == passwordDto.Login);

        if (user == null)
        {
            _logger.LogWarning("Пользователь с логином {login} не найден", passwordDto.Login);
            return false; // Пользователь не найден
        }

        // Проверьте, соответствует ли текущий пароль
        var passwordCheck = PasswordHasher.HashPassword(passwordDto.CurrentPassword);
        if (!passwordCheck.Equals(user.Password))
        {
            _logger.LogWarning("Текущий пароль неверный для пользователя {login}", passwordDto.Login);
            return false; // Текущий пароль неверный
        }

        // Обновите пароль
        user.Password = PasswordHasher.HashPassword(passwordDto.NewPassword);
        _dbContext.Teachers.Update(user);
        await _dbContext.SaveChangesAsync();

        return true; // Пароль успешно обновлен
    }

    /// <inheritdoc/>
    public async Task<bool> AddTeacher(TeacherInDto person)
    {

        var teacher = await GetTeacher(person.Id, false);
        if (teacher != null)
        {
            _logger.LogWarning("Преподаватель {surname} уже существует", person.Surname);
            return false;
        }

        var hashedPassword = PasswordHasher.HashPassword(person.Password);

        await _dbContext.Teachers.AddAsync(new Teacher()
        {
            Surname = person.Surname,
            Name = person.Name,
            Patronymic = person.Patronymic,
            Login = person.Login,
            Password = hashedPassword, // сохраняем хэшированный пароль
            IsActive = person.IsActive,
            Post = person.Post,
            Rate = person.Rate
        });

        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateTeacher(TeacherDto updateTeacher)
    {
        var teacher = await _dbContext.Teachers.FindAsync(updateTeacher.Id);
        if (teacher == null)
        {
            _logger.LogWarning("Преподаватель с ID {id} не найден", updateTeacher.Id);
            return false; // Преподаватель не найден
        }

        // Обновляем поля
        teacher.Surname = updateTeacher.Surname;
        teacher.Name = updateTeacher.Name;
        teacher.Patronymic = updateTeacher.Patronymic;
        teacher.Login = updateTeacher.Login;
        teacher.IsActive = updateTeacher.IsActive;
        teacher.Post = updateTeacher.Post;
        teacher.Rate = updateTeacher.Rate;

        // Если пароль изменился, хэшируем его
        if (!string.IsNullOrEmpty(updateTeacher.Password))
        {
            teacher.Password = PasswordHasher.HashPassword(updateTeacher.Password);
        }

        _dbContext.Teachers.Update(teacher);
        await _dbContext.SaveChangesAsync();

        return true; // Успешно обновлено
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteTeacher(long id)
    {
        var teacher = await GetTeacher(id, false);
        if (teacher == null)
            return false;

        _dbContext.Teachers.Remove(teacher);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc/>
    public async Task<List<TeacherDto>> GetTeachers()
    {
        _logger.LogDebug("Получен запрос GetTeachers");
        var teachers = new List<TeacherDto>();

        foreach (var teacher in await _dbContext.Teachers.ToListAsync())
        {
            teachers.Add(new TeacherDto()
            {
                Id = teacher.Id,
                Surname = teacher.Surname,
                Name = teacher.Name,
                Patronymic = teacher.Patronymic,
                Login = teacher.Login,
                Password = teacher.Password,
                IsActive = teacher.IsActive,
                Post = teacher.Post,
                Rate = teacher.Rate
            });
        }
        return teachers;

    }

    /// <inheritdoc/>
    public async Task<TeacherDto> GetTeacherByLogin(LoginDto model)
    {
        _logger.LogDebug("Получен запрос GetTeacher");
        var teacher = await _dbContext.Teachers.FirstOrDefaultAsync(x => x.Login == model.Login);
        if (teacher == null)
            return null;
        var pwd = PasswordHasher.HashPassword(model.Password);
        
        if (!pwd.Equals(teacher.Password))
            return null;

        return new TeacherDto()
        {
            Id = teacher.Id,
            Surname = teacher.Surname,
            Name = teacher.Name,
            Patronymic = teacher.Patronymic,
            Login = teacher.Login,
            IsActive = teacher.IsActive,
            Post = teacher.Post,
            Rate = teacher.Rate
        };
    }

    #endregion

    #region Предмет

    /// <inheritdoc/>

    public async Task<bool> AddSubject(SubjectInDto newOne)
    {
        var subject = await GetSubject(newOne, false);
        if (subject != null)
        {
            _logger.LogWarning("Предмет {name} уже существует", newOne.Name);
            return false;
        }

        await _dbContext.Subjects.AddAsync(new Subject()
        {
            Name = newOne.Name,
            BeginYearId = newOne.BeginYearId,
            EndYearId = newOne.EndYearId
        });

        await _dbContext.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteSubject(SubjectInDto newOne)
    {
        var subject = await GetSubject(newOne, false);
        if (subject == null)
            return false;

        _dbContext.Subjects.Remove(subject);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc/>
    public async Task<List<SubjectDto>> GetSubjects()
    {
        _logger.LogDebug("Получен запрос GetSubjects");
        var subjects = new List<SubjectDto>();

        foreach (var subject in await _dbContext.Subjects.ToListAsync())
        {
            subjects.Add(new SubjectDto()
            {
                Id = subject.Id,
                Name = subject.Name,
                BeginYearId = subject.BeginYearId,
                EndYearId = subject.EndYearId
            });
        }
        return subjects;
    }

    #endregion

    #region Группа

    /// <inheritdoc/>
    public async Task<bool> AddUnit(UnitInDto newOne)
    {
        var unit = await GetUnit(newOne.Number, false);
        if (unit != null)
        {
            _logger.LogWarning("Группа {number} уже существует", newOne.Number);
            return false;
        }

        await _dbContext.Units.AddAsync(new Unit()
        {
            Faculty = newOne.Faculty,
            Number = newOne.Number
        });

        await _dbContext.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteUnit(string number)
    {
        var unit = await GetUnit(number, false);
        if (unit == null)
            return false;

        _dbContext.Units.Remove(unit);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc/>
    public async Task<List<UnitDto>> GetUnits()
    {
        _logger.LogDebug("Получен запрос GetUnits");
        var units = new List<UnitDto>();

        foreach (var unit in await _dbContext.Units.ToListAsync())
        {
            units.Add(new UnitDto()
            {
                Id = unit.Id,
                Faculty = unit.Faculty,
                Number = unit.Number
            });
        }
        return units;
    }

    #endregion

    #region Актуальная нагрузка

    /// <inheritdoc/>
    public async Task<bool> AddActualLoad(ActualLoadInDto newOne)
    {
        var actLoad = await GetActualLoad(newOne.PlannedLoadId, newOne.TeacherId, false);
        if (actLoad != null)
        {
            _logger.LogWarning("Связь уже существует");
            return false;
        }

        await _dbContext.ActualLoads.AddAsync(new ActualLoad()
        {
            PlannedLoadId = newOne.PlannedLoadId,
            TeacherId = newOne.TeacherId,
            Lecture = newOne.Lecture,
            Lesson = newOne.Lesson,
            Labwork = newOne.Labwork,
            Coursework = newOne.Coursework,
            CourseProject = newOne.CourseProject,
            Сonsultation = newOne.Сonsultation,
            Exam = newOne.Exam,
            Rating = newOne.Rating,
            Credit = newOne.Credit,
            Practice = newOne.Practice
        });

        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateActualLoad(long id, ActualLoadDto updatedLoad)
    {
        var actLoad = await _dbContext.ActualLoads.FindAsync(id);
        if (actLoad == null)
        {
            _logger.LogWarning("Запись с Id {Id} не найдена", id);
            return false;
        }

        // Обновляем поля
        actLoad.PlannedLoadId = updatedLoad.PlannedLoadId;
        actLoad.TeacherId = updatedLoad.TeacherId;
        actLoad.Lecture = updatedLoad.Lecture;
        actLoad.Lesson = updatedLoad.Lesson;
        actLoad.Labwork = updatedLoad.Labwork;
        actLoad.Coursework = updatedLoad.Coursework;
        actLoad.CourseProject = updatedLoad.CourseProject;
        actLoad.Сonsultation = updatedLoad.Consultation;
        actLoad.Exam = updatedLoad.Exam;
        actLoad.Rating = updatedLoad.Rating;
        actLoad.Credit = updatedLoad.Credit;
        actLoad.Practice = updatedLoad.Practice;

        await _dbContext.SaveChangesAsync();
        return true;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteActualLoad(long delOne, long teacherId)
    {
        var actLoad = await GetActualLoad(delOne, teacherId, false);
        if (actLoad == null)
            return false;

        _dbContext.ActualLoads.Remove(actLoad);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc/>

    public async Task<List<ActualLoadDto>> GetActualLoads()
    {
        _logger.LogDebug("Получен запрос GetActualLoads");

        var actLoads = await (from actLoad in _dbContext.ActualLoads
                              join teacher in _dbContext.Teachers on actLoad.TeacherId equals teacher.Id
                              join plannedLoad in _dbContext.PlannedLoads on actLoad.PlannedLoadId equals plannedLoad.Id
                              join subject in _dbContext.Subjects on plannedLoad.SubjectId equals subject.Id
                              join unit in _dbContext.Units on plannedLoad.UnitId equals unit.Id
                              select new ActualLoadDto
                              {
                                  Id = actLoad.Id,
                                  TeacherId = actLoad.TeacherId,
                                  TeacherName = $"{teacher.Surname} {teacher.Name} {teacher.Patronymic}",
                                  PlannedLoadId = actLoad.PlannedLoadId,
                                  SubjectName = subject.Name,
                                  UnitNumber = unit.Number,
                                  Semester = (int)plannedLoad.Semester,
                                  Lecture = actLoad.Lecture,
                                  Lesson = actLoad.Lesson,
                                  Labwork = actLoad.Labwork,
                                  Coursework = actLoad.Coursework,
                                  CourseProject = actLoad.CourseProject,
                                  Consultation = actLoad.Сonsultation,
                                  Exam = actLoad.Exam,
                                  Rating = actLoad.Rating,
                                  Credit = actLoad.Credit,
                                  Practice = actLoad.Practice
                              }).ToListAsync();

        return actLoads;
    }

    public async Task<List<ActualLoadDto>> GetActualLoadsByTeacherIdAndYearId(long teacherId, long yearId)
    {
        _logger.LogDebug("Получен запрос GetActualLoadsByTeacherIdAndYearId");

        // Получаем данные с группировкой
        var actualLoads = await (from actualLoad in _dbContext.ActualLoads
                                 join plannedLoad in _dbContext.PlannedLoads on actualLoad.PlannedLoadId equals plannedLoad.Id
                                 join subject in _dbContext.Subjects on plannedLoad.SubjectId equals subject.Id
                                 where actualLoad.TeacherId == teacherId && plannedLoad.YearId == yearId
                                 select new
                                 {
                                     SubjectName = subject.Name,
                                     actualLoad.Lecture,
                                     actualLoad.Lesson,
                                     actualLoad.Labwork,
                                     actualLoad.Coursework,
                                     actualLoad.CourseProject,
                                     actualLoad.Сonsultation,
                                     actualLoad.Exam,
                                     actualLoad.Rating,
                                     actualLoad.Credit,
                                     actualLoad.Practice,
                                     Semester = (int)plannedLoad.Semester // Добавляем идентификатор семестра
                                 }).ToListAsync();

        // Группируем по SubjectName и суммируем остальные поля
        var groupedLoads = actualLoads
            .GroupBy(x => new { x.SubjectName, x.Semester }) // Группируем также по SemesterId
            .Select(g => new ActualLoadDto
            {
                SubjectName = g.Key.SubjectName,
                Lecture = g.Sum(x => x.Lecture),
                Lesson = g.Sum(x => x.Lesson),
                Labwork = g.Sum(x => x.Labwork),
                Coursework = g.Sum(x => x.Coursework),
                CourseProject = g.Sum(x => x.CourseProject),
                Consultation = g.Sum(x => x.Сonsultation),
                Exam = g.Sum(x => x.Exam),
                Rating = g.Sum(x => x.Rating),
                Credit = g.Sum(x => x.Credit),
                Practice = g.Sum(x => x.Practice),
                Semester = (int)g.Key.Semester // Добавляем идентификатор семестра в результат
            }).ToList();

        return groupedLoads;
    }

    #endregion

    #region Плановая нагрузка

    /// <inheritdoc/>
    public async Task<bool> AddPlannedLoad(PlannedLoadInDto newOne)
    {
        var actLoad = await GetPlannedLoad(newOne.YearId, (int)newOne.Semester, newOne.SubjectId, newOne.UnitId, false);
        if (actLoad != null)
        {
            _logger.LogWarning("Связь уже существует");
            return false;
        }

        await _dbContext.PlannedLoads.AddAsync(new PlannedLoad()
        {
            SubjectId = newOne.SubjectId,
            YearId = newOne.YearId,
            UnitId = newOne.UnitId,
            Semester = newOne.Semester,
            Lecture = newOne.Lecture,
            Lesson = newOne.Lesson,
            Labwork = newOne.Labwork,
            Coursework = newOne.Coursework,
            CourseProject = newOne.CourseProject,
            Сonsultation = newOne.Сonsultation,
            Exam = newOne.Exam,
            Rating = newOne.Rating,
            Credit = newOne.Credit,
            Practice = newOne.Practice
        });

        await _dbContext.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc/>
    public async Task<bool> DeletePlannedLoad(long yearId, int semester, long subjectId, long unitId)
    {
        var plLoad = await GetPlannedLoad(yearId, semester, subjectId, unitId, false);
        if (plLoad == null)
            return false;


        _dbContext.PlannedLoads.Remove(plLoad);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc/>
    public async Task<List<PlannedLoadDto>> GetPlannedLoads()
    {
        _logger.LogDebug("Получен запрос GetPlannedLoads");
        var plLoads = new List<PlannedLoadDto>();

        foreach (var plLoad in await _dbContext.PlannedLoads.ToListAsync())
        {
            plLoads.Add(new PlannedLoadDto()
            {
                Id = plLoad.Id,
                SubjectId = plLoad.SubjectId,
                YearId = plLoad.YearId,
                UnitId = plLoad.UnitId,
                Semester = plLoad.Semester,
                Lecture = plLoad.Lecture,
                Lesson = plLoad.Lesson,
                Labwork = plLoad.Labwork,
                Coursework = plLoad.Coursework,
                CourseProject = plLoad.CourseProject,
                Сonsultation = plLoad.Сonsultation,
                Exam = plLoad.Exam,
                Rating = plLoad.Rating,
                Credit = plLoad.Credit,
                Practice = plLoad.Practice
            });
        }
        return plLoads;
    }


    //public async Task<List<PlannedLoadDto>> GetPLforSelf(long yearId, int semesterId, long subjectId, string faculty)
    //{
    //    _logger.LogDebug("Получен запрос GetPLforSelf для предмета: {subjectId}", subjectId);

    //    var plLoads = await (from plLoad in _dbContext.PlannedLoads
    //                         join subject in _dbContext.Subjects on plLoad.SubjectId equals subject.Id // Исправлено на subject.Id
    //                         join unit in _dbContext.Units on plLoad.UnitId equals unit.Id
    //                         where (int)plLoad.Semester == semesterId && plLoad.YearId == yearId && plLoad.SubjectId == subjectId
    //                         select new PlannedLoadDto // Заменено на PlannedLoadDto
    //                         {
    //                             Id = plLoad.Id,
    //                             SubjectId = plLoad.SubjectId,
    //                             SubjectName = subject.Name,
    //                             UnitId = plLoad.UnitId,
    //                             UnitName = unit.Number,
    //                             Lecture = plLoad.Lecture,
    //                             Lesson = plLoad.Lesson,
    //                             Labwork = plLoad.Labwork,
    //                             Coursework = plLoad.Coursework,
    //                             CourseProject = plLoad.CourseProject,
    //                             Сonsultation = plLoad.Сonsultation,
    //                             Exam = plLoad.Exam,
    //                             Rating = plLoad.Rating,
    //                             Credit = plLoad.Credit,
    //                             Practice = plLoad.Practice
    //                         }).ToListAsync(); // Изменено на ToListAsync()

    //    return plLoads;
    //}

    public async Task<List<PlannedLoadDto>> GetPLforSelf(long yearId, int semesterId, long subjectId, string faculty)
    {
        _logger.LogDebug("Получен запрос GetPLforSelf для предмета: {subjectId} и факультета: {faculty}", subjectId, faculty);

        var plLoads = await (from plLoad in _dbContext.PlannedLoads
                             join subject in _dbContext.Subjects on plLoad.SubjectId equals subject.Id
                             join unit in _dbContext.Units on plLoad.UnitId equals unit.Id
                             where (int)plLoad.Semester == semesterId
                                   && plLoad.YearId == yearId
                                   && plLoad.SubjectId == subjectId
                                   && unit.Faculty == faculty // Добавлено условие для фильтрации по факультету
                             select new PlannedLoadDto
                             {
                                 Id = plLoad.Id,
                                 SubjectId = plLoad.SubjectId,
                                 SubjectName = subject.Name,
                                 UnitId = plLoad.UnitId,
                                 UnitName = unit.Number,
                                 Lecture = plLoad.Lecture,
                                 Lesson = plLoad.Lesson,
                                 Labwork = plLoad.Labwork,
                                 Coursework = plLoad.Coursework,
                                 CourseProject = plLoad.CourseProject,
                                 Сonsultation = plLoad.Сonsultation,
                                 Exam = plLoad.Exam,
                                 Rating = plLoad.Rating,
                                 Credit = plLoad.Credit,
                                 Practice = plLoad.Practice
                             }).ToListAsync();

        return plLoads;
    }


    #endregion

    #region Импорт данных

    public async Task UploadFile(byte[] fileBytes)
    {
        using (var memoryStream = new MemoryStream(fileBytes))
        {
            var workbook = new Workbook(memoryStream);
            string connectionString = "Host=localhost;Port=5432;Database=ServiceDB;Username=postgres;Password=New0101*";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var sheetStructureMap = new Dictionary<string, Func<Worksheet, NpgsqlConnection, Task>>
            {
                { "Годы", ProcessYears },                // Имя листа с пробелами, если есть
                { "Предметы", ProcessSubjects },         // Имя листа с пробелами, если есть
                { "Группы", ProcessUnits },       // Имя листа с пробелами, если есть
                { "Актуальная", ProcessActualLoads }, // Имя листа с пробелами, если есть
                { "Плановая", ProcessPlannedLoads },   // Имя листа с пробелами, если есть
                {"Руководство", ProcessGuides },
                {"Часы", ProcessGuideHours }
            };

                for (int sheetIndex = 0; sheetIndex < workbook.Worksheets.Count; sheetIndex++)
                {
                    var worksheet = workbook.Worksheets[sheetIndex];
                    string sheetName = worksheet.Name.Trim(); // Удаление лишних пробелов

                    Console.WriteLine($"Обработка листа: '{sheetName}'"); // Для отладки

                    if (sheetStructureMap.ContainsKey(sheetName))
                    {
                        await sheetStructureMap[sheetName](worksheet, connection);
                    }
                    else
                    {
                        Console.WriteLine($"Неизвестный лист: {sheetName}");
                    }
                }
            }
        }
    }

    private async Task ProcessYears(Worksheet worksheet, NpgsqlConnection connection)
    {
        int rowCount = worksheet.Cells.MaxDataRow + 1;

        for (int row = 1; row < rowCount; row++) // Начинаем с 1, если первая строка - заголовки
        {
            string academicYear = worksheet.Cells[row, 0]?.Value?.ToString();

            var newYearDto = new YearInDto
            {
                AcademicYear = academicYear
            };

            await AddYear(newYearDto);
        }
    }

    private async Task ProcessSubjects(Worksheet worksheet, NpgsqlConnection connection)
    {
        int rowCount = worksheet.Cells.MaxDataRow + 1;

        for (int row = 1; row < rowCount; row++)
        {
            string name = worksheet.Cells[row, 0]?.Value?.ToString();
            string beginYearId = worksheet.Cells[row, 1]?.Value?.ToString();
            string endYearId = worksheet.Cells[row, 2]?.Value?.ToString();

            var newSubjectDto = new SubjectInDto
            {
                Name = name,
                BeginYearId = beginYearId,
                EndYearId = endYearId
            };

            await AddSubject(newSubjectDto);
        }
    }

    private async Task ProcessUnits(Worksheet worksheet, NpgsqlConnection connection)
    {
        int rowCount = worksheet.Cells.MaxDataRow + 1;

        for (int row = 1; row < rowCount; row++) // Начинаем с 1, если первая строка - заголовки
        {
            string faculty = worksheet.Cells[row, 0]?.Value?.ToString();
            string number = worksheet.Cells[row, 1]?.Value?.ToString();

            var newUnitDto = new UnitInDto
            {
                Faculty = faculty,
                Number = number
            };

            await AddUnit(newUnitDto);
        }
    }

    private async Task ProcessActualLoads(Worksheet worksheet, NpgsqlConnection connection)
    {
        int rowCount = worksheet.Cells.MaxDataRow + 1;

        for (int row = 1; row < rowCount; row++)
        {
            string plannedLoadId = worksheet.Cells[row, 0]?.Value?.ToString();
            string teacherId = worksheet.Cells[row, 1]?.Value?.ToString();

            // Используем вспомогательный метод для преобразования или присвоения 0
            long lecture = ParseOrDefault(worksheet.Cells[row, 2]?.Value?.ToString());
            long lesson = ParseOrDefault(worksheet.Cells[row, 3]?.Value?.ToString());
            long labwork = ParseOrDefault(worksheet.Cells[row, 4]?.Value?.ToString());
            long coursework = ParseOrDefault(worksheet.Cells[row, 5]?.Value?.ToString());
            long courseProject = ParseOrDefault(worksheet.Cells[row, 6]?.Value?.ToString());
            long consultation = ParseOrDefault(worksheet.Cells[row, 7]?.Value?.ToString());
            long exam = ParseOrDefault(worksheet.Cells[row, 8]?.Value?.ToString());
            long rating = ParseOrDefault(worksheet.Cells[row, 9]?.Value?.ToString());
            long credit = ParseOrDefault(worksheet.Cells[row, 10]?.Value?.ToString());
            long practice = ParseOrDefault(worksheet.Cells[row, 11]?.Value?.ToString());

            var newActualLoadDto = new ActualLoadInDto
            {
                PlannedLoadId = long.Parse(plannedLoadId),
                TeacherId = long.Parse(teacherId),
                Lecture = lecture,
                Lesson = lesson,
                Labwork = labwork,
                Coursework = coursework,
                CourseProject = courseProject,
                Сonsultation = consultation,
                Exam = exam,
                Rating = rating,
                Credit = credit,
                Practice = practice
            };

            await AddActualLoad(newActualLoadDto);
        }
    }

    // Вспомогательный метод для обработки парсинга
    private long ParseOrDefault(string value)
    {
        return string.IsNullOrEmpty(value) ? 0 : long.Parse(value);
    }

    private async Task ProcessPlannedLoads(Worksheet worksheet, NpgsqlConnection connection)
    {
        int rowCount = worksheet.Cells.MaxDataRow + 1;

        for (int row = 1; row < rowCount; row++)
        {
            string subjectId = worksheet.Cells[row, 0]?.Value?.ToString();
            string yearId = worksheet.Cells[row, 1]?.Value?.ToString();
            string unitId = worksheet.Cells[row, 2]?.Value?.ToString();
            string semester = worksheet.Cells[row, 3]?.Value?.ToString();

            // Используем вспомогательный метод для преобразования или присвоения 0
            long lecture = ParseOrDefault(worksheet.Cells[row, 4]?.Value?.ToString());
            long lesson = ParseOrDefault(worksheet.Cells[row, 5]?.Value?.ToString());
            long labwork = ParseOrDefault(worksheet.Cells[row, 6]?.Value?.ToString());
            long coursework = ParseOrDefault(worksheet.Cells[row, 7]?.Value?.ToString());
            long courseProject = ParseOrDefault(worksheet.Cells[row, 8]?.Value?.ToString());
            long consultation = ParseOrDefault(worksheet.Cells[row, 9]?.Value?.ToString());
            long rating = ParseOrDefault(worksheet.Cells[row, 10]?.Value?.ToString());
            long credit = ParseOrDefault(worksheet.Cells[row, 11]?.Value?.ToString());
            long exam = ParseOrDefault(worksheet.Cells[row, 12]?.Value?.ToString());
            long practice = ParseOrDefault(worksheet.Cells[row, 13]?.Value?.ToString());

            SemesterType semesterType;

            // Определение семестра по русским названиям
            if (semester.Equals("Осень", StringComparison.OrdinalIgnoreCase) || semester == "1")
            {
                semesterType = SemesterType.Autumn;
            }
            else if (semester.Equals("Весна", StringComparison.OrdinalIgnoreCase) || semester == "2")
            {
                semesterType = SemesterType.Spring;
            }
            else
            {
                Console.WriteLine($"Ошибка преобразования семестра: '{semester}' не является допустимым значением.");
                continue; // Пропускаем текущую итерацию в случае ошибки
            }

            var newPlannedLoadDto = new PlannedLoadInDto
            {
                SubjectId = long.Parse(subjectId),
                YearId = long.Parse(yearId),
                Semester = semesterType,
                UnitId = long.Parse(unitId),
                Lecture = lecture,
                Lesson = lesson,
                Labwork = labwork,
                Coursework = coursework,
                CourseProject = courseProject,
                Сonsultation = consultation,
                Exam = exam,
                Rating = rating,
                Credit = credit,
                Practice = practice
            };

            await AddPlannedLoad(newPlannedLoadDto);
        }
    }

    private async Task ProcessGuides(Worksheet worksheet, NpgsqlConnection connection)
    {
        int rowCount = worksheet.Cells.MaxDataRow + 1;

        for (int row = 1; row < rowCount; row++)
        {
            string yearId = worksheet.Cells[row,0]?.Value?.ToString();
            string teacherId = worksheet.Cells[row, 1]?.Value?.ToString();
            long bachelor = ParseOrDefault(worksheet.Cells[row, 2]?.Value?.ToString());
            long masterOne = ParseOrDefault(worksheet.Cells[row, 3]?.Value?.ToString());
            long masterTwo = ParseOrDefault(worksheet.Cells[row, 4]?.Value?.ToString());
            long postgraduate = ParseOrDefault(worksheet.Cells[row, 5]?.Value?.ToString());
            long nirs = ParseOrDefault(worksheet.Cells[row, 6]?.Value?.ToString());
            long department = ParseOrDefault(worksheet.Cells[row, 7]?.Value?.ToString());

            var newGuideDto = new GuideInDto
            {
                YearId = long.Parse(yearId),
                TeacherId = long.Parse(teacherId),
                Bachelor = bachelor,
                MasterOne = masterOne,
                MasterTwo = masterTwo,
                Postgraduate = postgraduate,
                NIRS = nirs,
                Department = department
            };

            await AddGuide(newGuideDto);
        }
    }

    private async Task ProcessGuideHours(Worksheet worksheet, NpgsqlConnection connection)
    {
        int rowCount = worksheet.Cells.MaxDataRow + 1;

        for (int row = 1; row < rowCount; row++)
        {
            string type = worksheet.Cells[row, 0]?.Value?.ToString();
            long semesterHours = ParseOrDefault(worksheet.Cells[row, 1]?.Value?.ToString());
            long defenceHours = ParseOrDefault(worksheet.Cells[row, 2]?.Value?.ToString());

            var newGuideHourDto = new GuideHourInDto
            {
                Type = type,
                SemesterHours = semesterHours,
                DefenseHours = defenceHours
            };

            await AddGuideHour(newGuideHourDto);
        }
    }

    #endregion

    #region Руководство

    /// <inheritdoc/>
    public async Task<bool> AddGuide(GuideInDto newOne)
    {
        var guide = await GetGuide(newOne.TeacherId, newOne.YearId, false);
        if (guide != null)
        {
            _logger.LogWarning("Запись о руководстве преподавателя с id {teacherId} уже существует", newOne.TeacherId);
            return false;
        }

        await _dbContext.Guides.AddAsync(new Guide()
        {
            YearId = newOne.YearId,
            TeacherId = newOne.TeacherId,
            Bachelor = newOne.Bachelor,
            MasterOne = newOne.MasterOne,
            MasterTwo = newOne.MasterTwo,
            Postgraduate = newOne.Postgraduate,
            NIRS = newOne.NIRS,
            Department = newOne.Department
        });

        await _dbContext.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteGuide(long teacherId, long yearId)
    {
        var guide = await GetGuide(teacherId, yearId, false);
        if (guide == null)
            return false;

        _dbContext.Guides.Remove(guide);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateGuideHour(GuideHourDto updateGuideHour)
    {
        var guideHour = await _dbContext.GuideHours.FindAsync(updateGuideHour.Id);
        if (guideHour == null)
        {
            _logger.LogWarning(" не найден");
            return false; // Преподаватель не найден
        }

        // Обновляем поля
        guideHour.Type = updateGuideHour.Type;
        guideHour.SemesterHours = updateGuideHour.SemesterHours;
        guideHour.DefenseHours = updateGuideHour.DefenseHours;

        _dbContext.GuideHours.Update(guideHour);
        await _dbContext.SaveChangesAsync();

        return true; // Успешно обновлено
    }

    /// <inheritdoc/>
    public async Task<List<GuideDto>> GetGuides()
    {
        _logger.LogDebug("Получен запрос GetGuides");
        var guides = new List<GuideDto>();

        foreach (var guide in await _dbContext.Guides.ToListAsync())
        {
            guides.Add(new GuideDto()
            {
                Id = guide.Id,
                YearId = guide.YearId,
                TeacherId = guide.TeacherId,
                Bachelor = guide.Bachelor,
                MasterOne = guide.MasterOne,
                MasterTwo = guide.MasterTwo,
                Postgraduate = guide.Postgraduate,
                NIRS = guide.NIRS,
                Department = guide.Department
            });
        }
        return guides;
    }

    /// <inheritdoc/>
    public async Task<bool> AddGuideHour(GuideHourInDto newOne)
    {
        var guideHour = await GetGuideHour(newOne.Type, false);
        if (guideHour != null)
        {
            _logger.LogWarning("Запись о часах руководства уже существует");
            return false;
        }

        await _dbContext.GuideHours.AddAsync(new GuideHour()
        {
            Type = newOne.Type,
            SemesterHours = newOne.SemesterHours,
            DefenseHours = newOne.DefenseHours
        });

        await _dbContext.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteGuideHour(string type)
    {
        var guideHour = await GetGuideHour(type, false);
        if (guideHour == null)
            return false;

        _dbContext.GuideHours.Remove(guideHour);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc/>
    public async Task<List<GuideHourDto>> GetGuideHours()
    {
        _logger.LogDebug("Получен запрос GetGuideHours");
        var guideHours = new List<GuideHourDto>();

        foreach (var guideHour in await _dbContext.GuideHours.ToListAsync())
        {
            guideHours.Add(new GuideHourDto()
            {
                Id = guideHour.Id,
                Type = guideHour.Type,
                SemesterHours = guideHour.SemesterHours,
                DefenseHours = guideHour.DefenseHours
            });
        }
        return guideHours;
    }

    public async Task<Dictionary<string, long>> CalculateTotalGuideHours(long teacherId, long yearId)
    {
        var guides = await GetGuides();
        var guide = guides.FirstOrDefault(g => g.TeacherId == teacherId && g.YearId == yearId);

        if (guide == null)
        {
            return new Dictionary<string, long>();
        }

        var guideHours = await GetGuideHours();

        var result = new Dictionary<string, long>
    {
        { "TotalHours", 0 },
        { "BachelorHours", 0 },
        { "MasterOneHours", 0 },
        { "MasterTwoHours", 0 },
        { "PostgraduateHours", 0 },
        { "NIRSHours", 0 },
        { "DepartmentHours", 0 }
    };

        // Считаем часы для бакалавров
        var bachelorHourData = guideHours.FirstOrDefault(gh => gh.Type == "Бакалавр");
        if (bachelorHourData != null && guide.Bachelor > 0)
        {
            long semesterHours = bachelorHourData.SemesterHours; // Используем значение как есть
            long defenseHours = bachelorHourData.DefenseHours; // Используем значение как есть

            _logger.LogInformation($"Calculating Bachelor Hours: {guide.Bachelor} * (2 * {semesterHours} + {defenseHours})");

            result["BachelorHours"] = guide.Bachelor * (2 * semesterHours + defenseHours);
            result["TotalHours"] += result["BachelorHours"];
        }

        // Аналогично для магистров первого года
        var masterOneHourData = guideHours.FirstOrDefault(gh => gh.Type == "Магистр год 1");
        if (masterOneHourData != null && guide.MasterOne > 0)
        {
            long semesterOne = masterOneHourData.SemesterHours; // Используем значение как есть
            long defenseOne = masterOneHourData.DefenseHours; // Используем значение как есть

            _logger.LogInformation($"Calculating Master One Hours: {guide.MasterOne} * (2 * {semesterOne} + {defenseOne})");

            result["MasterOneHours"] = guide.MasterOne * (2 * semesterOne + defenseOne);
            result["TotalHours"] += result["MasterOneHours"];
        }

        // Аналогично для магистров второго года
        var masterTwoHourData = guideHours.FirstOrDefault(gh => gh.Type == "Магистр год 2");
        if (masterTwoHourData != null && guide.MasterTwo > 0)
        {
            long semesterTwo = masterTwoHourData.SemesterHours; // Используем значение как есть
            long defenseTwo = masterTwoHourData.DefenseHours; // Используем значение как есть

            _logger.LogInformation($"Calculating Master Two Hours: {guide.MasterTwo} * (2 * {semesterTwo} + {defenseTwo})");

            result["MasterTwoHours"] += guide.MasterTwo * (2 * semesterTwo + defenseTwo);
            result["TotalHours"] += result["MasterTwoHours"];
        }

        // Аналогично для аспирантов
        var postgraduateHourData = guideHours.FirstOrDefault(gh => gh.Type == "Аспирант");
        if (postgraduateHourData != null && guide.Postgraduate > 0)
        {
            long semesterPostgraduate = postgraduateHourData.SemesterHours; // Используем значение как есть
            long defensePostgraduate = postgraduateHourData.DefenseHours; // Используем значение как есть

            _logger.LogInformation($"Calculating Postgraduate Hours: {guide.Postgraduate} * (2 * {semesterPostgraduate} + {defensePostgraduate})");

            result["PostgraduateHours"] += guide.Postgraduate * (2 * semesterPostgraduate + defensePostgraduate);
            result["TotalHors"] += result["PostgraduateHours"];
        }

        // Аналогично для НИРС
        var nirsHourData = guideHours.FirstOrDefault(gh => gh.Type == "НИРС");
        if (nirsHourData != null && guide.NIRS > 0)
        {
            long semesterNIRS = nirsHourData.SemesterHours; // Используем значение как есть
            long defenseNIRS = nirsHourData.DefenseHours; // Используем значение как есть

            _logger.LogInformation($"Calculating NIRS Hours: {guide.NIRS} * (2 * {semesterNIRS} + {defenseNIRS})");

            result["NIRSHours"] += guide.NIRS * (2 * semesterNIRS + defenseNIRS);
            result["TotalHours"] += result["NIRSHours"];
        }

        // Аналогично для руководства кафедрой
        var departmentHourData = guideHours.FirstOrDefault(gh => gh.Type == "Кафедра");
        if (nirsHourData != null && guide.NIRS > 0)
        {
            long semesterDepartment = departmentHourData.SemesterHours; // Используем значение как есть
            long defenseDepartment = departmentHourData.DefenseHours; // Используем значение как есть

            _logger.LogInformation($"Calculating Department Hours: {guide.Department} * 2 * {semesterDepartment}");

            result["DepartmentHours"] += guide.NIRS * 2 * semesterDepartment;
            result["TotalHours"] += result["DepartmentHours"];
        }

        return result;
    }

    #endregion

    #endregion

    #region Вспомогательные функции

    private async Task<Year> GetYear(YearInDto newOne, bool throwError = true)
    {
        var year = await _dbContext.Years.FirstOrDefaultAsync(a => a.AcademicYear == newOne.AcademicYear);
        if (year == null)
        {
            _logger.LogWarning("Год не найден");

            if (throwError)
                throw new System.Exception($"Год не найден");
        }

        return year;
    }

    private async Task<Teacher> GetTeacher(long id, bool throwError = true)
    {
        var teacher = await _dbContext.Teachers.FirstOrDefaultAsync(a => a.Id == id);
        if (teacher == null)
        {
            _logger.LogWarning("Преподаватель {id} не найден", id);

            if (throwError)
                throw new System.Exception($"Преподаватель {id} не найден");
        }

        return teacher;
    }

    private async Task<Subject> GetSubject(SubjectInDto newOne, bool throwError = true)
    {
        var subject = await _dbContext.Subjects.FirstOrDefaultAsync(a => a.Name == newOne.Name);
        if (subject == null)
        {
            _logger.LogWarning("Предмет не найден");

            if (throwError)
                throw new System.Exception($"Предмет не найден");
        }

        return subject;
    }


    private async Task<Unit> GetUnit(string number, bool throwError = true)
    {
        var group = await _dbContext.Units.FirstOrDefaultAsync(a => a.Number == number);
        if (group == null)
        {
            _logger.LogWarning("Группа {number} не найдена", number);

            if (throwError)
                throw new System.Exception($"Группа {number} не найдена");
        }

        return group;
    }

    private async Task<Guide> GetGuide(long teacherId, long yearId, bool throwError = true)
    {
        var guide = await _dbContext.Guides.FirstOrDefaultAsync(a => a.TeacherId == teacherId && a.YearId == yearId);
        if (guide == null)
        {
            _logger.LogWarning("Запись о руководстве не найдена");

            if (throwError)
                throw new System.Exception($"Запись {teacherId} не найдена");
        }

        return guide;
    }

    private async Task<GuideHour> GetGuideHour(string type, bool throwError = true)
    {
        var guideHour = await _dbContext.GuideHours.FirstOrDefaultAsync(a => a.Type == type);
        if (guideHour == null)
        {
            _logger.LogWarning("Запись о часах руководства не найдена");

            if (throwError)
                throw new System.Exception($"Запись о {type} не найдена");
        }

        return guideHour;
    }

    private async Task<ActualLoad> GetActualLoad(long actLoad, long teacherId, bool throwError = true)
    {
        var load = await _dbContext.ActualLoads.FirstOrDefaultAsync(a => a.PlannedLoadId == actLoad && a.TeacherId == teacherId);
        if (load == null)
        {
            _logger.LogWarning("Связь не найдена");

            if (throwError)
                throw new System.Exception($"Связь не найдена");
        }

        return load;
    }

    private async Task<PlannedLoad> GetPlannedLoad(long yearId, int semester, long subjectId, long unitId, bool throwError = true)
    {
        var load = await _dbContext.PlannedLoads.FirstOrDefaultAsync(a => a.YearId == yearId && (int)a.Semester == semester && a.SubjectId == subjectId && a.UnitId == unitId);
        if (load == null)
        {
            _logger.LogWarning("Связь не найдена");

            if (throwError)
                throw new System.Exception($"Связь не найдена");
        }

        return load;
    }

    #endregion
}
