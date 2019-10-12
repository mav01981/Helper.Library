using Helper.FileConvertor;
using Helper.FileConvertor.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using Xunit;

namespace Tests.FileConvertor
{
    public class ByteConvertorTests
    {
        private static IServiceProvider _serviceProvider;

        private readonly string _filepath;

        public ByteConvertorTests()
        {
            _filepath = $@"{Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)}";

            var collection = new ServiceCollection();
            collection.AddScoped<IByteConvertor, ByteConvertor>();

            _serviceProvider = collection.BuildServiceProvider();

            var directory = new DirectoryInfo($@"{_filepath}\Output\");

            foreach (FileInfo file in directory.EnumerateFiles())
            {
                file.Delete();
            }
        }

        [Fact]
        public void ConvertFileToBytes()
        {
            //Arrange
            var fileConvertor = _serviceProvider.GetService<IByteConvertor>();
            //Act
            var bytes = fileConvertor.ConvertToBytes($@"{_filepath}\Files\Untitled Document.pdf");
            //Assert
            Assert.Equal(typeof(byte[]), bytes.GetType());
        }

        [Fact]
        public void ConvertBytesToFile()
        {
            //Arrange
            var fileConvertor = _serviceProvider.GetService<IByteConvertor>();
            //Act
            var data = fileConvertor.ConvertToBytes($@"{_filepath}\Files\Untitled Document.pdf");
            fileConvertor.ConvertBytesToFile($@"{_filepath}\Output\Untitled Document", FileExtensionsEnums.PDF, data);
            //Assert
            Assert.True(File.Exists($@"{_filepath}\Output\Untitled Document.pdf"));
        }
    }
}
