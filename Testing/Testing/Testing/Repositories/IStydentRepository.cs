using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Testing.Models;

namespace Testing.Repositories
{
    public interface IStydentRepository
    {
        Task<List<Student>> GetStudents();
        Task<Student> GetStudentAsync(Guid studentId);
        Task<List<Gender>> GetGendersAsync();
        Task<bool> Exists(Guid StudentId);
        Task<Student> UpdateStudent(Guid studentId, Student request);

        Task<Student> DeleteStudent(Guid studentId);

        Task<Student> AddStudent(Student request);

        Task<bool> UpdateProfileImage(Guid studentId, string profileIamgeUrl);


    }
}
