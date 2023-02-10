namespace Ao.Stock.Test
{
    [TestClass]
    public class DefaultCastHelperTest
    {
        [TestMethod]
        public void CastWithSame()
        {
            var res = DefaultCastHelper.Default.Convert(123, typeof(int), true);
            Assert.AreEqual(123, res);
        }
        [TestMethod]
        public void CastWithNormal()
        {
            var res = DefaultCastHelper.Default.Convert(123, typeof(decimal), true);
            Assert.AreEqual(123M, res);
        }
        [TestMethod]
        public void CastWithNullable()
        {
            int? a = 123;
            var res = DefaultCastHelper.Default.Convert(a, typeof(decimal), true);
            Assert.AreEqual(123M, res);
        }
        [TestMethod]
        public void CastWithNull()
        {
            int? a = null;
            var res = DefaultCastHelper.Default.Convert(a, typeof(decimal?), true);
            Assert.IsNull(res);
        }
    }
}
