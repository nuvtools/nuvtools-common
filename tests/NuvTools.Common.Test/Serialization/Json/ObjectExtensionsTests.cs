using NUnit.Framework;
using NuvTools.Common.Serialization.Json;

namespace NuvTools.Common.Tests.Serialization.Json
{

    class ModelTest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int YearBirth { get; set; }
        public ModelTest Cousin { get; set; }
    }

    [TestFixture()]
    public class ObjectExtensionsTests
    {

        private readonly ModelTest modelInstance = new()
        {
            Id = 2,
            Name = "Bruno Melo",
            Cousin = new ModelTest
            {
                Name = "J",
                YearBirth = 1968,
                Cousin = new ModelTest
                {
                    Name = "Julia",
                    YearBirth = 2010
                }
            }
        };
        private string serializedObject;

        [Test(), Order(1)]
        public void CopyTest()
        {
            var copiedObject = modelInstance.Clone(2);
            Assert.AreNotSame(modelInstance, copiedObject);
        }

        [Test(), Order(2)]
        public void SerializeTest()
        {
            serializedObject = modelInstance.Serialize(5);
            Assert.IsNotNull(serializedObject);
        }

        [Test(), Order(3)]
        public void DeserializeTest()
        {
            var copiedObject = serializedObject.Deserialize<ModelTest>(1);
            Assert.IsNotNull(copiedObject);
            Assert.AreSame(modelInstance, copiedObject);
        }
    }
}