﻿using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CatFactory.SqlServer.Tests
{
    public class DocumentationTests
    {
        [Fact]
        public void TestGetMsDescriptionTest()
        {
            // Arrange
            var databaseFactory = new SqlServerDatabaseFactory
            {
                ConnectionString = "server=(local);database=AdventureWorks2012;integrated security=yes;",
                ImportMSDescription = true,
                ExclusionTypes = new List<String>()
                {
                    "geography"
                }
            };

            // Act
            var database = databaseFactory.Import();

            var table = database.FindTableBySchemaAndName("Production.Product");

            var view = database.Views.First(item => item.FullName == "HumanResources.vEmployee");

            // Assert
            Assert.True(table.Description != null);
            Assert.True(table.Columns.First().Description != null);

            Assert.True(view.Description != null);
            Assert.True(view.Columns.First().Description != null);
        }

        [Fact]
        public void TestAddMsDescriptionTest()
        {
            // Arrange
            var databaseFactory = new SqlServerDatabaseFactory
            {
                ConnectionString = "server=(local);database=Northwind;integrated security=yes;",
                ImportMSDescription = true
            };

            // Act
            var database = databaseFactory.Import();

            var table = database.FindTableBySchemaAndName("dbo.Products");

            databaseFactory.DropMsDescription(table);

            databaseFactory.AddMsDescription(table, "Test description");

            var column = table.Columns.FirstOrDefault();

            databaseFactory.DropMsDescription(table, column);

            databaseFactory.AddMsDescription(table, column, "Primary key");

            // Assert
            Assert.True(table.Description == "Test description");
            Assert.True(column.Description == "Primary key");
        }

        [Fact]
        public void TestUpdateMsDescriptionTest()
        {
            // Arrange
            var databaseFactory = new SqlServerDatabaseFactory
            {
                ConnectionString = "server=(local);database=Northwind;integrated security=yes;",
                ImportMSDescription = true
            };

            // Act
            var database = databaseFactory.Import();

            var table = database.FindTableBySchemaAndName("dbo.Products");

            databaseFactory.UpdateMsDescription(table, "Test update description");

            var column = table.Columns.FirstOrDefault();

            databaseFactory.UpdateMsDescription(table, column, "PK (updated)");

            // Assert
            Assert.True(table.Description == "Test update description");
            Assert.True(column.Description == "PK (updated)");
        }
    }
}
