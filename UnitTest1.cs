using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Xunit;

namespace ProductsApp.Tests
{

    public class ProductsAppShould
    {
        // Add your test here

        [Fact]
        public void Return_Argument_Null_Exception_When_Product_Not_Specified()
        {
            //Arrange
            var products = new Products();

            //Act
            var exception = Assert.Throws<ArgumentNullException>(() => products.AddNew(null));

            //Assert
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void Add_New_Product_Should_Add_To_Products()
        {
            //Arrange
            var products = new Products();
            var product = new Product() { IsSold = false, Name = "Test Product" };

            //Act
            products.AddNew(product);

            //Assert
            Assert.NotNull(products.Items);
            Assert.True(products.Items.Count() > 0 );
        }

        [Fact]
        public void Return_Name_Required_Exception_When_Product_Name_Is_Null()
        {
            //Arrange
            var product = new Product() { IsSold = false, Name = null };

            //Act
            var exception = Assert.Throws<NameRequiredException>(() => product.Validate());

            //Assert
            Assert.IsType<NameRequiredException>(exception);
        }

        [Fact]
        public void Sold_Products_Should_Not_Be_Counted_On_Items()
        {
            //Arrange
            var products = new Products();
            var product1 = new Product() { IsSold = false, Name = "Test Product 1" };
            var product2 = new Product() { IsSold = false, Name = "Test Product 2" };
            var expected = 1;

            //Act
            products.AddNew(product1);
            products.AddNew(product2);
            products.Sold(product2);
            
            var actual = products.Items.Count();

            //Assert
            Assert.NotNull(products.Items);
            Assert.True(product2.IsSold);
            Assert.Equal(expected, actual);
        }
    }

    internal class Products
    {
        private readonly List<Product> _products = new List<Product>();

        public IEnumerable<Product> Items => _products.Where(t => !t.IsSold);

        public void AddNew(Product product)
        {
            product = product ??
                throw new ArgumentNullException();
            product.Validate();
            _products.Add(product);
        }

        public void Sold(Product product)
        {
            product.IsSold = true;
        }

    }

    internal class Product
    {
        public bool IsSold { get; set; }
        public string Name { get; set; }

        internal void Validate()
        {
            Name = Name ??
                throw new NameRequiredException();
        }

    }

    [Serializable]
    internal class NameRequiredException : Exception
    {
        public NameRequiredException() { /* ... */ }

        public NameRequiredException(string message) : base(message) { /* ... */ }

        public NameRequiredException(string message, Exception innerException) : base(message, innerException) { /* ... */ }

        protected NameRequiredException(SerializationInfo info, StreamingContext context) : base(info, context) { /* ... */ }
    }
}