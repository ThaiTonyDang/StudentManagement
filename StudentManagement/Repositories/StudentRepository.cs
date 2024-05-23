using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private static StudentRepository _instance;
        private static int _currentId = 1; // biến ĩnh giữ ID hiện tại của Sinh viên 
        private List<Student> _students = new List<Student>();
        public StudentRepository() { }

        public void AddStudent(Student student)
        {
            student.StudentID = _currentId++;  // Tự động tăng ID của Sinh viên đỡ nhập tay
            _students.Add(student);
        }

        public void DeleteStudent(int studentID)
        {
            var student = GetStudent(studentID);
            if (student != null)
            {
                _students.Remove(student);
            }
        }

        public List<Student> GetAllStudents()
        {
            return _students;
        }

        public Student GetStudent(int studentID)
        {
            return _students.FirstOrDefault(s => s.StudentID == studentID);
        }

        public void UpdateStudent(Student student)
        {
            var existringStudent = GetStudent(student.StudentID);
            if (existringStudent != null)
            {
                existringStudent.Name = student.Name;
                existringStudent.Age = student.Age;
                existringStudent.GPA = student.GPA;
            }
        }

        public void AddSampleStudents() // Add Student dữ liệu mẫu
        {
            //_students.Add(new Student { Name = "Nguyen Van A", Age = 20, GPA = 1.5 });
            //_students.Add(new Student { Name = "Le Thi B", Age = 21, GPA = 3.7 });
            //_students.Add(new Student { Name = "Tran Van C", Age = 22, GPA = 3.8 });
            //_students.Add(new Student { Name = "Pham Thi D", Age = 23, GPA = 4.0 });
            //_students.Add(new Student { Name = "Hoang Van E", Age = 20, GPA = 3.4 });
            //_students.Add(new Student { Name = "Do Thi F", Age = 21, GPA = 3.9 });
            //_students.Add(new Student { Name = "Bui Van G", Age = 22, GPA = 2.3 });
            //_students.Add(new Student { Name = "Nguyen Thi H", Age = 23, GPA = 3.5 });
            //_students.Add(new Student { Name = "Le Van I", Age = 20, GPA = 3.2 });
            //_students.Add(new Student { Name = "Tran Thi K", Age = 21, GPA = 2.6 });        
            List<Student> sampleStudents = new List<Student>
            {
                new Student { Name = "Nguyen Van A", Age = 20, GPA = 3.5 },
                new Student { Name = "Le Thi B", Age = 21, GPA = 3.7 },
                new Student { Name = "Tran Van C", Age = 22, GPA = 3.8 },
                new Student { Name = "Pham Thi D", Age = 23, GPA = 3.6 },
                new Student { Name = "Hoang Van E", Age = 20, GPA = 3.4 },
                new Student { Name = "Do Thi F", Age = 21, GPA = 3.9 },
                new Student { Name = "Bui Van G", Age = 22, GPA = 3.3 },
                new Student { Name = "Nguyen Thi H", Age = 23, GPA = 3.5 },
                new Student { Name = "Le Van I", Age = 20, GPA = 3.2 },
                new Student { Name = "Tran Thi K", Age = 21, GPA = 3.6 }
            };

            foreach (var student in sampleStudents)
            {
                AddStudent(student);
            }
        }
    }
}
