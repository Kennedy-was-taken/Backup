using Backup;

namespace TestBackup
{
    public class ValidateTest
    {

        [Fact]
        public void isSqlInstalled()
        {

            var isInstalled = Validate.isSqlServerInstalled();

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
