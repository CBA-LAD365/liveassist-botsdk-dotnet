using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cafex.LiveAssist.Bot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
    }
}