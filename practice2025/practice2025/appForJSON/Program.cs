using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using task13;

namespace Program
{
    public static class Validating
    {
        public static bool ValidatingInDesirialization(Student student)
        {
            if (string.IsNullOrWhiteSpace(student.FirstName))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(student.LastName))
            {
                return false;
            }

            if (student.BirthDate == default)
            {
                return false;
            }

            if (student.Grades == null)
            {
                return false;
            }

            return true;
        }
    }
    public static class Json
    {
        public static void Main(string[] args)
        {

            JsonSerializerOptions options = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
                WriteIndented = true

            };

            var student = new Student
            {
                FirstName = "Vika",
                LastName = "Shushakova",
                BirthDate = new DateTime(2006, 1, 10),
                Grades = new List<Subject>
                {
                    new Subject { Name = "Mathematical_analysis", Grade = 4 },
                    new Subject { Name = "Discrete_mathematics", Grade = 5 },
                    new Subject { Name = "Algoritmization", Grade = 5 },
                }
            };

            string studentJson = JsonSerializer.Serialize(student, options);
            Console.WriteLine($"Выполнена сериализация: {studentJson}");

            File.WriteAllText("student.json", studentJson);
            string fileJson = File.ReadAllText("student.json");

            Student deserializedStudent = JsonSerializer.Deserialize<Student>(fileJson, options);
            var deserializedFirstName = deserializedStudent.FirstName;
            var deserializedLastName = deserializedStudent.LastName;
            var deserializedBirthDate = deserializedStudent.BirthDate;

            Console.WriteLine($"Выполнена десериализация. Имя: {deserializedFirstName}. Фамилия: {deserializedLastName}. " +
                $"Дата: {deserializedBirthDate}.");
            foreach (var subject in deserializedStudent.Grades)
            {
                Console.WriteLine($"Оценки за предметы: {subject.Name} - {subject.Grade}");
            }
        }
    }
}
        