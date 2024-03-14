using NUnit.Framework;
using SimpleIRCLib;
using SimpleIRCLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace SimpleIRCTests
{
    public class SimpleIRCLibTests
    {
        private bool shallRun = false;
        string ip;
        int port;
        string username;
        string channel;

        [SetUp]
        public void Setup()
        {
            this.shallRun = HaveInternetConnection();
            ip = "irc.abjects.net";
            port = 6667;
            username = new StringBuilder().AppendJoin(null, GenerateRandomString(8)).ToString();
            channel = "#beast-xdcc";
        }

        [Test]
        public void ConstructorTest()
        {
            Assert.DoesNotThrow(() => { _ = new SimpleIRC(); });
        }

        [Test]
        public void SetupTest()
        {
            SimpleIRC s = new();
            Assert.DoesNotThrow(() => { s.SetupIrc(this.ip, this.username, this.channel, this.port, string.Empty, enableSSL: true, acceptAllCertificates: true); });
            Assert.DoesNotThrow(() => { s.SetupIrc(this.ip, this.username, this.channel, this.port, string.Empty, enableSSL: true, acceptAllCertificates: false); });
            Assert.DoesNotThrow(() => { s.SetupIrc(this.ip, this.username, this.channel, this.port, string.Empty, enableSSL: false, acceptAllCertificates: false); });
            Assert.DoesNotThrow(() => { s.SetupIrc(this.ip, this.username, this.channel, this.port, null, enableSSL: false, acceptAllCertificates: false); });
            Assert.DoesNotThrow(() => { s.SetupIrc(this.ip, this.username, this.channel, 22, null, enableSSL: false, acceptAllCertificates: false); });
            Assert.DoesNotThrow(() => { s.SetupIrc(this.ip, this.username, this.channel, 65535, null, enableSSL: false, acceptAllCertificates: false); });
            Assert.Throws<InvalidPortException>(() => { s.SetupIrc(this.ip, this.username, this.channel, 70521, null, enableSSL: false, acceptAllCertificates: false); });
        }

        [Test]
        public void ConnectAndDisconnect()
        {
            DecisionRunTest();

            SimpleIRC s = new();
            Assert.DoesNotThrow(() => { s.SetupIrc(this.ip, this.username, this.channel, this.port, string.Empty, 100, false, true); });
            Assert.DoesNotThrow(() => { Assert.That(s.StartClient(), Is.True); });
            Assert.That(s.IsClientRunning(), Is.True);

            Assert.DoesNotThrow(() => { s.Dispose(); });
        }

        [Test]
        public void ConnectAndDisconnectSSL()
        {
            DecisionRunTest();

            SimpleIRC s = new();
            Assert.DoesNotThrow(() => { s.SetupIrc(this.ip, this.username, this.channel, 9999, string.Empty, 100, true, true); });
            Assert.DoesNotThrow(() => { Assert.That(s.StartClient(), Is.True); });
            Assert.That(s.IsClientRunning(), Is.True);

            Assert.DoesNotThrow(() => { s.Dispose(); });
        }

        [TearDown]
        public void TearDown()
        {

        }

        #region Helper Functions
        private static IEnumerable<char> GenerateRandomString(int length)
        {
            Random r = new(BitConverter.ToInt32(Guid.NewGuid().ToByteArray()));
            for (int i = 0; i < length; i++)
            {
                yield return r.Next(0, 2) == 1 ? ((char)r.Next(97, 122)) : ((char)r.Next(65, 90));
            }
        }
        private void DecisionRunTest()
        {
            if (!shallRun)
            {
                Assert.Ignore("No internet connection...");
            }
        }

        private static bool HaveInternetConnection()
        {
            using (HttpClient client = new())
            {
                client.Timeout = TimeSpan.FromSeconds(2);

                HttpResponseMessage res = null;

                try
                {
                    res = client.GetAsync("https://google.com").Result;
                }
                catch (Exception)
                {
                    return false;
                }

                if (res.StatusCode != HttpStatusCode.OK)
                {
                    return false;
                }
            }

            return true;
        }
        #endregion
    }
}