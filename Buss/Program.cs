using System;
using System.Collections.Generic;

public class Person
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public Booking Booking { get; private set; }

    public Person(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public string GetName()
    {
        return Name;
    }

    public void AddBooking(Booking booking)
    {
        Booking = booking;
    }
}

public class Seat
{
    public int Number { get; private set; }
    public Bus Bus { get; private set; }
    public bool Booked { get; private set; }

    public Seat(int number, Bus bus)
    {
        Number = number;
        Bus = bus;
        Booked = false;
    }

    public void ChangeBookingStatus()
    {
        Booked = !Booked;
    }
}

public class Bus
{
    public List<Seat> Seats { get; private set; }
    public string Destination { get; private set; }

    public Bus(string destination, int seatCount)
    {
        Destination = destination;
        Seats = new List<Seat>();
        for (int i = 1; i <= seatCount; i++)
        {
            Seats.Add(new Seat(i, this));
        }
    }

    public Seat GetSeatInfo(int seatNumber)
    {
        return Seats.Find(s => s.Number == seatNumber);
    }
}

public class Booking
{
    public Person Booker { get; private set; }
    public Bus Bus { get; private set; }
    public Seat Seat { get; private set; }
    public bool Cancelled { get; private set; }
    public DateTime Date { get; private set; }

    public Booking(Bus bus, Person booker, int seatNumber)
    {
        Bus = bus;
        Booker = booker;
        Seat = bus.GetSeatInfo(seatNumber);
        if (Seat != null && !Seat.Booked)
        {
            Seat.ChangeBookingStatus();
            Date = DateTime.Now;
            Cancelled = false;
        }
        else
        {
            throw new Exception("Seat is already booked or invalid.");
        }
    }

    public void EditBooking(int newSeatNumber)
    {
        if (Cancelled)
        {
            throw new Exception("Cannot edit a cancelled booking.");
        }

        Seat.ChangeBookingStatus();
        Seat = Bus.GetSeatInfo(newSeatNumber);
        if (Seat != null && !Seat.Booked)
        {
            Seat.ChangeBookingStatus();
        }
        else
        {
            throw new Exception("New seat is already booked or invalid.");
        }
    }

    public void CancelBooking()
    {
        if (!Cancelled)
        {
            Seat.ChangeBookingStatus();
            Cancelled = true;
        }
    }
}

public class BookingSystem
{
    private List<Booking> bookings;
    private List<Person> customers;
    private Bus bus;

    public BookingSystem()
    {
        bookings = new List<Booking>();
        customers = new List<Person>();
        bus = new Bus("Stockholm", 20); // Destination och antal platser
    }

    public void Menu()
    {
        while (true)
        {
            Console.WriteLine("\nVälkommen till Bokningssystemet!");
            Console.WriteLine("1. Skapa bokning");
            Console.WriteLine("2. Ändra bokning");
            Console.WriteLine("3. Avboka resa");
            Console.WriteLine("4. Avsluta");
            Console.Write("Välj ett alternativ: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateBooking();
                    break;
                case "2":
                    EditBooking();
                    break;
                case "3":
                    CancelBooking();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Ogiltigt val.");
                    break;
            }
        }
    }

    public void CreateBooking()
    {
        Console.Write("Ange ditt namn: ");
        string name = Console.ReadLine();
        var person = customers.Find(p => p.Name == name) ?? new Person(customers.Count + 1, name);

        if (!customers.Contains(person))
        {
            customers.Add(person);
        }

        Console.Write("Ange platsnummer (1-20): ");
        int seatNumber = int.Parse(Console.ReadLine());

        try
        {
            var booking = new Booking(bus, person, seatNumber);
            person.AddBooking(booking);
            bookings.Add(booking);
            Console.WriteLine("Bokningen lyckades!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Misslyckades: {ex.Message}");
        }
    }

    public void EditBooking()
    {
        Console.Write("Ange ditt namn: ");
        string name = Console.ReadLine();
        var person = customers.Find(p => p.Name == name);

        if (person?.Booking != null && !person.Booking.Cancelled)
        {
            Console.Write("Ange nytt platsnummer (1-20): ");
            int newSeatNumber = int.Parse(Console.ReadLine());
            try
            {
                person.Booking.EditBooking(newSeatNumber);
                Console.WriteLine("Bokningen ändrades!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Misslyckades: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Ingen aktiv bokning hittades.");
        }
    }

    public void CancelBooking()
    {
        Console.Write("Ange ditt namn: ");
        string name = Console.ReadLine();
        var person = customers.Find(p => p.Name == name);

        if (person?.Booking != null && !person.Booking.Cancelled)
        {
            person.Booking.CancelBooking();
            Console.WriteLine("Bokningen avbokades!");
        }
        else
        {
            Console.WriteLine("Ingen aktiv bokning hittades.");
        }
    }

    public static void Main(string[] args)
    {
        BookingSystem bookingSystem = new BookingSystem();
        bookingSystem.Menu();
    }
}
