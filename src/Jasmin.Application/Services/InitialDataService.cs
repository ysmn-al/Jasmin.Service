using Jasmin.Common.Helpers;
using Jasmin.Common.Services;
using Jasmin.Db.DB;
using Jasmin.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Jasmin.Service.Services;

public class InitialDataService
{
    private readonly IJasminDBContext _dbContext;
    private readonly ILogger<InitialDataService> _logger;

    /// <summary>
    /// Конструктор
    /// </summary>
    public InitialDataService(IJasminDBContext dbContext,
        ILogger<InitialDataService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Init()
    {
        if (await _dbContext.Teachers.AnyAsync())
            return;
        var user = new Teacher()
        {
            Login = "admin",
            Password = PasswordHasher.HashPassword("admin"),
            Post = Common.Enums.PostType.Deputy,
            IsActive = true,
            Rate = 0.0f
        };
        await _dbContext.Teachers.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

}
