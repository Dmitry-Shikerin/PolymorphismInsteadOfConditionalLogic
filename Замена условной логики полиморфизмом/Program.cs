using System;
using System.Collections.Generic;
using System.Linq;

namespace Замена_условной_логики_полиморфизмом
{
    class Program
    {
        static void Main(string[] args)
        {
            var orderForm = new OrderForm();

            PaymentSystem[] paymentSystems =
            {
                new QIWI("QIWI"),
                new WebMoney("WebMoney"),
                new Card("Card")
            };

            PaymantSystemFactory paymantSystemFactory = new PaymantSystemFactory
                (paymentSystems);

            var paymantSistemsNames = paymentSystems.Select(paymantSistemName => paymantSistemName.Name);

            string systemId = orderForm.ShowForm(paymantSistemsNames);

            PaymentSystem paymentSystem = paymantSystemFactory.Create(systemId);

            paymentSystem.ShowPaymentResult();
        }
    }

    public class OrderForm
    {
        public string ShowForm(IEnumerable<string> paymantSystemsNames)
        {
            Console.Write("Мы принимаем: ");

            foreach (string paymantSystemName in paymantSystemsNames)
            {
                Console.Write($" {paymantSystemName},");
            }

            Console.WriteLine();

            Console.WriteLine("Какой системой вы хотите совершить оплату?");
            return Console.ReadLine();
        }
    }

    class PaymantSystemFactory
    {
        private readonly PaymentSystem[] _paymentSystems;

        public PaymantSystemFactory(params PaymentSystem[] paymentSystems)
        {
            if (paymentSystems == null)
                throw new NullReferenceException(nameof(paymentSystems));

            _paymentSystems = paymentSystems;
        }

        public PaymentSystem Create(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            foreach (PaymentSystem paymentSystem in _paymentSystems)
            {
                if (name == paymentSystem.Name)
                {
                    return paymentSystem;
                }
            }

            throw new InvalidOperationException();
        }
    }

    abstract class PaymentSystem
    {
        protected PaymentSystem(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
        }

        public string Name { get; }

        public void ShowPaymentResult()
        {
            ShowTransition();
            Console.WriteLine($"Вы оплатили с помощью {Name}");
            Console.WriteLine($"Проверка платежа через {Name}...");
            Console.WriteLine("Оплата прошла успешно!");
        }

        protected abstract void ShowTransition();
    }

    class QIWI : PaymentSystem
    {
        public QIWI(string name) : base(name) { }

        protected override void ShowTransition()
        {
            Console.WriteLine("Перевод на страницу QIWI...");
        }
    }

    class WebMoney : PaymentSystem
    {
        public WebMoney(string name) : base(name) { }

        protected override void ShowTransition()
        {
            Console.WriteLine("Вызов API WebMoney...");
        }
    }

    class Card : PaymentSystem
    {
        public Card(string name) : base(name) { }

        protected override void ShowTransition()
        {
            Console.WriteLine("Вызов API банка эмитера карты Card...");
        }
    }
}
