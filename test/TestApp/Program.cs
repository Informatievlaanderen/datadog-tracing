namespace TestApp
{
    using System.Threading;
    using Be.Vlaanderen.Basisregisters.DataDog.Tracing;

    class Program
    {
        static void Main(string[] args)
        {
            var source = new TraceSource(42);
            var agent = new TraceAgent();
            using (source.Subscribe(agent))
            using (var span = source.Begin("sample-trace", "test-app", "Main", "console"))
            {
                TestSpan(span);
                TestSpan(span);
                TestSpan(span);
            }
            agent.OnCompleted();
            agent.Completion.Wait();
        }

        private static void TestSpan(ISpan span)
        {
            using (var child = span.Begin("GET something", "memcached", "GET", "cache"))
            {
                child.SetMeta("memcached_key", "test123");
                Thread.Sleep(100);
            }
        }
    }
}
