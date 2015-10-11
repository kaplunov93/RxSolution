using System;
using System.Data.SqlClient;
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
            Customer_Wrapper my=new Customer_Wrapper(me);
            
            Obs(my);
            
            while(true)
            {
                int age = 0;
                int.TryParse(Console.ReadLine(), out age);
                if (age != 0)
                    my.Age = age;
            }

            //Console.ReadKey();
        }

        public static void Obs(Customer_Wrapper me )
        {
            var ages = FromEvent<Customer_Wrapper.Changer, int>
                (h => me.OnChange += h, h => me.OnChange -= h);

            ages.Buffer(3,1)
                .Subscribe(
                    buffer =>
                    {
                        if (buffer[0] == 10 && buffer.Any(v => v == 20))
                            Console.WriteLine("Catch It");
                    },
                    () => Console.WriteLine("Complited"));
        }

        public class Customer_Wrapper
        {
            private Customer _customer;

            public delegate void Changer(int age);

            public event Changer OnChange;

            public string FirstName
            {
                get { return _customer.FirstName; }
                set { _customer.FirstName = value; }
            }

            public string LastName
            {
                get { return _customer.FirstName; }
                set { _customer.FirstName = value; }
            }

            public int Age
            {
                get { return _customer.Age; }
                set
                {
                    _customer.Age = value;
                    OnChange(Age);
                }
            }


            public Customer_Wrapper(Customer customer)
            {
                this._customer = customer;
            }
        }

        public class Customer
        {
            private string _firstName;
            private string _lastName;
            private int _age;
            
            //public event EventHandler OnAgeChange;

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
                    //OnAgeChange?.Invoke(this,_age);
                }
            }
        }
    }
}
