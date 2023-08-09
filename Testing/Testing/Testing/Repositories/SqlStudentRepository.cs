using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Testing.Models;

namespace Testing.Repositories
{
    public class SqlStudentRepository : IStydentRepository
    {
        private readonly AppDbContext context;
        public SqlStudentRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Student> AddStudent(Student request)
        {
            var student= await context.Student.AddAsync(request);
            await context.SaveChangesAsync();
            return student.Entity;
        }

        public async Task<Student> DeleteStudent(Guid studentId)
        {
            var student = await GetStudentAsync(studentId);
            if (student != null)
            {
                context.Student.Remove(student);
               await context.SaveChangesAsync();
            }
            return null;
        }

        public async Task<bool> Exists(Guid StudentId)
        {
           return await context.Student.AnyAsync(x => x.Id == StudentId);
        }

        public async Task<List<Gender>> GetGendersAsync()
        {
            return await context.Gender.ToListAsync();
        }

        public async Task<Student> GetStudentAsync(Guid studentId)
        {
            return await context.Student.Include(nameof(Gender)).Include(nameof(Address)).FirstOrDefaultAsync(x=>x.Id==studentId);
        }

        public async Task<List<Student>> GetStudents()
        {
           return await context.Student.Include(nameof(Gender)).Include(nameof(Address)).ToListAsync();
        }

        public async Task<bool> UpdateProfileImage(Guid studentId, string profileIamgeUrl)
        {
            var student = await GetStudentAsync(studentId);
            if(student != null)
            {
                student.ProfileImageUrl = profileIamgeUrl;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Student> UpdateStudent(Guid studentId, Student request)
        {
            var existingStudent = await GetStudentAsync(studentId);
            if(existingStudent != null)
            {
                existingStudent.FirstName = request.FirstName;
                existingStudent.LastName = request.LastName;
                existingStudent.DateOfBirth = request.DateOfBirth;
                existingStudent.Email = request.Email;
                existingStudent.Mobile = request.Mobile;
                existingStudent.GenderId = request.GenderId;
                existingStudent.Address.PhysicalAddress = request.Address.PhysicalAddress;
                existingStudent.Address.PostalAddress = request.Address.PostalAddress;

                await context.SaveChangesAsync();
                return existingStudent;
            }
            return null;
        }
    }
}
