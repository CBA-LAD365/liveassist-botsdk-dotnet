using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cafex.LiveAssist.Bot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Cafex.LiveAssist.Bot.Tests
{
    [TestClass()]
    public class ChatContextTests
    {
        [TestMethod()]
        public void ChatContextJsonSerializationTest()
        {
            ChatContext objectUnderTest = new ChatContext("SessionURL", "NextEvents");
            String serializedObject = JsonConvert.SerializeObject(objectUnderTest);
            ChatContext deserializedObject = JsonConvert.DeserializeObject<ChatContext>(serializedObject);
            Assert.AreEqual(objectUnderTest, deserializedObject);
        }

        [TestMethod()]
        public void ChatContextBinarySerializationTest()
        {
            ChatContext objectUnderTest = new ChatContext("SessionURL", "NextEvents");
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(memoryStream, objectUnderTest);
            memoryStream.Seek(0, SeekOrigin.Begin);
            ChatContext deserializedObject = (ChatContext) serializer.Deserialize(memoryStream);
            Assert.AreEqual(objectUnderTest, deserializedObject);
        }
    }
}
