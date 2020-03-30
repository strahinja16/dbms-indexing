using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Bogus;
using GeneratorApp.Model;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore.Migrations;
using Newtonsoft.Json;

namespace GeneratorApp.Data
{
    public static class DataGenerator
    {
        public static List<User> Users { get; private set; } = new List<User>();
        public static List<WarehouseStatistic> Statistics { get; private set; } = new List<WarehouseStatistic>();
        public static string[] UserExcludedProperties = { };

        public static Random rnd = new Random();
        static int moduo = 4;

        public static void GenerateData(MigrationBuilder migrationBuilder)
        {
            for (int i = 0; i < 500; i++)
            {
                Console.WriteLine($"{i + 1}th iteration of Users");
                migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Name", "LastName", "TrainingCompletionDate" },
                values: GenerateUsers(i * 1000, 1000));
            }

            for (int i = 0; i < 2200; i++)
            {
                Console.WriteLine($"{i + 1}th iteration of Statistics");

                migrationBuilder.InsertData(
                table: "WarehouseStatistics",
                columns: new[] { "Id", "ItemName", "Count" },
                values: GenerateStatistics(i * 1000, 1000));
            }
        }

        private static object[,] GenerateUsers(int start, int userCount)
        {
            const int userPropertiesCount = 4;
            object[,] users = new object[userCount, userPropertiesCount];
            int x = -1;

            for (int i = start; i < start + userCount; i++)
            {
                var testUser = new Faker<User>()

                .RuleFor(u => u.Name, f => f.Name.FirstName())

                .RuleFor(u => u.LastName, f => f.Name.LastName())

                .RuleFor(u => u.TrainingCompletionDate,
                    f => f.Date.Between(new DateTime(2019, 1, 1), new DateTime(2019, 12, 31)));

                var user = testUser.Generate();

                user.Id = i + 1;

                if (i % 10 == moduo)
                {
                    user.TrainingCompletionDate = null;
                    moduo = moduo == 4 ? 5 : 4;
                }


                Users.Add(user);

                var userGeneric = new List<object>();

                foreach (var p in user.GetType().GetProperties())
                {
                    if (!UserExcludedProperties.Contains(p.Name))
                    {
                        userGeneric.Add(p.GetValue(user, null));
                    }
                }

                x++;
                for (int j = 0; j < userGeneric.Count; j++)
                {
                    users[x, j] = userGeneric.ElementAt(j);
                }
            }

            return users;
        }

        private static object[,] GenerateStatistics(int start, int statCount)
        {
            const int statPropCount = 3;
            object[,] stats = new object[statCount, statPropCount];
            int x = -1;

            for (int i = start; i < start + statCount; i++)
            {
                var testStat = new Faker<WarehouseStatistic>()

                .RuleFor(u => u.ItemName, f => f.Lorem.Random.AlphaNumeric(10))

                .RuleFor(u => u.Count, f => f.Lorem.Random.Number(0, 500));

                var stat = testStat.Generate();

                stat.Id = i + 1;

                Statistics.Add(stat);

                var statGeneric = new List<object>();

                foreach (var p in stat.GetType().GetProperties())
                {
                    if (!UserExcludedProperties.Contains(p.Name))
                    {
                        statGeneric.Add(p.GetValue(stat, null));
                    }
                }

                x++;
                for (int j = 0; j < statGeneric.Count; j++)
                {
                    stats[x, j] = statGeneric.ElementAt(j);
                }
            }

            return stats;
        }
    }
}
