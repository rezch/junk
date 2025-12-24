using GraphQL.Types;

namespace lab5.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class StudentType : ObjectGraphType<Student>
    {
        public StudentType()
        {
            Name = "Student";
            Field(x => x.Id).Description("ID студента");
            Field(x => x.Name).Description("Имя студента");
            Field(x => x.Age).Description("Возраст студента");
        }
    }
}