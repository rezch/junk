using GraphQL;
using GraphQL.Types;
using lab5.Database;
using lab5.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace lab5.Queries
{
    public class StudentQuery : ObjectGraphType
    {
        public StudentQuery(SchoolContext db)
        {
            // Получение всех студентов
            Field<ListGraphType<StudentType>>(
                "students",
                resolve: context => db.Students.ToList()
            );

            // Получение студента по ID
            Field<StudentType>(
                "student",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }
                ),
                resolve: context =>
                {
                    var id = context.GetArgument<int>("id");
                    return db.Students.Find(id);
                }
            );
        }
    }
}