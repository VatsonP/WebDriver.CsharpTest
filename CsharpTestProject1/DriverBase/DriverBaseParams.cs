
namespace CsharpTestProject1
{
    public class DriverBaseParams
    {
        // значение времени (в сек) общих неявных ожиданий
        public int drvImplWaitTime;
        // значение времени (в сек) для явных ожиданий
        public int drvExplWaitTime;
        // константа времени (в сек) для максимального времени неявного ожидания
        public int drvMaxWaitTime;

        public DriverBaseParams(int drvImplWaitTime = WebDriverExtensions.drvImplWaitTime,
                                int drvExplWaitTime = WebDriverExtensions.drvExplWaitTime,
                                int drvMaxWaitTime  = WebDriverExtensions.drvMaxWaitTime)
        {
            // значение времени (в сек) общих неявных ожиданий
            this.drvImplWaitTime = drvImplWaitTime;
            // значение времени (в сек) для явных ожиданий
            this.drvExplWaitTime = drvExplWaitTime;
            // значение времени (в сек) для максимального времени неявного ожидания
            this.drvMaxWaitTime = drvMaxWaitTime;
        }

    }
}
