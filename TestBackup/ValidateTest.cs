using Backup;

namespace TestBackup
{
    public class ValidateTest
    {
        [Fact]
        public void isSqlInstalled()
        {
            Validate valid = new Validate();

            var isInstalled = valid.isSqlServerInstalled();

            if (isInstalled.isSuccess)
            {
                Assert.True(isInstalled.isSuccess, isInstalled.message);
            }

            else
            {
                Assert.Fail(isInstalled.message);
            }
        }
    }
}
