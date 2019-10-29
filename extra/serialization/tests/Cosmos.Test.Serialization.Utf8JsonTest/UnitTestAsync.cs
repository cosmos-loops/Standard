using System;
using System.IO;
using System.Threading.Tasks;
using Cosmos.Serialization.Json;
using Xunit;

namespace Cosmos.Test.Serialization.Utf8JsonTest
{
    public class UnitTestAsync
    {
        [Fact]
        public async Task BytesTest()
        {
            var model = CreateNiceModel();
            var bytes = await model.ToUtf8JsonBytesAsync();
            var backs = await bytes.FromUtf8JsonBytesAsync<NiceModel>();

            Assert.Equal(
                Tuple.Create(model.Id, model.Name, model.NiceType, model.Count, model.CreatedTime, model.IsValid),
                Tuple.Create(backs.Id, backs.Name, backs.NiceType, backs.Count, backs.CreatedTime, backs.IsValid));
        }

        [Fact]
        public async Task NonGenericBytesTest()
        {
            var model = CreateNiceModel();
            var bytes = await model.ToUtf8JsonBytesAsync();
            var backs = (NiceModel) await bytes.FromUtf8JsonBytesAsync(typeof(NiceModel));

            Assert.Equal(
                Tuple.Create(model.Id, model.Name, model.NiceType, model.Count, model.CreatedTime, model.IsValid),
                Tuple.Create(backs.Id, backs.Name, backs.NiceType, backs.Count, backs.CreatedTime, backs.IsValid));
        }

        [Fact]
        public async Task StreamTest()
        {
            var model = CreateNiceModel();
            var stream1 = await model.Utf8JsonPackAsync();
            var stream2 = new MemoryStream();
            await model.Utf8JsonPackToAsync(stream2);
            var stream3 = new MemoryStream();
            await stream3.Utf8JsonPackByAsync(model);

            var back1 = await stream1.Utf8JsonUnpackAsync<NiceModel>();
            var back2 = await stream2.Utf8JsonUnpackAsync<NiceModel>();
            var back3 = await stream3.Utf8JsonUnpackAsync<NiceModel>();

            Assert.Equal(
                Tuple.Create(model.Id, model.Name, model.NiceType, model.Count, model.CreatedTime, model.IsValid),
                Tuple.Create(back1.Id, back1.Name, back1.NiceType, back1.Count, back1.CreatedTime, back1.IsValid));

            Assert.Equal(
                Tuple.Create(model.Id, model.Name, model.NiceType, model.Count, model.CreatedTime, model.IsValid),
                Tuple.Create(back2.Id, back2.Name, back2.NiceType, back2.Count, back2.CreatedTime, back2.IsValid));

            Assert.Equal(
                Tuple.Create(model.Id, model.Name, model.NiceType, model.Count, model.CreatedTime, model.IsValid),
                Tuple.Create(back3.Id, back3.Name, back3.NiceType, back3.Count, back3.CreatedTime, back3.IsValid));
        }

        [Fact]
        public async Task NonGenericStreamTest()
        {
            var model = CreateNiceModel();
            var stream1 = await model.Utf8JsonPackAsync();
            var stream2 = new MemoryStream();
            await model.Utf8JsonPackToAsync(stream2);
            var stream3 = new MemoryStream();
            await stream3.Utf8JsonPackByAsync(model);

            var back1 = (NiceModel) await stream1.Utf8JsonUnpackAsync(typeof(NiceModel));
            var back2 = (NiceModel) await stream2.Utf8JsonUnpackAsync(typeof(NiceModel));
            var back3 = (NiceModel) await stream3.Utf8JsonUnpackAsync(typeof(NiceModel));

            Assert.Equal(
                Tuple.Create(model.Id, model.Name, model.NiceType, model.Count, model.CreatedTime, model.IsValid),
                Tuple.Create(back1.Id, back1.Name, back1.NiceType, back1.Count, back1.CreatedTime, back1.IsValid));

            Assert.Equal(
                Tuple.Create(model.Id, model.Name, model.NiceType, model.Count, model.CreatedTime, model.IsValid),
                Tuple.Create(back2.Id, back2.Name, back2.NiceType, back2.Count, back2.CreatedTime, back2.IsValid));

            Assert.Equal(
                Tuple.Create(model.Id, model.Name, model.NiceType, model.Count, model.CreatedTime, model.IsValid),
                Tuple.Create(back3.Id, back3.Name, back3.NiceType, back3.Count, back3.CreatedTime, back3.IsValid));
        }

        [Fact]
        public async Task StringTest()
        {
            var model = CreateNiceModel();
            var json1 = await model.ToUtf8JsonAsync();
            var back1 = await json1.FromUtf8JsonAsync<NiceModel>();

            Assert.Equal(
                Tuple.Create(model.Id, model.Name, model.NiceType, model.Count, model.CreatedTime, model.IsValid),
                Tuple.Create(back1.Id, back1.Name, back1.NiceType, back1.Count, back1.CreatedTime, back1.IsValid));
        }

        [Fact]
        public async Task NonGenericStringTest()
        {
            var model = CreateNiceModel();
            var json1 = await model.ToUtf8JsonAsync();
            var back1 = (NiceModel) await json1.FromUtf8JsonAsync(typeof(NiceModel));

            Assert.Equal(
                Tuple.Create(model.Id, model.Name, model.NiceType, model.Count, model.CreatedTime, model.IsValid),
                Tuple.Create(back1.Id, back1.Name, back1.NiceType, back1.Count, back1.CreatedTime, back1.IsValid));
        }

        private static NiceModel CreateNiceModel()
        {
            return new NiceModel
            {
                Id = Guid.NewGuid(),
                Name = "nice",
                NiceType = NiceType.Yes,
                Count = new Random().Next(0, 100),
                CreatedTime = new DateTime(2019, 10, 1).ToUniversalTime(),
                IsValid = true
            };
        }
    }
}