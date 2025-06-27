using System;
using System.Collections.Generic;

namespace Store
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Good iPhone12 = new Good("IPhone 12");
            Good iPhone11 = new Good("IPhone 11");

            Warehouse warehouse = new Warehouse();

            Shop shop = new Shop(warehouse);

            warehouse.Delive(iPhone12, 10);
            warehouse.Delive(iPhone11, 1);

            //Вывод всех товаров на складе с их остатком

            Cart cart = shop.Cart();
            cart.Add(iPhone12, 4);
            cart.Add(iPhone11, 3); //при такой ситуации возникает ошибка так, как нет нужного количества товара на складе

            //Вывод всех товаров в корзине

            Console.WriteLine(cart.Order().Paylink);

            cart.Add(iPhone12, 9); //Ошибка, после заказа со склада убираются заказанные товары
        }
    }

    public class Good
    {
        public Good(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            Name = name;
        }

        public string Name { get; }
    }

    public class Warehouse
    {
        private Dictionary<Good, int> _goods = new Dictionary<Good, int>();

        public void Delive(Good good, int count)
        {
            if (good == null)
                throw new ArgumentNullException(nameof(good));

            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            _goods.Add(good, count);
        }

        public void TakeGoods(Good good, int count)
        {
            if (good == null)
                throw new ArgumentNullException(nameof(good));

            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (_goods.ContainsKey(good) == false)
                throw new ArgumentException("Данного товара нет на складе", nameof(good));

            if (_goods[good] < count)
                throw new ArgumentException("Нет нужного количества товара на складе", nameof(count));

            _goods[good] -= count;

            if (_goods[good] == 0)
                _goods.Remove(good);
        }
    }

    public class Shop
    {
        private Warehouse _warehouse;

        public Shop(Warehouse warehouse)
        {
            if (warehouse == null)
                throw new ArgumentNullException(nameof(warehouse));

            _warehouse = warehouse;
        }

        public Cart Cart() =>
            new Cart(_warehouse);
    }

    public class Cart
    {
        private Warehouse _warehouse;
        private Dictionary<Good, int> _goods = new Dictionary<Good, int>();

        public Cart(Warehouse warehouse)
        {
            if (warehouse == null)
                throw new ArgumentNullException(nameof(warehouse));

            _warehouse = warehouse;
        }

        public void Add(Good good, int count)
        {
            if (good == null)
                throw new ArgumentNullException(nameof(good));

            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            _warehouse.TakeGoods(good, count);
            _goods.Add(good, count);
        }

        public Order Order()
        {
            _goods.Clear();

            return new Order();
        }
    }

    public class Order
    {
        public string Paylink { get; } = "123";
    }
}