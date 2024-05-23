using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Repositories
{
    public interface IStudentRepository
    {
        void AddStudent(Student student);
        Student GetStudent(int studentID);
        void UpdateStudent(Student student);
        void DeleteStudent(int studentID);
        List<Student> GetAllStudents();
        void AddSampleStudents();
    }
}