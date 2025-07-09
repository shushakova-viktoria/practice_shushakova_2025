using System.Text.Json.Serialization;
using System.Text.Json;
using task13;
using static Program.Validating;

namespace task13tests
{
    public class task13tests
    {
        JsonSerializerOptions _options = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            WriteIndented = true

        };

        [Fact]
        public void ContainingInformationInSerializingCorrectly()
        {
            var student = new Student
            {
                FirstName = "Vika",
                LastName = "Shushakova",
                BirthDate = new DateTime(2006, 1, 10),
                Grades = null
            };

            string jsonSerialize = JsonSerializer.Serialize(student, _options);

            Assert.Contains("FirstName", jsonSerialize);
            Assert.DoesNotContain("Grades", jsonSerialize);
        }

        [Fact]
        public void ContainingInformationInDeserializingCorrectly()
        {
            var testStudent = new Student
            {
                FirstName = "Sasha",
                LastName = "Miroshnikova",
                BirthDate = new DateTime(2006, 5, 9),
                Grades = new List<Subject> 
                {
                new Subject { Name = "Geometry", Grade = 5 }
                }
        
            };

            string jsonDesirialize = JsonSerializer.Serialize(testStudent, _options);

            var student = JsonSerializer.Deserialize<Student>(jsonDesirialize, _options);

            Assert.Equal(testStudent.FirstName, student.FirstName);
            Assert.Equal(testStudent.LastName, student.LastName);
            Assert.Equal(testStudent.BirthDate, student.BirthDate);
            Assert.Single(student.Grades);
        }

        [Fact]
        public void FalseValidating()
        {
            var student = new Student
            {
                FirstName = null,
                LastName = "",
                BirthDate = default,
                Grades = null
            };

            bool desirialization = ValidatingInDesirialization(student);

            Assert.False(desirialization);
        }

        [Fact]
        public void TrueValidating()
        {
            var student = new Student
            {
                FirstName = "Ksusha",
                LastName = "Firstova",
                BirthDate = new DateTime(2006, 7, 1),
                Grades = new List<Subject>
                {
                new Subject { Name = "Algebra", Grade = 5 }
                }
            };

            bool desirialization = ValidatingInDesirialization(student);

            Assert.True(desirialization);
        }
    }
    
}