using System;

namespace BuilderFacet
{
    internal class Program
    {
        class Person
        {
            //address
            public string StreetAddress, PostCode, City;

            //employment
            public string CompanyName, Position;

            public int AnnualIncome;


            public override string ToString()
            {
                return
                    $"{nameof(StreetAddress)}: {StreetAddress}, {nameof(PostCode)}: {PostCode}, {nameof(City)}: {City}, {nameof(CompanyName)}: {CompanyName}, {nameof(Position)}: {Position}, {nameof(AnnualIncome)}: {AnnualIncome}";
            }
        }

        class PersonBuilder
        {
            protected Person person = new Person();

            public PersonWorkBuilder Works => new PersonWorkBuilder(this.person);
            public PersonAddressBuilder Lives => new PersonAddressBuilder(this.person);

            public override string ToString()
            {
                return person.ToString();
            }
        }

        class PersonWorkBuilder : PersonBuilder
        {
            public PersonWorkBuilder(Person person)
            {
                this.person = person;
            }

            public PersonWorkBuilder At(string companyName)
            {
                person.CompanyName = companyName;
                return this;
            }

            public PersonWorkBuilder AsA(string position)
            {
                person.Position = position;
                return this;
            }

            public PersonWorkBuilder Earning(int annualIncome)
            {
                person.AnnualIncome = annualIncome;
                return this;
            }
        }

        class PersonAddressBuilder : PersonBuilder
        {
            public PersonAddressBuilder(Person person)
            {
                this.person = person;
            }

            public PersonAddressBuilder At(string streetAddress)
            {
                person.StreetAddress = streetAddress;
                return this;
            }

            public PersonAddressBuilder WithPostCode(string postCode)
            {
                person.PostCode = postCode;
                return this;
            }

            public PersonAddressBuilder In(string city)
            {
                person.City = city;
                return this;
            }
        }

        public static void Main(string[] args)
        {
            var pb = new PersonBuilder();

            pb.Lives.At("Krottendorf 8").WithPostCode("8509").In("Kapfenberg")
                .Works.At("Google").AsA("CTO").Earning(120000);
            Console.WriteLine(pb);
        }
    }
}