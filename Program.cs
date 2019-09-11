using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using CommandLine;

namespace dotnetcore_test_smtp
{
    class Program
    {

        class Options
        {
            [Option("server", Required = true, HelpText = "SMTP Server name.")]
            public string Server { get; set; }

            [Option("port", HelpText = "SMTP Server port number.")]
            public int Port { get; set; }

            [Option('u', "user", Required = true, HelpText = "Username.")]
            public string Username { get; set; }

            [Option('p', "pwd", Required = true, HelpText = "Password.")]
            public string Password { get; set; }

            [Option("to", Required = true, HelpText = "Email To.")]
            public string To { get; set; }

            [Option("subject", Required = true, HelpText = "Email subject.")]
            public string Subject { get; set; }

            [Option("body", Required = true, HelpText = "Email body.")]
            public string Body { get; set; }

        }

        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(opts => RunOptionsAndReturnExitCode(opts))  
                .WithNotParsed<Options>((errs) => HandleParseError(errs));
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            errs.Output();
        }

        private static void RunOptionsAndReturnExitCode(Options opts)
        {
            try
            {
                Console.WriteLine("Preparing message...");

                var client = new SmtpClient(opts.Server, opts.Port > 0 ? opts.Port : 25 );
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(opts.Username, opts.Password);

                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(opts.Username);
                mailMessage.To.Add(opts.To);
                mailMessage.Subject = opts.Subject;
                mailMessage.Body = opts.Body;
                client.Send(mailMessage);

                Console.WriteLine("Message sent! :)\nCheck your inbox!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong!\n");
                Console.WriteLine("ERROR:\n");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
