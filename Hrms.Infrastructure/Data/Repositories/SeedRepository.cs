using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics.Metrics;

namespace Hrms.Infrastructure.Data.Repositories
{
    public class SeedRepository : ISeedRepository
    {
        private readonly ILogger _logger;
        private readonly DataContext _dataContext;
        private readonly RoleManager<Role<int>> _roleManager;
        private readonly UserManager<User> _userManager;

        public SeedRepository(ILogger<SeedRepository> logger,
            DataContext dataContext,
            RoleManager<Role<int>> roleManager,
            UserManager<User> userManager)
        {
            _logger = logger;
            _dataContext = dataContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task SeedRolesAsync()
        {
            try
            {
                IdentityResult identityResult;
                if (!await _roleManager.Roles.AnyAsync(x => x.Name == Constants.UserType.Admin))
                {
                    identityResult = await _roleManager.CreateAsync(new Role<int>
                    { Id = 1, Name = Constants.UserType.Admin, DisplayName = "Admin" });
                    _logger.LogInformation($"Role ({Constants.UserType.Admin}) seed result: {identityResult}");
                }
                if (!await _roleManager.Roles.AnyAsync(x => x.Name == Constants.UserType.Employee))
                {
                    identityResult = await _roleManager.CreateAsync(new Role<int>
                    { Id = 2, Name = Constants.UserType.Employee, DisplayName = "Employee" });
                    _logger.LogInformation($"Role ({Constants.UserType.Employee}) seed result: {identityResult}");
                }
                if (!await _roleManager.Roles.AnyAsync(x => x.Name == Constants.UserType.HRManager))
                {
                    identityResult = await _roleManager.CreateAsync(new Role<int>
                    { Id = 3, Name = Constants.UserType.HRManager, DisplayName = "HR Manager" });
                    _logger.LogInformation($"Role ({Constants.UserType.HRManager}) seed result: {identityResult}");
                }
                if (!await _roleManager.Roles.AnyAsync(x => x.Name == Constants.UserType.HRExecutive))
                {
                    identityResult = await _roleManager.CreateAsync(new Role<int>
                    { Id = 4, Name = Constants.UserType.HRExecutive, DisplayName = "HR Executive" });
                    _logger.LogInformation($"Role ({Constants.UserType.HRExecutive}) seed result: {identityResult}");
                }
                if (!await _roleManager.Roles.AnyAsync(x => x.Name == Constants.UserType.ReportingManager))
                {
                    identityResult = await _roleManager.CreateAsync(new Role<int>
                    { Id = 5, Name = Constants.UserType.ReportingManager, DisplayName = "Reporting Manager" });
                    _logger.LogInformation($"Role ({Constants.UserType.ReportingManager}) seed result: {identityResult}");
                }

                if (!await _roleManager.Roles.AnyAsync(x => x.Name == Constants.UserType.Interviewer))
                {
                    identityResult = await _roleManager.CreateAsync(new Role<int>
                    { Id = 6, Name = Constants.UserType.Interviewer, DisplayName = "Interviewer" });
                    _logger.LogInformation($"Role ({Constants.UserType.Interviewer}) seed result: {identityResult}");
                }

                _logger.LogInformation("Role seeding done.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Role seeding");
            }
        }

        public async Task SeedAdminAsync()
        {
            try
            {
                if (!await _roleManager.Roles.AnyAsync())
                {
                    _logger.LogError("Administrator seed failed, no role found");
                }

                var user = await _userManager.FindByNameAsync("anupam@logimonk.com");

                if (user != null)
                {
                    return;
                }

                user = new User
                {
                    FirstName = "Anupam",
                    LastName = "Shukla",
                    UserName = "anupam@logimonk.com",
                    Email = "anupam@logimonk.com",
                    EmailConfirmed = true,
                    Status = Constants.RecordStatus.Active,
                    CreatedOn = Utility.GetDateTime(),
                };

                var result = await _userManager.CreateAsync(user, "Password@123");

                if (!result.Succeeded)
                {
                    _logger.LogError("Administrator seed failed");
                    _logger.LogError(string.Join(",", result.Errors.Select(x => x.Description)));
                    return;
                }

                result = await _userManager.AddToRoleAsync(user, Constants.UserType.Admin);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Administrator seed completed");
                    return;
                }

                _logger.LogError("Administrator seed failed");
                _logger.LogError(string.Join(",", result.Errors.Select(x => x.Description)));

                await _userManager.DeleteAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in administrator seeding {ex}");
            }

        }

        public async Task SeedCountryAsync()
        {
            try
            {
                if (await _dataContext.Countries.AnyAsync(x => x.Name == "India"))
                {
                    return;
                }

                var country = new Country
                {
                    Name = "India",
                    Status = Constants.RecordStatus.Active,
                    States = new List<State>
                {
                    new State {
                        Name = "Madhya Pradesh",
                        Status = Constants.RecordStatus.Active,
                        Cities = new List<City>
                        {
                            new City {Name = "Jabalpur"}
                        }
                    }
                }
                };

                await _dataContext.AddAsync(country);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in country seeding {ex}");
            }
        }

        public async Task SeedCompanyAsync()
        {
            try
            {
                if (await _dataContext.Companies.AnyAsync(x => x.Email == "contact@logimonk.com"))
                {
                    return;
                }

                var user = await _dataContext.Users.SingleOrDefaultAsync(x => x.UserName == "anupam@logimonk.com");

                if (user == null)
                {
                    return;
                }

                var company = new Company
                {
                    UserId = user.Id,
                    RegisteredName = "Logimonk Technologies Pvt. Ltd.",
                    BrandName = "LOGIMONK",
                    WebsiteUrl = "https://www.logimonk.com",
                    Email = "contact@logimonk.com",
                    Phone = "8800452327",
                    CreatedOn = Utility.GetDateTime(),
                    EffectiveFrom = Utility.GetDateTime(),
                    CreatedById = 1,
                };

                await _dataContext.AddAsync(company);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in company seeding {ex}");
            }
        }

        public async Task SeedRelationshipAsync()
        {
            try
            {
                if (!await _dataContext.Relationships.AnyAsync())
                {
                    var relationships = new List<Relationship>
                    {
                        new Relationship
                        {
                            Name = "Father",
                            Status = Constants.RecordStatus.Active
                        },
                        new Relationship
                        {
                            Name = "Mother",
                            Status = Constants.RecordStatus.Active
                        },
                        new Relationship
                        {
                            Name = "Husband",
                            Status = Constants.RecordStatus.Active
                        },
                        new Relationship
                        {
                            Name = "Wife",
                            Status = Constants.RecordStatus.Active
                        },
                        new Relationship
                        {
                            Name = "Son",
                            Status = Constants.RecordStatus.Active
                        },
                        new Relationship
                        {
                            Name = "Daughter",
                            Status = Constants.RecordStatus.Active
                        },
                        new Relationship
                        {
                            Name = "Brother",
                            Status = Constants.RecordStatus.Active
                        },
                        new Relationship
                        {
                            Name = "Sister",
                            Status = Constants.RecordStatus.Active
                        },
                    };
                    await _dataContext.AddRangeAsync(relationships);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in relationship seeding {ex}");
            }
        }

        public async Task SeedQualificationTypeAsync()
        {
            try
            {
                if (!await _dataContext.QualificationTypes.AnyAsync())
                {
                    var qualificationTypes = new List<QualificationType>
                    {
                        new QualificationType
                        {
                            Name ="Certification",
                            Status = Constants.RecordStatus.Active
                        },
                        new QualificationType
                        {
                            Name ="Diploma",
                            Status = Constants.RecordStatus.Active
                        },
                        new QualificationType
                         {
                            Name ="Doctorate",
                            Status = Constants.RecordStatus.Active
                         },
                        new QualificationType
                        {
                            Name ="Graduation",
                            Status = Constants.RecordStatus.Active
                        },
                        new QualificationType
                        {
                            Name ="Other Education",
                            Status = Constants.RecordStatus.Active
                        },
                        new QualificationType
                        {
                            Name ="Post Graduation",
                            Status = Constants.RecordStatus.Active
                        },
                        new QualificationType
                        {
                            Name ="Pre University",
                            Status = Constants.RecordStatus.Active
                        },
                    };
                    await _dataContext.AddRangeAsync(qualificationTypes);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in qualificationtypes seeding {ex}");
            }
        }

        public async Task SeedCourseTypeAsync()
        {
            try
            {
                if (!await _dataContext.CourseTypes.AnyAsync())
                {
                    var courseTypes = new List<CourseType>
                    {
                        new CourseType
                        {
                            Name = "Full Time",
                            Status = Constants.RecordStatus.Active
                        },
                         new CourseType
                         {
                            Name = "Part Time",
                            Status = Constants.RecordStatus.Active
                         },
                         new CourseType
                         {
                            Name = "Correspondence",
                            Status = Constants.RecordStatus.Active
                         },
                         new CourseType
                         {
                            Name = "Certificate",
                            Status = Constants.RecordStatus.Active
                         },
                    };
                    await _dataContext.AddRangeAsync(courseTypes);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in course types seeding {ex}");
            }
        }

        public async Task SeedAssetTypeAsync()
        {
            try
            {
                if (await _dataContext.AssetTypes.AnyAsync(x => x.Name == "Laptop"))
                {
                    return;
                }
                var AssetType = new AssetType
                {
                    Name = "Laptop",
                    Manufacturers = new List<Manufacturer>
                    {
                        new Manufacturer
                        {
                        Name = "Dell",
                        Description = "Dell Inc. is an American based technology company",
                        Status = Constants.RecordStatus.Active,
                        Variants = new List<Variant>
                        {
                        new Variant
                        {
                            Name = "Inspiron",
                            Status = Constants.RecordStatus.Active,

                        },
                         new Variant
                        {
                            Name = "Vostro",
                            Status = Constants.RecordStatus.Active
                        },
                         new Variant
                        {
                            Name = "Latitude",
                            Status = Constants.RecordStatus.Active
                         }

                        }
                        },
                         new Manufacturer
                        {
                        Name = "HP",
                        Description = "HP Inc. is an American based technology company",
                        Status = Constants.RecordStatus.Active,
                        Variants = new List<Variant>
                        {
                        new Variant
                        {
                            Name = "Pavilion",
                            Status = Constants.RecordStatus.Active,

                        },
                         new Variant
                        {
                            Name = "Envy",
                            Status = Constants.RecordStatus.Active
                        },
                         new Variant
                        {
                            Name = "Spectre",
                            Status = Constants.RecordStatus.Active
                         }

                        }
                        }

                    }
                };

                await _dataContext.AddRangeAsync(AssetType);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in asset Type seeding {ex}");
            }
        }

        public async Task SeedSkillsAsync()
        {
            try
            {
                if (!await _dataContext.Skills.AnyAsync())
                {
                    var skills = new List<Skill>
                    {
                        new Skill
                        {
                            Name = "Java",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "Python",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "C/C++",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "Data structures",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "Algorithms",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "JavaScript",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "Android (Java/Kotlin)",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "HTML5",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "CSS3",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "React",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "Angular",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "Bootstrap",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "Node.js",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "Express.js",
                            Status = Constants.RecordStatus.Active
                        }, 
                        new Skill
                        {
                            Name = ".NET",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "Asp.NET",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "Asp.NET Core",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "UI/UX",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "Spring/Spring-Boot",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "Microservices",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "Blazor",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "Artificial Intelligence",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "Machine Learning",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "Big Data (Hadoop, Spark)",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "Flutter (Android/iOS)",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "MySQL",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "PostgreSQL",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "Oracle",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "SQL Server",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "MongoDB",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "Amazon Web Services (AWS)",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "Microsoft Azure",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "Google Cloud Platform (GCP)",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "Docker",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "Kubernetes",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "Jenkins",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "Git",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "Continuous Integration/Continuous Deployment (CI/CD)",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "iOS (Swift/Objective-C)",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "Manual Testing",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "Test Automation (Selenium) ",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "Test Automation (JUnit) ",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "Test Automation (TestNG) ",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "Network Security",
                            Status = Constants.RecordStatus.Active
                        }, 
                        new Skill
                        {
                            Name = "Communication",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "Presentation",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "Customer Service",
                            Status = Constants.RecordStatus.Active
                        },
                        new Skill
                        {
                            Name = "MS Office",
                            Status = Constants.RecordStatus.Active
                        },
                       
                    };
                    await _dataContext.AddRangeAsync(skills);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in skill seeding {ex}");
            }
        }
    }
}
