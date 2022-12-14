namespace Gaya.Migrations
{
    using Gaya.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Gaya.Data.GayaContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Gaya.Data.GayaContext context)
        {
            context.Processors.AddOrUpdate(x => x.Id,
                new Processor()
                {
                    Id = 1,
                    Name = "+",
                    Action = "+",
                    Description = "sum",
                    FirstParameterAsString = false,
                    SecondParameterAsString = false
                });

            context.Histories.AddOrUpdate(x => x.Id,
                new History()
                {
                    Id = 1,
                    FirstField = "1",
                    SecondField = "2",
                    ProcessorId = 1
                });
        }
    }
}
