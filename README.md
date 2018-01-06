**Battery Metrics Project**

**Introduction**

A console and web application using .net Core 2, that analyses device data from wearable bracelets. Device data is uploaded and battery metrics (Battery lifetime, Charging time, Number of charge cycles) are calculated and outputted to user on screen and exported as a csv file.

**Setup**

This project has two branches; *master* - which contains the console application and *webapp* - which contains the web application.

Each application can be run from cmd line.

$ cd /battery-metrics/battery-metrics
$ dotnet run

The web app can send emails, but the smpt server must be setup. I have utilised Gmail's SmtpClient to send emails.
To setup go to the *HomeController* and edit the following code;

private static SmtpClient smtp = new SmtpClient
{
    Host = "smtp.gmail.com",
    Port = 587,
    EnableSsl = true,
    DeliveryMethod = SmtpDeliveryMethod.Network,
    UseDefaultCredentials = false,
    Credentials = new NetworkCredential("ENTER GMAIL EMAIL", "ENTER GMAIL APP PASSWORD")
};

To obtain a *"GMAIL APP PASSWORD"* go to your gmail account security page, and edit the *Password and Sign in Method* to create a password. For more detailed instructions follow; https://galleryserverpro.com/use-gmail-as-your-smtp-server-even-when-using-2-factor-authentication-2-step-verification/
