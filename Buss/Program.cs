using System;
using System.Collections.Generic;

public class Person
{
    public string Name { get; private set; }

    public Person(string name)
    {
        Name = name;
    }
}

public class Seat
{
    public int Number { get; private set; }
    public bool Booked { get; private set; }

    public Seat(int number)
    {
        Number = number;
        Booked = false;
    }

    public void Book()
    {
        Booked = true;
    }

    public void Cancel()
    {
        Booked = false;
    }
}

public class Bus
{
    public List<Seat> Seats { get; private set; }

    public Bus(int seatCount)
    {
        Seats = new List<Seat>();
        for (int i = 1; i <= seatCount; i++)
        {
            Seats.Add(new Seat(i));
        }
    }

    public Seat GetSeat(int seatNumber)
    {
        return Seats.Find(s => s.Number == seatNumber);
    }
}

public class BookingSystem
{
    private Bus bus;

    public BookingSystem()
    {
        bus = new Bus(20); // 20 seats
    }

    public void Menu()
    {
        while (true)
        {
            Console.WriteLine("\n1. Boka plats\n2. Avboka plats\n3. Avsluta");
            Console.Write("Välj ett alternativ: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateBooking();
                    break;
                case "2":
                    CancelBooking();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Ogiltigt val.");
                    break;
            }
        }
    }

    public void CreateBooking()
    {
        Console.Write("Ange ditt platsnummer (1-20): ");
        int seatNumber = int.Parse(Console.ReadLine());
        Seat seat = bus.GetSeat(seatNumber);

        if (seat != null && !seat.Booked)
        {
            seat.Book();
            Console.WriteLine("bokad plats!");
        }
        else
        {
            Console.WriteLine("Platsen är bokad eller ogiltig.");
        }
    }

    public void CancelBooking()
    {
        Console.Write("Ange platsnummer (1-20): ");
        int seatNumber = int.Parse(Console.ReadLine());
        Seat seat = bus.GetSeat(seatNumber);

        if (seat != null && seat.Booked)
        {
            seat.Cancel();
            Console.WriteLine("Bokningen avbokad!");
        }
        else
        {
            Console.WriteLine("Ingen bokning hittades på den platsen.");
        }
    }

    public static void Main(string[] args)
    {
        BookingSystem bookingSystem = new BookingSystem();
        bookingSystem.Menu();
    }
}
