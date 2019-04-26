using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicExpressionForStudents.Model
{
    public class Student
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Faculty { get; set; }

        public int Course { get; set; }

        public DateTime DateOfBirth { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Faculty: {Faculty}, " +
                $"Course: {Course}, DOB: {DateOfBirth.ToShortDateString()}.";
        }
    }
}
