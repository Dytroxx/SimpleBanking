using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SimpleBanking;

class Program
{
    static void Main()
    {
        var bankOperations = new BankOperations();

        // Create and start the web server
        var host = new WebHostBuilder()
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureServices(services => services.AddRouting())
            .Configure(app =>
            {
                app.UseRouting();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapPost("/create-customer", async context =>
                    {
                        var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
                        var requestData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(requestBody);

                        // Check if create customer option is selected in the frontend
                        if (requestData.createCustomer)
                        {
                            var customerName = requestData.customerName.ToString();

                            var newCustomer = bankOperations.CreateCustomer(customerName);

                            context.Response.Headers.Add("Content-Type", "application/json");
                            await context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(new
                            {
                                success = true,
                                customer = newCustomer
                            }));
                        }
                        else
                        {
                            context.Response.Headers.Add("Content-Type", "application/json");
                            await context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(new
                            {
                                success = false,
                                message = "Create customer option not selected"
                            }));
                        }
                    });

                    endpoints.MapPost("/deposit", async context =>
                    {
                        var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
                        var requestData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(requestBody);
                        var amount = requestData.amount.ToObject<decimal>();

                        bankOperations.Deposit(amount);

                        context.Response.Headers.Add("Content-Type", "application/json");
                        await context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(new
                        {
                            success = true,
                            account = bankOperations.GetCurrentAccount()
                        }));
                    });

                    endpoints.MapPost("/withdraw", async context =>
                    {
                        var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
                        var requestData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(requestBody);
                        var amount = requestData.amount.ToObject<decimal>();

                        bankOperations.Withdraw(amount);

                        context.Response.Headers.Add("Content-Type", "application/json");
                        await context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(new
                        {
                            success = true,
                            account = bankOperations.GetCurrentAccount()
                        }));
                    });
                });
            })
            .Build();

        host.Run();
    }
}
