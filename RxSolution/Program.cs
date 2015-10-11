using System;
using System.Linq;
using System.Reactive.Linq;
using static System.Reactive.Linq.Observable;

namespace RxSolution
{
    class Program
    {


        private static void Main(string[] args)
        {
            Customer me = new Customer
            {
                FirstName = "Alex",
                LastName = "Kaplunov",
                Age = 1
            };
            
            Obs(me);
            
            while(true)
            {
                int age = 0;
                int.TryParse(Console.ReadLine(), out age);
                if (age != 0)
                    me.Age = age;
            }

            //Console.ReadKey();
        }

        public static void Obs(Customer me )
        {
            var ages = FromEvent<Customer.Changer, int>
                (h => me.OnAgeChange += h, h => me.OnAgeChange -= h);

            ages.Buffer(3,1)
                .Subscribe(
                    buffer =>
                    {
                        if (buffer[0] == 10 && buffer.Any(v => v == 20))
                            Console.WriteLine("Catch It");
                    },
                    () => Console.WriteLine("Complited"));
        }

        public class Customer
        {
            public delegate void Changer(int age);

            private string _firstName;
            private string _lastName;
            private int _age;
            
            public event Changer OnAgeChange;

            public string FirstName
            {
                get
                {
                    return _firstName;
                }

                set
                {
                    if (!string.IsNullOrEmpty(value))
                        _firstName = value;
                }
            }

            public string LastName
            {
                get
                {
                    return _lastName;
                }

                set
                {
                    if (!string.IsNullOrEmpty(value))
                        _lastName = value;
                }
            }

            public int Age
            {
                get { return _age; }
                set
                {
                    _age = value;
                    OnAgeChange?.Invoke(_age);
                }
            }
        }
    }
}
