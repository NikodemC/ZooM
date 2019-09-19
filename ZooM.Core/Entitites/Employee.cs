using System;
using ZooM.Core.Enums;
using ZooM.Core.Exceptions;

namespace ZooM.Core.Entitites
{
    public class Employee
    {
        public Employee(Guid id, string avatar, string name, Position position, int yearOfBirth)
        {
            Id = id;
            Avatar = avatar;
            Name = name;
            Position = position;
            SetYOB(yearOfBirth);
        }

        public Guid Id { get; }
        public string Avatar { get; private set; }
        public string Name { get; }
        public Position Position { get; private set; }
        public int YearOfBirth { get; private set; }

        public void ChangePosition(Position position)
        {
            Position = position;
        }

        public void ChangeAvatar(string avatar)
        {
            Avatar = avatar;
        }

        private void SetYOB(int yearOfBirth)
        {
            if (yearOfBirth > DateTime.Today.Year - 18 || yearOfBirth < DateTime.Today.Year - 80)
                throw new DomainException("Employee age is not within the range");
            YearOfBirth = yearOfBirth;
        }
    }
}