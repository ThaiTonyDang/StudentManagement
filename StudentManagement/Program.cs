using OfficeOpenXml;
using StudentManagement.Models;
using StudentManagement.Repositories;
using System;
using System.IO;

namespace StudentManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // in ra Tiếng việt
            // trình tự logic 

            // B1 . Gọi các hàm thực thi thông qua Interface Student bằng cách tạo mới new StudentRepository.
            // Không tạo không sử dụng được dịch vụ thêm sửa xóa, vv. Ví dụ gọi dịch vụ Thêm vào danh sách sinh viên mẫu
            IStudentRepository studentRepository = new StudentRepository(); // đã có dịch vụ 
            studentRepository.AddSampleStudents(); // Gọi dịch vụ  Load dữ liệu mẫu.

            //B2 . Sau khi có dịch vụ thiết kế màn hình chinh switch case  để thao tác. gồm có vòng lặp while để quay về màn hình nếu cần
            while (true)
            {
                // B3 Thiết kế màn hình Menu để chọn phím . Thiết kế hàm ShowMenu để chọn 
                ShowMainMenu();

                // B4 Viêt lệnh chọn số từ bàn phím, phải có điều kiện kiểm tra nhập là số hay không, 
                int choice;
                Console.Write("Lựa chọn của bạn: ");
                while (!int.TryParse(Console.ReadLine(), out choice) || !IsValidChoice(choice))
                {
                    Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng nhập số tương ứng trên menu.");
                }

                // B5 Switch case lựa chọn 
                switch (choice)
                {
                    case 1:
                        // B6 : Làm đúng theo menu đã thiết kế. Phim 1 là Show ra danh sách sinh viên. Gọi hàm tương ứng trong dịch vụ ở bước 1. Viết ra hàm riêng dễ nhìn
                        ShowAllStudents(studentRepository);
                        break;
                    case 2:
                        // B7 : Tương tự viết hàm Add Student . Hàm này nhập thông tin từ bàn phím 
                        AddStudent(studentRepository);
                        break;
                    case 3:
                        // B8 : Hàm này yêu cầu Update sinh viên theo Id 
                        UpdateStudent(studentRepository);
                        break;
                    case 4:
                        // B9 : Hàm này yêu cầu Xóa sinh viên theo Id 
                        DeleteStudent(studentRepository);
                        break;
                    case 5:
                        //B10 : Export thông tin ra file Excel( bước này không làm cũng đc ). Kích chuột phải vào Solution chọn manage Nuget cài EPPlus 
                        ExportInfo(studentRepository);
                        break;
                    case 0:
                        if (ConfirmExit())
                        {
                            return; // nếu chọn Y là đúng thì Thoát
                        }
                        break;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng thử lại.");
                        break;
                }
            }
        }

        // Bên dưới này là viết hàm //
        //---------------------//
        // bước 3 . Thiết kế ShowMenu
        static void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("Để thao tác mời nhập số tương ứng từ bàn phím ");
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine();
            Console.WriteLine("Phím 1 : Xem danh sách sinh viên có sẵn");
            Console.WriteLine("Phím 2 : Thêm vào sinh viên");
            Console.WriteLine("Phím 3 : Tìm Và cập nhật thông tin sinh viên");
            Console.WriteLine("Phím 4 : Tìm Và xóa thông tin sinh viên");
            Console.WriteLine("Phím 5 : Export thông tin sinh viên ra file Excel");
            Console.WriteLine("Phím 0 : Thoát");
            Console.Write("Lựa chọn của bạn: ");
        }

        // Bước 4 . hàm kiểm tra xem số được chọn nằm trong bảng menu không
        static bool IsValidChoice(int choice)
        {
            return choice == 0 || choice == 1 || choice == 2 || choice == 3 || choice == 4 || choice == 5; // trả về đúng khi mà nó nhập đúng số trên menu
        }

        // Bước 6 : Viêt Hàm Show All Student . Với dịch vụ từ bước 1
        static void ShowAllStudents(IStudentRepository studentRepository) // cho dịch vụ từ bước 1 vào làm tham số mới có thể lấy đc danh sách sinh viên
        {
            Console.Clear();
            var students = studentRepository.GetAllStudents(); // Lấy đc danh sách sinh viên
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("Danh sách sinh viên:");
            Console.WriteLine("-----------------------------------------------");
            foreach (var student in students)
            {
                Console.WriteLine($"ID: {student.StudentID}, Tên: {student.Name}, Tuổi: {student.Age}, GPA: {student.GPA}");
            }
            Console.WriteLine("-----------------------------------------------");

            // Viết thêm hàm quay về màn hình chính để thao tác tiếp
            ReturnToMainMenuPrompt();
        }

        //Hàm quay về màn hình chính
        static void ReturnToMainMenuPrompt()
        {
            Console.WriteLine("Bạn có muốn quay lại màn hình chính không? Y để quay lại (Y/N): ");
            string choice = Console.ReadLine();
            if (!choice.Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                Environment.Exit(0);
            }
        }

        // B7 : Nhập thông tin Student từ bàn phím và Add
        static void AddStudent(IStudentRepository studentRepository)
        {
            Console.Clear();
            Console.WriteLine("Thêm sinh viên mới:");
            Console.WriteLine("------------------");

            // Không cần nhập Id vì nó đã tự tăng .

            Console.Write("Tên: ");
            string name = Console.ReadLine();

            // Nhập tuổi phải kiểm tra xem có nhập hợp lệ không
            int age;
            while (true)
            {
                Console.Write("Tuổi: ");
                if (int.TryParse(Console.ReadLine(), out age) && age >= 18 && age <= 100) // đây là điều kiện kiểm tra
                {
                    break;
                }
                Console.WriteLine("Tuổi không hợp lệ. Vui lòng nhập tuổi trong khoảng từ 18 đến 100.");
            }

            Console.Write("GPA: ");
            double gpa = Convert.ToDouble(Console.ReadLine());

            studentRepository.AddStudent(new Student { Name = name, Age = age, GPA = gpa });

            Console.WriteLine("Sinh viên đã được thêm thành công.");
            ReturnToMainMenuPrompt();
        }

        //B8 : Update
        static void UpdateStudent(IStudentRepository studentRepository)
        {
            Console.Clear();
            Console.Write("Nhập ID của sinh viên cần cập nhật: ");
            int id;
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("ID không hợp lệ. Vui lòng nhập lại ID:");
            }

            var student = studentRepository.GetStudent(id);
            if (student == null)
            {
                Console.WriteLine("Không tìm thấy sinh viên với ID này.");
                ReturnToMainMenuPrompt();
                return;
            }

            Console.WriteLine($"Sinh viên tìm thấy: ID: {student.StudentID}, Tên: {student.Name}, Tuổi: {student.Age}, GPA: {student.GPA}");
            Console.WriteLine("Bạn có muốn cập nhật sinh viên này không? (Y/N): ");
            string choice = Console.ReadLine();
            if (!choice.Equals("Y", StringComparison.OrdinalIgnoreCase)) // nếu ko muốn cập nhật quay về màn hình chính
            {
                ReturnToMainMenuPrompt();
                return;
            }

            Console.Write("Tên mới: ");
            student.Name = Console.ReadLine();

            while (true)
            {
                Console.Write("Tuổi mới: ");
                if (int.TryParse(Console.ReadLine(), out int newAge) && newAge >= 18 && newAge <= 100)
                {
                    student.Age = newAge;
                    break;
                }
                Console.WriteLine("Tuổi không hợp lệ. Vui lòng nhập tuổi trong khoảng từ 18 đến 100.");
            }

            Console.Write("GPA mới: ");
            student.GPA = Convert.ToDouble(Console.ReadLine());

            studentRepository.UpdateStudent(student);

            Console.WriteLine("Sinh viên đã được cập nhật thành công.");
            ReturnToMainMenuPrompt();
        }

        // B9 : Xóa

        static void DeleteStudent(IStudentRepository studentRepository)
        {
            Console.Clear();
            Console.Write("Nhập ID của sinh viên cần xóa: ");
            int id;
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("ID không hợp lệ. Vui lòng nhập lại ID:");
            }

            var student = studentRepository.GetStudent(id);
            if (student == null)
            {
                Console.WriteLine("Không tìm thấy sinh viên với ID này.");
                ReturnToMainMenuPrompt();
                return;
            }

            Console.WriteLine($"Sinh viên tìm thấy: ID: {student.StudentID}, Tên: {student.Name}, Tuổi: {student.Age}, GPA: {student.GPA}");
            Console.WriteLine("Bạn có muốn xóa sinh viên này không? (Y/N): ");
            string choice = Console.ReadLine();
            if (!choice.Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                ReturnToMainMenuPrompt();
                return;
            }

            studentRepository.DeleteStudent(id);

            Console.WriteLine("Sinh viên đã được xóa thành công.");
            ReturnToMainMenuPrompt();
        }

        //B10 : Export 
        static void ExportInfo(IStudentRepository studentRepository)
        {
            var students = studentRepository.GetAllStudents();
            if (students.Count == 0)
            {
                Console.WriteLine("Không có sinh viên để xuất ra file Excel.");
                ReturnToMainMenuPrompt();
                return;
            }
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string folderPath = "C://NewStudent";
            // Kiểm tra và tạo thư mục nếu chưa tồn tại
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string fileName = Path.Combine(folderPath, $"ThongtinSinhvien_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");

            FileInfo file = new FileInfo(fileName);
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Thông tin sinh viên");

                // Tiêu đề cột
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Tên";
                worksheet.Cells[1, 3].Value = "Tuổi";
                worksheet.Cells[1, 4].Value = "GPA";

                // Dữ liệu sinh viên
                for (int i = 0; i < students.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = students[i].StudentID;
                    worksheet.Cells[i + 2, 2].Value = students[i].Name;
                    worksheet.Cells[i + 2, 3].Value = students[i].Age;
                    worksheet.Cells[i + 2, 4].Value = students[i].GPA;
                }

                package.Save();
            }

            Console.WriteLine($"Xuất thông tin sinh viên ra file Excel thành công: {fileName}");
            ReturnToMainMenuPrompt();
        }

        static bool ConfirmExit()
        {
            Console.WriteLine("Bạn có chắc chắn muốn thoát không? (Y/N): ");
            string choice = Console.ReadLine();
            return choice.Equals("Y", StringComparison.OrdinalIgnoreCase);
        }
    }
}
