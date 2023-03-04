using gitguardian_integration;
using Microsoft.AspNetCore.Http;
using System.Text;
using Xunit;

namespace testing
{
    public class UnitTest1
    {
        //create 2 unit test for IsFileOverSize
        [Fact]
        public void IsFileOverSize_WhenFileIsNotOverSize_ReturnFalse()
        {
            //arrange
            byte[] bytes = Encoding.UTF8.GetBytes("cscs");

            var file = new FormFile(
                baseStream: new MemoryStream(bytes),
                baseStreamOffset: 0,
                length: bytes.Length,
                name: "Data",
                fileName: "cs"
            );

            //act
            var result = Helper.IsFileOverSize(file);

            //assert
            Assert.False(result);
        }

        [Fact]
        public void IsFileOverSize_WhenFileIsOverSize_ReturnTrue()
        {
            //arrange
            byte[] bytes = Encoding.UTF8.GetBytes("cscs");

            var file = new FormFile(
                baseStream: new MemoryStream(bytes),
                baseStreamOffset: 0,
                length: bytes.Length,
                name: "Data",
                fileName: "cs"

            );

            //act
            var result = Helper.IsFileOverSize(file, 1);

            //assert
            Assert.True(result);
        }

        //create 2 unit test for IsFileTextBased
        [Fact]
        public void IsFileTextBased_WhenFileIsTextBased_ReturnTrue()
        {
            //arrange
            byte[] bytes = Encoding.UTF8.GetBytes("cscs");

            var file = new FormFile(
                baseStream: new MemoryStream(bytes),
                baseStreamOffset: 0,
                length: bytes.Length,
                name: "Data",
                fileName: "cs"
            );

            //act
            var result = Helper.IsFileTextBased(file);

            //assert
            Assert.True(result);
        }

        [Fact]
        public void IsFileTextBased_WhenFileIsNotTextBased_ReturnFalse()
        {
            //arrange
            byte[] bytes = Encoding.UTF8.GetBytes("cscs\0");

            var file = new FormFile(
                baseStream: new MemoryStream(bytes),
                baseStreamOffset: 0,
                length: bytes.Length,
                name: "Data",
                fileName: "cs"
            );

            //act
            var result = Helper.IsFileTextBased(file);

            //assert
            Assert.False(result);
        }

        [Fact]
        public void ReadFileAsStringAsync_ReturnString()
        {
            //arrange
            var content = "cscs";
            byte[] bytes = Encoding.UTF8.GetBytes(content);

            var file = new FormFile(
                baseStream: new MemoryStream(bytes),
                baseStreamOffset: 0,
                length: bytes.Length,
                name: "Data",
                fileName: "cs"
            );

            //act
            var result = Helper.ReadFileAsStringAsync(file).Result;

            //assert
            Assert.Equal(result, content);
        }
        
    }
}