using Data.Services.Data;
using Data.Services.Intefaces;
using Entities.DbSet;
using Entities.Dto.CourseDto;
using Entities.Dto.CourseDto.OutComing;
using Entities.Dto.ExpermintDto;
using Entities.Dto.UserDto;
using LLS.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LLS.Controllers
{
    public class CourseController : BaseController
    {
        public CourseController(IUnitOfWork unitOfWork,
                                UserManager<IdentityUser> userManager,
                                AppDbContext context)
            : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("Create-Course")]
        public async Task<IActionResult> CreateCourse(CourseDto courseDto)
        {
            var allCourses = await _unitOfWork.CourseRepository.GetAllCourses();
            foreach(var C in allCourses)
            {
                if (C.Name == courseDto.Name)
                    return BadRequest(new Result()
                    {
                        Status = false,
                        Error = "The Course IDD already in use"
                    });
            }
            var course = new Course()
            {
                Name = courseDto.Name,
                CourseIDD = courseDto.IDD,
                StartDate = courseDto.StartDate,
                EndDate = courseDto.EndDate
            };

            var result = await _unitOfWork.CourseRepository.CtreateCourse(course);
            await _unitOfWork.SaveAsync();

            return Ok(new Result()
            {
                Status = true,
                Data = "Added Successfully"
            });
        }



        [HttpGet("Get-List-Of-Courses")]
        public async Task<IActionResult> GetListOfCourses()
        {
            var CourseList = await _unitOfWork.CourseRepository.GetAllCourses();
            if(!CourseList.Any())
            {
                return BadRequest(new Result()
                {
                    Status = false,
                    Error = "There is no Courses"
                });
            }

            var CourseListDto = new List<CourseOutDto>();
            foreach(var x in CourseList)
            {
                var course = new CourseOutDto()
                {
                    CourseName = x.Name,
                    CourseIDD = x.CourseIDD,
                    NOExpirments = x.NumberOfExp,
                    NOStudents = x.NumberOfStudents,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate
                };
                CourseListDto.Add(course);
            }

            return Ok(new Result()
            {
                Status = true,
                Data = CourseListDto
            });
        }

        [HttpGet("Get-Course-By-Name")]
        public async Task<IActionResult> GetCourseByName(string courseName)
        {
            var course = await _unitOfWork.CourseRepository.GetCourseByName(courseName);
            if(course == null)
            {
                return BadRequest(new Result()
                {
                    Status = false,
                    Error = "There is no Course with this name"
                });
            }

            var courseDto = new CourseOutDto()
            {
                CourseName = course.Name,
                CourseIDD = course.CourseIDD,
                NOExpirments = course.NumberOfExp,
                NOStudents = course.NumberOfStudents,
                StartDate = course.StartDate,
                EndDate = course.EndDate
            };

            return Ok(new Result()
            {
                Status = true,
                Data = courseDto
            });
        }



        [HttpGet("Get-Assigned-Expeirments")]
        public async Task<IActionResult> GetExpAssignedToCourse(string courseName)
        {
            var course = await _unitOfWork.CourseRepository.GetCourseByName(courseName);
            if (course == null)
            {
                return BadRequest(new Result()
                {
                    Status = false,
                    Error = "There is no Course with this name"
                });
            }

            var expList = await _unitOfWork.GetBy.GetFullExpByCourse(course.Id);
            if (!expList.Any())
            {
                return BadRequest(new Result()
                {
                    Status = false,
                    Error = "There is no Expirment for this Course"
                });
            }

            var listDto = new List<ExperimentByCourse>();
            foreach(var x in expList)
            {
                var dto = new ExperimentByCourse()
                {
                    ExperimentName = x.Name
                };
                listDto.Add(dto);
            }

            return Ok(new Result()
            {
                Status = true,
                Data = listDto
            });
        }

        [HttpGet("Get-Assigned-Students")]
        public async Task<IActionResult> GetStudentAssignedToCourse(string courseName)
        {
            var course = await _unitOfWork.CourseRepository.GetCourseByName(courseName);
            if (course == null)
            {
                return BadRequest(new Result()
                {
                    Status = false,
                    Error = "There is no Course with this name"
                });
            }

            var studnetList = await _unitOfWork.GetBy.GetFullStudentByCourse(course.Id);
            if (!studnetList.Any())
            {
                return BadRequest(new Result()
                {
                    Status = false,
                    Error = "There is no Students for this Course"
                });
            }

            var listDto = new List<StudentByCourse>();
            foreach (var x in studnetList)
            {
                var dto = new StudentByCourse()
                {
                    StudentName = x.Email,
                    AcademicYear = x.AcademicYear
                };
                listDto.Add(dto);
            }

            return Ok(new Result()
            {
                Status = true,
                Data = listDto
            });
        }

        [HttpGet("Get-Assigned-Teachers")]
        public async Task<IActionResult> GetTeachersAssignedToCourse(string courseName)
        {
            var course = await _unitOfWork.CourseRepository.GetCourseByName(courseName);
            if (course == null)
            {
                return BadRequest(new Result()
                {
                    Status = false,
                    Error = "There is no Course with this name"
                });
            }

            var studnetList = await _unitOfWork.GetBy.GetFullTeacherByCourse(course.Id);
            if (!studnetList.Any())
            {
                return BadRequest(new Result()
                {
                    Status = false,
                    Error = "There is no Teachers for this Course"
                });
            }

            var listDto = new List<StudentByCourse>();
            foreach (var x in studnetList)
            {
                var dto = new StudentByCourse()
                {
                    StudentName = x.Email,
                    AcademicYear = x.AcademicYear
                };
                listDto.Add(dto);
            }

            return Ok(new Result()
            {
                Status = true,
                Data = listDto
            });
        }



        [HttpDelete("Delete-Course")]
        public async Task<IActionResult> DeleteCourse(string courseName)
        {
            var course = await _unitOfWork.CourseRepository.GetCourseByName(courseName);

            if (course == null)
                return BadRequest(new Result()
                {
                    Status = false,
                    Error = "Course name doesn't exists"
                });

            var result = await _unitOfWork.CourseRepository.Delete(course.Id);
            await _unitOfWork.SaveAsync();

            return Ok(new Result()
            {
                Status = true,
                Data = "Deleted Successfully"
            });
        }



        [HttpPost("Assign-Teacher")]
        public async Task<IActionResult> AssignTeacherToCourse(string teacherName, string courseName)
        {
            var teacher = await _unitOfWork.Users.GetUserByEmail(teacherName);
            if(teacher == null)
                return BadRequest(new Result()
                {
                    Status = false,
                    Error = "User doesn't exists"
                });

            if(teacher.Role.ToLower() != "teacher")
                return BadRequest(new Result()
                {
                    Status = false,
                    Error = "User must have the Role Teacher to be Assigned as A teacher"
                });

            var course = await _unitOfWork.CourseRepository.GetCourseByName(courseName);
            if (course == null)
                return BadRequest(new Result()
                {
                    Status = false,
                    Error = "Course doesn't exists"
                });

            var listOfTeacher = await _unitOfWork.GetBy.GetTeacherByCourse(course.Id);
            foreach (var x in listOfTeacher)
            {
                if (x == teacher.Id)
                {
                    return BadRequest(new Result()
                    {
                        Status = false,
                        Error = "This Teacher is already assigned to the course"
                    });
                }
            }

            var result = await _unitOfWork.CourseRepository.AssignTeacherToCourse(teacher.Id, course.Id);
            if (!result)
            {
                return BadRequest(new Result()
                {
                    Status = false,
                    Error = "Something went wrong"
                });
            }
            await _unitOfWork.SaveAsync();

            return Ok(new Result()
            {
                Status = true,
                Data = "Teacher has been added to the Course Successfully"
            });
        }

        //[HttpDelete("Remove-Teacher")]
        //public async Task<IActionResult> RemoveTeacherToCourse(string teacherName, string courseName)
        //{
        //    var teacher = await _unitOfWork.Users.GetUserByEmail(teacherName);
        //    if (teacher == null)
        //        return BadRequest(new Result()
        //        {
        //            Status = false,
        //            Error = "User doesn't exists"
        //        });

        //    if (teacher.Role.ToLower() != "teacher")
        //        return BadRequest(new Result()
        //        {
        //            Status = false,
        //            Error = "User must have the Role Teacher to be Assigned as A teacher"
        //        });

        //    var course = await _unitOfWork.CourseRepository.GetCourseByName(courseName);
        //    if (course == null)
        //        return BadRequest(new Result()
        //        {
        //            Status = false,
        //            Error = "Course doesn't exists"
        //        });

        //    var result = await _unitOfWork.CourseRepository.AssignTeacherToCourse(teacher.Id, course.Id);
        //    await _unitOfWork.SaveAsync();

        //    return Ok(new Result()
        //    {
        //        Status = true,
        //        Data = "Teacher has been added to the Course Successfully"
        //    });
        //}

        [HttpPost("Assign-Student")]
        public async Task<IActionResult> AssignStudentToCourse(string studentName, string courseName)
        {
            var student = await _unitOfWork.Users.GetUserByEmail(studentName);
            if (student == null)
                return BadRequest(new Result()
                {
                    Status = false,
                    Error = "User doesn't exists"
                });

            if (student.Role.ToLower() != "student")
                return BadRequest(new Result()
                {
                    Status = false,
                    Error = "User must have the Role Student to be Assigned as A student"
                });

            var course = await _unitOfWork.CourseRepository.GetCourseByName(courseName);
            if (course == null)
                return BadRequest(new Result()
                {
                    Status = false,
                    Error = "Course doesn't exists"
                });

            var listOfStudent = await _unitOfWork.GetBy.GetStudentsByCourse(course.Id);
            foreach (var x in listOfStudent)
            {
                if (x == student.Id)
                {
                    return BadRequest(new Result()
                    {
                        Status = false,
                        Error = "This Student is already assigned to the course"
                    });
                }
            }

            var result = await _unitOfWork.CourseRepository.AssignStudentToCourse(student.Id, course.Id);
            if(!result)
            {
                return BadRequest(new Result()
                {
                    Status = false,
                    Error = "Something went wrong"
                });
            }

            await _unitOfWork.CourseRepository.AssignStudentToExpCourse(student.Id, course.Id);
            

            await _unitOfWork.SaveAsync();

            return Ok(new Result()
            {
                Status = true,
                Data = "Student has been added to the Course Successfully"
            });
        }

        [HttpPost("Assign-Exp")]
        public async Task<IActionResult> AssignExpToCourse(string expName, string courseName)
        {
            var exp = await _unitOfWork.ExpirmentRepository.GetExpByName(expName);
            if (exp == null)
                return BadRequest(new Result()
                {
                    Status = false,
                    Error = "Expirment doesn't exists or it already "
                });

            var course = await _unitOfWork.CourseRepository.GetCourseByName(courseName);
            if (course == null)
                return BadRequest(new Result()
                {
                    Status = false,
                    Error = "Course doesn't exists"
                });


            var listOfExp = await _unitOfWork.GetBy.GetExpByCourse(course.Id);
            foreach(var x in listOfExp)
            {
                if(x == exp.Id)
                {
                    return BadRequest(new Result()
                    {
                        Status = false,
                        Error = "This Expirment is already assigned to the course"
                    });
                }
            }


            var result = await _unitOfWork.CourseRepository.AssignExpToCourse(exp.Id, course.Id);
            if (!result)
            {
                return BadRequest(new Result()
                {
                    Status = false,
                    Error = "Something went wrong"
                });
            }

            course.NumberOfExp++;

            await _unitOfWork.SaveAsync();

            return Ok(new Result()
            {
                Status = true,
                Data = "Experiment has been added to the Course Successfully"
            });
        }

        
    }
}
