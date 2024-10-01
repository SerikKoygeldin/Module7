using System.Diagnostics;
using System.Net;
using System.Xml.Linq;

namespace Module7
{
    internal class Program
    {
        static void Main(string[] args)
        {

            int amount = 0;
            Console.WriteLine("Введите количество товаров");
            int.TryParse(Console.ReadLine(), out amount);
        
            Product[] arrayProduct = new Product[amount];

            for (int i = 1; i <= arrayProduct.Length; i++) {

                Console.WriteLine("Введите название {0} товара",i);

                string productName = Console.ReadLine();
                Product product = new Product(productName);

                Console.WriteLine("Введите вес {0} товара", i);

                double weight = 0; 
                double.TryParse(Console.ReadLine(),out weight); 

                product.Weight = weight;

                Console.WriteLine("Введите стоимость {0} товара", i);

                double productPrice = 0;
                double.TryParse(Console.ReadLine(), out productPrice);

                product.Price = productPrice;

                arrayProduct[i-1] = product;
            }


            Order<Delivery, string> order = new Order<Delivery, string>();

            HomeDelivery Delivery = new HomeDelivery("new adress");
            Delivery.СourierName = "Courier 1";

            order.Client = new Client("Client 1");
            order.Delivery = Delivery;
            order.Description = "Доставка";
            order.Number = "N-0001";
            order.OrderProduct = arrayProduct;

            Console.WriteLine("Введите цену за кг");

            double price = 0;
            double.TryParse(Console.ReadLine(), out price);

            order.Price = price;
            order.Cost = OrderCalculation.Cost(order.Weight(), order.Price);

            Console.WriteLine("-------------------");
            order.DispayOrder();
            Console.WriteLine("-------------------");
            Console.ReadKey();           
        }
    }

    abstract class Delivery
    {
        public string Address;

        public Delivery(string address)
        {
            Address = address;
        }

        public virtual void Display()
        {
            
        }


    }

    //Доствка до двери (Курьер)
    class HomeDelivery : Delivery
    {
        // Этот тип будет подразумевать наличие курьера или передачу курьерской компании, в нем будет располагаться своя, отдельная от прочих типов доставки логика.

        private Courier сourier;
        private string courierName = "Undefined";

        public HomeDelivery(string address) : base(address)
        {
            сourier = new Courier(courierName);
        }

        public string СourierName
        {
            get
            {
                return courierName;
            }
            set
            {
                //можно добавить проверку по шаблону в будущем 
                courierName = value;
            }
        }

        public override void Display()
        {
            Console.WriteLine("Адрес - " + this.Address);
            Console.WriteLine("Имя курьера - " + this.courierName);
            Console.WriteLine("Номер курьера - " + this.сourier.CourierNumber);
        }


    }

    //Доствка до пункта выдачи
    class PickPointDelivery : Delivery
    {
        //Здесь будет храниться какая-то ещё логика, необходимая для процесса доставки в пункт выдачи, например, хранение компании и точки выдачи, а также какой-то ещё информации.
 
        private string pointName = "Undefined";

        public string PointName
        {
            get
            {
                return pointName;    
            }
            set
            {
                pointName = value;   
            } 
        }
      
        public PickPointDelivery(string address) : base(address)
        {

        }

        public override void Display()
        {
            Console.WriteLine("Адрес - " + this.Address);
            Console.WriteLine("Точка - " + this.pointName);
        }

    }

    //Доствка до магазина
    class ShopDelivery : Delivery
    {
        //Эта доставка может выполняться внутренними средствами компании и совсем не требует работы с «внешними&raquo; элементами.
        public ShopDelivery(string address) : base(address)
        {

        }

        public override void Display()
        {
           
        }
    }

    class Order<TDelivery, TNumber> where TDelivery : Delivery
    {
        public TDelivery Delivery;

        public TNumber Number;

        public string Description;

        public void DisplayAddress()
        {
            Console.WriteLine(Delivery.Address);
        }

       // public Product Product;

        public Product[] OrderProduct;
        
        private double price = 0;

        public double Price {

            get
            {
                return price;
            }
            set
            {
                price = value;
            }
        }

        public double Weight()
        {
            double weight = 0.0;

            for (int i = 0; i < OrderProduct.Length; i++) {

                weight += OrderProduct[i].Weight;
            }

            return weight;
        }

        public Client Client;

        public double Cost;

        public void DispayOrder()
        {

            // стоимость доставки
            // стоимость товаров
            // доставка

            Console.WriteLine("Номер заказа - " + Number.ToString());
            Console.WriteLine("Описание - " + Description);
            Console.WriteLine("Клиент - " + Client.Name);

            double TotalCost = 0;

            for (int i = 0; i < OrderProduct.Length; i++)
            {
                OrderProduct[i].Display();

                TotalCost += OrderProduct[i].Price;
            }

            Console.WriteLine("Стоимость товаров - " + TotalCost.ToString());
            Console.WriteLine("Стоимость доставки - " + this.Cost.ToString());

            Delivery.Display();


        }


    }

    class Product {

        private string Name;
        private double weight;
        private double price;

        public double Weight
        {
            get
            {
                return weight;
            }
            set
            {
                weight = value;
            }
        }

        public double Price
        {
            get
            {
                return price;
            }
            set
            {
                price = value;
            }
        }

        public void Display()
        {
            Console.WriteLine("Название товара - " + Name);
            Console.WriteLine("Вес товара - " + weight.ToString());
            Console.WriteLine("Стоимость товара - " + price.ToString());
        }

        public Product(string name) 
        {
            this.Name = name;
        }

        public Product()
        {
            Name = "new product";
            price = 0;
            weight = 0;
        }
    }

    class OrderCalculation {

        public static double Cost(double weight, double price)
        {
            double cost = weight * price;          
            return cost;
        }
    }

    abstract class Person { 
    
        public string Name;

        public Person(string name)
        {
            Name = name;
        }

        public virtual void Describe()
        {
            Console.WriteLine("Name - " + Name);
        }

    }

    class Client : Person {

        public Client(string name) : base(name)
        {
            
        }

    }

    class Courier : Person
    {
        private string courierNumber = "+7 ()   -  -  ";

        public string CourierNumber
        {
            get
            {
                return courierNumber;
            }
            set
            {
                //можно добавить проверку по шаблону в будущем 
                courierNumber = value;
            }
        }
        public Courier(string name) : base(name)
        {

        }

        public override void Describe()
        {
            Console.WriteLine("Имя курьера - " + this.Name);
            Console.WriteLine("Номер курьера - " + this.courierNumber);
        }

    }


}
