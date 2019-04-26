using DynamicExpressionForStudents.Model;
using ExpressionLogic;
using ExpressionLogic.Model;
using ExpressionLogic.Model.FilteredProperty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DynamicExpressionForStudents
{
    class Program
    {
        static void Main(string[] args)
        {
            var listStudents = GetRandomListWithStudents();

            foreach (var student in listStudents)
            {
                Console.WriteLine(student);
            }

            Console.WriteLine("____________________");

            //var filtredList = listStudents.Where(student => student.Course < 2 && student.Faculty == "Faculty0");
            var builder = new FilteredPropertyBuilder(typeof(Student));
            var filteredFields = new List<IFilteredProperty>();
            filteredFields.Add(builder.Build(nameof(Student.Id)));
            filteredFields.Add(builder.Build(nameof(Student.Name)));
            filteredFields.Add(builder.Build(nameof(Student.Course)));
            filteredFields.Add(builder.Build(nameof(Student.Faculty)));
            filteredFields.Add(builder.Build(nameof(Student.DateOfBirth)));
            
            var dynamicFilter = new DynamicFilter<Student>();
            

            while (true)
            {
                var prediction = new Prediction();

                Console.WriteLine($"Choose field (index from 0 to {filteredFields.Count - 1}) or -1 to end: ");
                for (int i = 0; i < filteredFields.Count; i++)
                {
                    Console.WriteLine($"{i}) {filteredFields[i].FiledName}");
                }

                var fieldIndex = ReadFromConsole<int>();

                if (fieldIndex == -1)
                {
                    break;
                }

                var filteredField = filteredFields[fieldIndex];
                prediction.PropertyName = filteredField.FiledName;

                Console.WriteLine($"Choose field (index from 0 to {filteredField.AvalibleActions.Count - 1}): ");
                for (int i = 0; i < filteredField.AvalibleActions.Count; i++)
                {
                    Console.WriteLine($"{i}) {filteredField.AvalibleActions[i]}");
                }
                
                var indexOfAction = ReadFromConsole<int>();
                prediction.CompareAction = filteredField.AvalibleActions[indexOfAction];

                Console.WriteLine("Set right part:");
                var rightPart = ReadFromConsole(filteredField.FieldType);
                prediction.RightValue = rightPart;

                dynamicFilter.AddPredict(prediction);
            }

            Console.WriteLine();
            Console.WriteLine($"Lambda: {dynamicFilter.GetLambda()}");
            Console.WriteLine();

            var filtredList = dynamicFilter.Filter(listStudents);

            foreach (var student in filtredList)
            {
                Console.WriteLine(student);
            }
            Console.Read();
        }

        public static T ReadFromConsole<T>()
        {
            return (T)ReadFromConsole(typeof(T));
        }

            public static object ReadFromConsole(Type type)
        {
            var input = Console.ReadLine();
            object obj = null;
            try
            {
                obj = Convert.ChangeType(input, type);
            }
            catch (Exception)
            {

                throw;
            }
            
            while (obj == null)
            {
                try
                {
                    obj = Convert.ChangeType(input, type);
                }
                catch (Exception)
                {
                    Console.WriteLine($"It's not a {type.Name}. Pleas enter the {type.Name}: ");
                    input = Console.ReadLine();
                }
            }

            return obj;
        }

        public static List<Student> GetRandomListWithStudents()
        {
            var listStudents = new List<Student>();
            var random = new Random();
            var from = new DateTime(1990, 1, 1);
            var to = new DateTime(2000, 1, 1);

            for (int i = 0; i < 10; i++)
            {
                var student = new Student()
                {
                    Id = i,
                    Name = "Student" + random.Next(6),
                    Faculty = "Faculty" + random.Next(5),
                    Course = random.Next(1, 5),
                    DateOfBirth = GetRandomDate(from, to, random)
                };
                listStudents.Add(student);
            }
            return listStudents;
        }

        public static DateTime GetRandomDate(DateTime from, DateTime to, Random random)
        {
            TimeSpan range = to - from;

            var randomTimeSpan = new TimeSpan((long)(random.NextDouble() * range.Ticks));

            return from + randomTimeSpan;
        }
    }
}
