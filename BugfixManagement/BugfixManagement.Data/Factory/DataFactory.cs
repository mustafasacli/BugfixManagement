namespace BugfixManagement.Data.Factory
{
    public class DataFactory
    {
        private static object lockObj = new object();
        private static DataFactory _fact = null;

        private DataFactory()
        {
        }

        public static DataFactory Instance
        {
            get
            {
                if (_fact == null)
                {
                    lock (lockObj)
                    {
                        if (_fact == null)
                        {
                            _fact = new DataFactory();
                        }
                    }
                }

                return _fact;
            }
        }
    }
}
