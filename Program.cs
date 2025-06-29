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
        private GoodCollection _goods = new GoodCollection();

        public void ShowAllGoods() =>
            _goods.ShowAll();

        public void Delive(Good good, int count)
        {
            _goods.Add(good, count);
        }

        public void TakeGoods(Good good, int count)
        {
            _goods.Take(good, count);
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
        private GoodCollection _goods = new GoodCollection();

        public Cart(Warehouse warehouse)
        {
            if (warehouse == null)
                throw new ArgumentNullException(nameof(warehouse));

            _warehouse = warehouse;
        }

        public void Add(Good good, int count)
        {
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

    public class GoodCollection
    {
        private Dictionary<Good, int> _goods = new Dictionary<Good, int>();

        public void ShowAll()
        {
            foreach (var good in _goods)
            {
                Console.WriteLine($"Название: {good.Key.Name}/ Количество: {good.Value}");
            }
        }

        public void Add(Good good, int count)
        {
            TryReportIncorrectData(good, count);

            if (_goods.ContainsKey(good) == false)
                _goods.Add(good, count);
            else
                IncreaseGoodCount(good, count);
        }

        public void Take(Good good, int count)
        {
            TryReportIncorrectData(good, count);
            TryReportGoodUnavailability(good);

            if (_goods[good] < count)
                throw new ArgumentException("Нет нужного количества товара на складе", nameof(count));

            _goods[good] -= count;

            if (_goods[good] == 0)
                _goods.Remove(good);
        }

        public void Clear() =>
            _goods.Clear();

        private void IncreaseGoodCount(Good good, int count)
        {
            TryReportIncorrectData(good, count);
            TryReportGoodUnavailability(good);

            _goods[good] += count;
        }

        private void TryReportIncorrectData(Good good, int count)
        {
            if (good == null)
                throw new ArgumentNullException(nameof(good));

            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));
        }

        private void TryReportGoodUnavailability(Good good)
        {
            if (_goods.ContainsKey(good) == false)
                throw new ArgumentException("Данного товара нет на складе", nameof(good));
        }
    }
}