
using static System.Net.Mime.MediaTypeNames;
using System.Net.Mail;
using System;

class Activity
{
    public string ActivityRef { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public List<Activity> ClashedActivities { get; set; }
}
class Program
{
    private static List<Activity> clashingActivities = new List<Activity>();
    private static List<Activity> activities = new List<Activity>();
    static void Main(string[] args)
    {
        // Path to the CSV file

        string csvFilePath = "C:\\Users\\goyal\\SampleActivities.csv";


        // Create a StreamReader object to read the CSV file
        using (StreamReader sr = new StreamReader(csvFilePath))
        {
            // Read the first line to skip the header row
            sr.ReadLine();

            // Loop through the rest of the file
            while (!sr.EndOfStream)
            {
                // Read a line and split it by commas
                string[] values = sr.ReadLine().Split(',');

                // Access the values by their index in the array
                string activityRef = values[0];
                DateTime StartDate = DateTime.Parse(values[1]);
                DateTime EndDate = DateTime.Parse(values[2]);
                string WeekPattern = values[3];
                string Name = values[4];
                //if(activities.Count==1)
                //    activities.Add(new Activity { ActivityRef = activityRef, Start = StartDate, End = EndDate, ClashedActivities = null });
                if (GetClashActivities(StartDate) != null)
                {
                    activities.Add(new Activity { ActivityRef = activityRef, Start = StartDate, End = EndDate, ClashedActivities = GetClashActivities(StartDate) });
                }
                else
                {
                    activities.Add(new Activity { ActivityRef = activityRef, Start = StartDate, End = EndDate, ClashedActivities = null });
                }


            }
            PrintClashActivity();
            Console.Read();
        }

    }

    private static void PrintClashActivity()
    {

        foreach (var activity in activities.Where(a => a.ClashedActivities != null))
        {
            Console.WriteLine("_________");
            Console.WriteLine("Activity:", activity.ActivityRef);
            Console.WriteLine(activity.ActivityRef);
            Console.WriteLine("_________");

            Console.WriteLine("Clashes With:");
            Console.WriteLine("_________");

            foreach (var item in activity.ClashedActivities)
            {
                Console.WriteLine("{0} on ({1} - {2})", item.ActivityRef, item.Start.ToShortDateString(), item.End.ToShortDateString());
            }
            Console.WriteLine("_________");
        }



    }

    private static List<Activity> GetClashActivities(DateTime StartDate)
    {
        if (activities != null && activities.Count() > 0)
        {
            List<Activity> overLappingActivities = new List<Activity>();
            var FoundActivities = activities
                 .Where(a => a.Start.Date == StartDate.Date || a.End.Date == StartDate.Date || (a.Start < StartDate && a.End > StartDate))
                 .ToList();

            if (FoundActivities.Count() == 0)
            {
                return null;

            }
            else
            {
                foreach (var item in FoundActivities)
                {
                    overLappingActivities.Add(item);
                }

                return overLappingActivities;
            }
        }
        return null;

    }
}
